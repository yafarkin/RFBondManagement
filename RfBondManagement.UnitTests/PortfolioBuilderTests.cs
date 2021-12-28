using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RfBondManagement.Engine.Calculations;
using RfBondManagement.Engine.Interfaces;
using RfFondPortfolio.Common.Dtos;
using RfFondPortfolio.Common.Interfaces;
using Shouldly;

namespace RfBondManagement.UnitTests
{
    [TestClass]
    public class PortfolioBuilderTests
    {
        public IPaperRepository PaperRepository;
        public IExternalImport Import;
        public IPortfolioPaperActionRepository PaperActionRepository;
        public IPortfolioMoneyActionRepository MoneyActionRepository;
        public ISplitRepository SplitRepository;
        public Portfolio Portfolio;
        public IBondCalculator BondCalculator;
        public IExternalImportFactory ImportFactory;

        public SharePaper ShareSample;
        public BondPaper BondSample;

        public PortfolioCalculator Calculator;
        public IPortfolioBuilder Builder;
        public IPortfolioLogic Logic;

        [TestInitialize]
        public void Setup()
        {
            TestsHelper.Reset();

            var today = DateTime.UtcNow.Date;

            Portfolio = new Portfolio();

            ShareSample = new SharePaper {SecId = "S1", PaperType = PaperType.Share};
            BondSample = new BondPaper
            {
                SecId = "B2", PaperType = PaperType.Bond,
                MatDate = today.AddDays(1),
                InitialFaceValue = 1000,
                FaceValue = 1000,
                IssueDate = today.AddYears(-1),
                Coupons = new List<BondCoupon>
                {
                    new BondCoupon
                    {
                        CouponDate = today.AddYears(-1).AddDays(182),
                        Value = 50
                    },
                    new BondCoupon
                    {
                        CouponDate = today.AddDays(1),
                        Value = 50
                    }
                }
            };

            TestsHelper.Papers = new List<AbstractPaper> { ShareSample, BondSample };

            Import = TestsHelper.CreateExternalImport(true);
            BondCalculator = TestsHelper.GetBondCalculator();
            PaperRepository = TestsHelper.CreatePaperRepository();
            PaperActionRepository = TestsHelper.CreatePaperActionRepository();
            MoneyActionRepository = TestsHelper.CreateMoneyActionRepository();
            SplitRepository = TestsHelper.CreateSplitRepository();
            ImportFactory = TestsHelper.CreateExternalImportFactory();

            Builder = TestsHelper.CreateBuilder();
            Calculator = TestsHelper.CreateCalculator(Portfolio) as PortfolioCalculator;
            Logic = TestsHelper.CreateLogic(Portfolio);
        }

        [TestMethod]
        public async Task PortfolioEngine_Bond_SmokeTest()
        {
            TestsHelper.LastPrice = 105;
            Portfolio.Commissions = 0.61m;
            Portfolio.Tax = 13m;

            var when = DateTime.UtcNow.AddDays(-91);

            var paper = BondSample;
            Logic.ApplyActions(Calculator.MoveMoney(2500, MoneyActionType.IncomeExternal, "пополнение счёта", null, when));
            Logic.ApplyActions(Calculator.BuyPaper(paper, 1, 95, when));
            Logic.ApplyActions(Calculator.BuyPaper(paper, 1, 98, when));

            when = DateTime.UtcNow;
            Logic.ApplyActions(Calculator.SellPaper(paper, 1, 108, when));

            var content = Builder.Build(Portfolio.Id);
            await Logic.GetPrice(content);

            content.ShouldNotBeNull();
            content.Sums.Count.ShouldBe(7);
            content.Sums[MoneyActionType.IncomeExternal].ShouldBe(2500);
            content.Sums[MoneyActionType.IncomeSellOnMarket].ShouldBe(1080);
            content.Sums[MoneyActionType.IncomeAci].ShouldBe(49.73m);
            content.Sums[MoneyActionType.OutcomeBuyOnMarket].ShouldBe(950 + 980);
            content.Sums[MoneyActionType.OutcomeAci].ShouldBe(25 + 25);
            content.Sums[MoneyActionType.OutcomeCommission].ShouldBe(18.969353m);
            content.Sums[MoneyActionType.DraftProfit].ShouldBe(130);
            content.AvailSum.ShouldBe(1630.760647m);
            content.Profit.ShouldBe(70);
        }

        [TestMethod]
        public void PayTaxByDraftProfit_Test()
        {
            TestsHelper.LastPrice = 105;
            Portfolio.Commissions = 0.61m;
            Portfolio.Tax = 13m;

            var when = DateTime.UtcNow.AddDays(-91);

            var paper = BondSample;
            Logic.ApplyActions(Calculator.MoveMoney(2500, MoneyActionType.IncomeExternal, "пополнение счёта", null, when));
            Logic.ApplyActions(Calculator.BuyPaper(paper, 1, 95, when));

            when = DateTime.UtcNow;
            Logic.ApplyActions(Calculator.SellPaper(paper, 1, 108, when));

            var content = Builder.Build(Portfolio.Id);

            content.ShouldNotBeNull();
            content.Sums[MoneyActionType.DraftProfit].ShouldBe(130);
            content.Sums.ContainsKey(MoneyActionType.OutcomeTax).ShouldBeFalse();

            var moneyActions = Calculator.PayTaxByDraftProfit(130).OfType<PortfolioMoneyAction>().ToList();
            Logic.ApplyActions(moneyActions);
            moneyActions.Count.ShouldBe(2);
            moneyActions[0].MoneyAction.ShouldBe(MoneyActionType.OutcomeTax);
            moneyActions[0].Sum.ShouldBe(16.9m);
            moneyActions[1].MoneyAction.ShouldBe(MoneyActionType.DraftProfit);
            moneyActions[1].Sum.ShouldBe(-130);

            content = Builder.Build(Portfolio.Id);

            content.ShouldNotBeNull();
            content.Sums[MoneyActionType.DraftProfit].ShouldBe(0);
            content.Sums[MoneyActionType.OutcomeTax].ShouldBe(16.9m);
        }

        [TestMethod]
        public void PortfolioEngine_AutomateDividends_Test()
        {
            var onDate = DateTime.UtcNow.Date;

            ShareSample.Dividends = new List<ShareDividend>
            {
                new ShareDividend
                {
                    RegistryCloseDate = onDate.AddDays(1),
                    Value = 10
                },
                new ShareDividend
                {
                    RegistryCloseDate = onDate.AddDays(180),
                    Value = 20
                }
            };

            Portfolio.Tax = 10;

            Logic.ApplyActions(Calculator.BuyPaper(ShareSample, 1, 100, onDate));

            var p = Calculator.AutomateDividend(onDate, new[] {ShareSample.SecId}).ToList();
            Logic.ApplyActions(p);
            p.ShouldBeEmpty();

            p = Calculator.AutomateDividend(onDate.AddDays(1), new[] {ShareSample.SecId}).ToList();
            Logic.ApplyActions(p);
            p.Count.ShouldBe(3);
            p[0].ShouldBeOfType<PortfolioPaperAction>();
            (p[0] as PortfolioPaperAction).Value.ShouldBe(10);
            p[1].ShouldBeOfType<PortfolioMoneyAction>();
            (p[1] as PortfolioMoneyAction).MoneyAction.ShouldBe(MoneyActionType.IncomeDividend);
            (p[1] as PortfolioMoneyAction).Sum.ShouldBe(10);
            p[2].ShouldBeOfType<PortfolioMoneyAction>();
            (p[2] as PortfolioMoneyAction).MoneyAction.ShouldBe(MoneyActionType.OutcomeTax);
            (p[2] as PortfolioMoneyAction).Sum.ShouldBe(1);
        }

        [TestMethod]
        public void PortfolioEngine_AutomateCoupons_Test()
        {
            var onDate = DateTime.UtcNow.Date;

            BondSample.Coupons = new List<BondCoupon>
            {
                new BondCoupon
                {
                    CouponDate = onDate.AddDays(1),
                    Value = 10
                },
                new BondCoupon
                {
                    CouponDate = onDate.AddDays(180),
                    Value = 20
                }
            };

            Portfolio.Tax = 10;

            Logic.ApplyActions(Calculator.BuyPaper(BondSample, 1, 100, onDate));

            var p = Calculator.AutomateCoupons(onDate, new[] {BondSample.SecId}).ToList();
            Logic.ApplyActions(p);
            p.ShouldBeEmpty();

            p = Calculator.AutomateCoupons(onDate.AddDays(1), new[] {BondSample.SecId}).ToList();
            Logic.ApplyActions(p);
            p.Count.ShouldBe(3);
            p[0].ShouldBeOfType<PortfolioPaperAction>();
            (p[0] as PortfolioPaperAction).Value.ShouldBe(10);
            p[1].ShouldBeOfType<PortfolioMoneyAction>();
            (p[1] as PortfolioMoneyAction).MoneyAction.ShouldBe(MoneyActionType.IncomeDividend);
            (p[1] as PortfolioMoneyAction).Sum.ShouldBe(10);
            p[2].ShouldBeOfType<PortfolioMoneyAction>();
            (p[2] as PortfolioMoneyAction).MoneyAction.ShouldBe(MoneyActionType.OutcomeTax);
            (p[2] as PortfolioMoneyAction).Sum.ShouldBe(1);
        }

        [TestMethod]
        public void PortfolioEngine_AutomateBondClose_Test()
        {
            var onDate = DateTime.UtcNow;

            BondSample.Coupons = new List<BondCoupon>
            {
                new BondCoupon
                {
                    CouponDate = onDate.AddDays(1),
                    Value = 10
                },
                new BondCoupon
                {
                    CouponDate = onDate.AddDays(180),
                    Value = 20
                }
            };

            Portfolio.Tax = 10;

            Logic.ApplyActions(Calculator.BuyPaper(BondSample, 1, 95, onDate));

            var p = Calculator.AutomateBondCloseDate(onDate, new[] {BondSample.SecId}).ToList();
            p.ShouldBeEmpty();

            p = Calculator.AutomateBondCloseDate(onDate.AddDays(1), new[] {BondSample.SecId}).ToList();
            Logic.ApplyActions(p);
            p.Count.ShouldBe(3);
            p[2].ShouldBeOfType<PortfolioPaperAction>();
            (p[2] as PortfolioPaperAction).PaperAction.ShouldBe(PaperActionType.Close);
            (p[2] as PortfolioPaperAction).Count.ShouldBe(1);
            (p[2] as PortfolioPaperAction).Value.ShouldBe(100);

            var m = TestsHelper.Actions.OfType<PortfolioMoneyAction>().ToList();
            m.Count.ShouldBe(4);
            m[0].MoneyAction.ShouldBe(MoneyActionType.OutcomeBuyOnMarket);
            m[0].Sum.ShouldBe(950);
            m[1].MoneyAction.ShouldBe(MoneyActionType.OutcomeAci);
            m[1].Sum.ShouldBe(9.97m);
            m[2].MoneyAction.ShouldBe(MoneyActionType.IncomeSellOnMarket);
            m[2].Sum.ShouldBe(1000);
            m[3].MoneyAction.ShouldBe(MoneyActionType.DraftProfit);
            m[3].Sum.ShouldBe(50);
        }

        [TestMethod]
        public void PortfolioEngine_AutomateSplit_Test()
        {
            var onDate = DateTime.UtcNow.Date;

            TestsHelper.Splits.Add(new PaperSplit
            {
                Date = onDate.AddDays(-1),
                Multiplier = 10,
                SecId = ShareSample.SecId
            });

            var p = Calculator.AutomateSplit(onDate.AddDays(-2), new[] {ShareSample.SecId});
            p.ShouldBeEmpty();

            p = Calculator.AutomateSplit(onDate.AddDays(-1), new[] {ShareSample.SecId});
            p.Count().ShouldBe(1);
        }

        [TestMethod]
        public void PortfolioEngine_Split_SmokeTest()
        {
            var onDate = DateTime.UtcNow.Date;

            TestsHelper.Splits.Add(new PaperSplit
            {
                Date = onDate.AddDays(-1),
                Multiplier = 10,
                SecId = ShareSample.SecId
            });

            Logic.ApplyActions(Calculator.BuyPaper(ShareSample, 5, 25, onDate.AddDays(-3)));
            Logic.ApplyActions(Calculator.SellPaper(ShareSample, 1, 30, onDate.AddDays(-2)));

            var automate = Calculator.AutomateSplit(onDate.AddDays(-1), new[] {ShareSample.SecId});
            automate.ShouldNotBeEmpty();
            Logic.ApplyActions(automate);

            Logic.ApplyActions(Calculator.BuyPaper(ShareSample, 50, 2.6m, onDate.AddDays(-1)));
            Logic.ApplyActions(Calculator.SellPaper(ShareSample, 25, 3.3m, onDate));

            var actions = TestsHelper.Actions.OfType<PortfolioPaperAction>().ToList();

            var p = Builder.BuildPaperInPortfolio(ShareSample, actions, onDate.AddDays(-4));
            p.Count.ShouldBe(0);

            p = Builder.BuildPaperInPortfolio(ShareSample, actions, onDate.AddDays(-3));
            p.Count.ShouldBe(5);
            p.Actions.Count.ShouldBe(1);
            p.AveragePrice.ShouldBe(25);
            
            p = Builder.BuildPaperInPortfolio(ShareSample, actions, onDate.AddDays(-2));
            p.Count.ShouldBe(4);
            p.Actions.Count.ShouldBe(2);
            p.AveragePrice.ShouldBe(25);

            p = Builder.BuildPaperInPortfolio(ShareSample, actions, onDate.AddDays(-1));
            p.Count.ShouldBe(90);
            p.Actions.Count.ShouldBe(4);
            p.AveragePrice.ShouldBe(2.56m);

            p = Builder.BuildPaperInPortfolio(ShareSample, actions, onDate);
            p.Count.ShouldBe(65);
            p.Actions.Count.ShouldBe(5);
            p.AveragePrice.ShouldBe(2.58m);

            p = Builder.BuildPaperInPortfolio(ShareSample, actions);
            p.Count.ShouldBe(65);
            p.Actions.Count.ShouldBe(5);
            p.AveragePrice.ShouldBe(2.58m);

            p = Builder.BuildPaperInPortfolio(ShareSample, actions, onDate.AddDays(1));
            p.Count.ShouldBe(65);
            p.Actions.Count.ShouldBe(5);
            p.AveragePrice.ShouldBe(2.58m);
        }

        [TestMethod]
        public async Task PortfolioEngine_Share_SmokeTest()
        {
            TestsHelper.LastPrice = 300;
            Portfolio.Commissions = 0.61m;
            Portfolio.Tax = 13m;

            var paper = PaperRepository.Get().First();
            Logic.ApplyActions(Calculator.MoveMoney(1000, MoneyActionType.IncomeExternal, "пополнение счёта"));
            Logic.ApplyActions(Calculator.BuyPaper(paper, 1, 100));
            Logic.ApplyActions(Calculator.BuyPaper(paper, 1, 200));
            Logic.ApplyActions(Calculator.SellPaper(paper, 1, 250));

            var content = Builder.Build(Portfolio.Id);
            await Logic.GetPrice(content);

            content.ShouldNotBeNull();
            content.Sums.Count.ShouldBe(5);
            content.Sums[MoneyActionType.IncomeExternal].ShouldBe(1000);
            content.Sums[MoneyActionType.IncomeSellOnMarket].ShouldBe(250);
            content.Sums[MoneyActionType.OutcomeBuyOnMarket].ShouldBe(300);
            content.Sums[MoneyActionType.OutcomeCommission].ShouldBe(3.355m);
            content.Sums[MoneyActionType.DraftProfit].ShouldBe(150);

            content.AvailSum.ShouldBe(946.645m);
            content.Profit.ShouldBe(100);

            var papers = content.Papers.ToList();
            papers.Count.ShouldBe(1);

            var paperActions = papers[0].Actions.ToList();
            paperActions.Count.ShouldBe(3);
            paperActions[0].PaperAction.ShouldBe(PaperActionType.Buy);
            paperActions[0].Count.ShouldBe(1);
            paperActions[0].Value.ShouldBe(100);
            paperActions[1].PaperAction.ShouldBe(PaperActionType.Buy);
            paperActions[1].Count.ShouldBe(1);
            paperActions[1].Value.ShouldBe(200);
            paperActions[2].PaperAction.ShouldBe(PaperActionType.Sell);
            paperActions[2].Count.ShouldBe(1);
            paperActions[2].Value.ShouldBe(250);

            var fifo = papers[0].FifoActions.ToList();
            fifo.Count.ShouldBe(2);
            fifo[0].Item1.PaperAction.ShouldBe(PaperActionType.Buy);
            fifo[0].Item1.Sum.ShouldBe(100);
            fifo[0].Item2.PaperAction.ShouldBe(PaperActionType.Sell);
            fifo[0].Item2.Sum.ShouldBe(250);
            fifo[0].Item3.ShouldBe(0);
            fifo[1].Item1.PaperAction.ShouldBe(PaperActionType.Buy);
            fifo[1].Item1.Sum.ShouldBe(200);
            fifo[1].Item2.ShouldBe(null);
            fifo[1].Item3.ShouldBe(1);
        }

        [TestMethod]
        public void BuildPaperFifo_SmokeTest()
        {
            TestsHelper.Actions.Add(new PortfolioPaperAction {PaperAction = PaperActionType.Buy, Count = 1, SecId = "S1", Sum = 100, Value = 100});
            TestsHelper.Actions.Add(new PortfolioPaperAction {PaperAction = PaperActionType.Buy, Count = 1, SecId = "S1", Sum = 200, Value = 200});
            TestsHelper.Actions.Add(new PortfolioPaperAction {PaperAction = PaperActionType.Sell, Count = 1, SecId = "S1", Sum = 250, Value = 250});

            var paper = PaperRepository.Get().First();
            var paperInPortfolio = Builder.BuildPaperInPortfolio(paper, TestsHelper.Actions.OfType<PortfolioPaperAction>());

            paperInPortfolio.ShouldNotBeNull();
            paperInPortfolio.AveragePrice.ShouldBe(200);
            paperInPortfolio.Count.ShouldBe(1);
            paperInPortfolio.OnDate.ShouldBeNull();
            paperInPortfolio.Paper.ShouldBe(paper);
            paperInPortfolio.Actions.Count.ShouldBe(3);

            var fifo = paperInPortfolio.FifoActions.ToList();
            fifo.ShouldNotBeNull();
            fifo[0].Item1.ShouldBe(TestsHelper.Actions[0]);
            fifo[0].Item2.ShouldBe(TestsHelper.Actions[2]);
            fifo[0].Item3.ShouldBe(0);
            fifo[1].Item1.ShouldBe(TestsHelper.Actions[1]);
            fifo[1].Item2.ShouldBeNull();
            fifo[1].Item3.ShouldBe(1);
        }

        [TestMethod]
        public void BuildFifio_SmokeTest2()
        {
            // основано на примере 1 из https://journal.open-broker.ru/taxes/chto-takoe-fifo/
            TestsHelper.Actions.Add(new PortfolioPaperAction {PaperAction = PaperActionType.Buy, Count = 15, SecId = "S1", Value = 50});
            TestsHelper.Actions.Add(new PortfolioPaperAction {PaperAction = PaperActionType.Buy, Count = 30, SecId = "S1", Value = 80});
            TestsHelper.Actions.Add(new PortfolioPaperAction {PaperAction = PaperActionType.Sell, Count = 20, SecId = "S1", Value = 75});

            var paper = PaperRepository.Get().First();
            var paperInPortfolio = Builder.BuildPaperInPortfolio(paper, TestsHelper.Actions.OfType<PortfolioPaperAction>());

            paperInPortfolio.ShouldNotBeNull();
            paperInPortfolio.AveragePrice.ShouldBe(80);

            var fifo = paperInPortfolio.FifoActions.ToList();
            fifo.ShouldNotBeNull();
            fifo.Count.ShouldBe(2);
            fifo[0].Item1.ShouldBe(TestsHelper.Actions[0]);
            fifo[0].Item2.ShouldBe(TestsHelper.Actions[2]);
            fifo[0].Item3.ShouldBe(0);
            fifo[1].Item1.ShouldBe(TestsHelper.Actions[1]);
            fifo[1].Item2.ShouldBe(TestsHelper.Actions[2]);
            fifo[1].Item3.ShouldBe(25);
        }
    }
}