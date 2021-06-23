using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NLog;
using NUnit.Framework;
using RfBondManagement.Engine.Calculations;
using RfBondManagement.Engine.Interfaces;
using RfFondPortfolio.Common.Dtos;
using RfFondPortfolio.Common.Interfaces;
using Shouldly;

namespace RfBondManagement.UnitTests
{
    [TestFixture]
    public class PortfolioBuilderTests
    {
        public PortfolioEngine PortfolioEngine;
        public IPaperRepository PaperRepository;
        public IExternalImport Import;
        public IPortfolioPaperActionRepository PaperActionRepository;
        public IPortfolioMoneyActionRepository MoneyActionRepository;
        public Portfolio Portfolio;
        public IBondCalculator BondCalculator;

        public List<PortfolioAction> Actions;

        public decimal LastPrice;

        public AbstractPaper ShareSample;
        public AbstractPaper BondSample;

        [SetUp]
        public void Setup()
        {
            Portfolio = new Portfolio();
            Actions = new List<PortfolioAction>();

            ShareSample = new SharePaper {SecId = "S1", PaperType = PaperType.Share};
            BondSample = new BondPaper
            {
                SecId = "B2", PaperType = PaperType.Bond,
                MatDate = DateTime.Today.AddDays(1),
                InitialFaceValue = 1000,
                FaceValue = 1000,
                IssueDate = DateTime.Today.AddYears(-1),
                Coupons = new List<BondCoupon>
                {
                    new BondCoupon
                    {
                        CouponDate = DateTime.Today.AddYears(-1).AddDays(182),
                        Value = 50
                    },
                    new BondCoupon
                    {
                        CouponDate = DateTime.Today.AddDays(1),
                        Value = 50
                    }
                }
            };

            var paperRepositoryMock = new Mock<IPaperRepository>();
            paperRepositoryMock.Setup(m => m.Get()).Returns(new List<AbstractPaper> {ShareSample, BondSample});

            var moneyActionRepositoryMock = new Mock<IPortfolioMoneyActionRepository>();
            moneyActionRepositoryMock.Setup(m => m.Get()).Returns(Actions.OfType<PortfolioMoneyAction>());
            moneyActionRepositoryMock.Setup(m => m.Insert(It.IsAny<PortfolioMoneyAction>()))
                .Callback<PortfolioMoneyAction>((i) =>
                {
                    i.Id = Guid.NewGuid();
                    Actions.Add(i);
                }).Returns(() => Actions.LastOrDefault() as PortfolioMoneyAction);

            var paperActionRepositoryMock = new Mock<IPortfolioPaperActionRepository>();
            paperActionRepositoryMock.Setup(m => m.Get()).Returns(Actions.OfType<PortfolioPaperAction>());
            paperActionRepositoryMock.Setup(m => m.Insert(It.IsAny<PortfolioPaperAction>()))
                .Callback<PortfolioPaperAction>((i) =>
                {
                    i.Id = Guid.NewGuid();
                    Actions.Add(i);
                }).Returns(() => Actions.LastOrDefault() as PortfolioPaperAction);

            BondCalculator = new BondCalculator();

            var importMock = new Mock<IExternalImport>();
            importMock
                .Setup(m => m.LastPrice(It.IsAny<ILogger>(), It.IsAny<AbstractPaper>()))
                .Returns(() => Task.FromResult(new PaperPrice {Price = LastPrice}));
            Import = importMock.Object;
            PaperRepository = paperRepositoryMock.Object;
            PaperActionRepository = paperActionRepositoryMock.Object;
            MoneyActionRepository = moneyActionRepositoryMock.Object;

            var logger = new Mock<ILogger>().Object;

            PortfolioEngine = new PortfolioEngine(Portfolio, Import, PaperRepository, MoneyActionRepository, PaperActionRepository, BondCalculator, logger);
        }

        [Test]
        public async Task PortfolioEngine_Bond_SmokeTest()
        {
            LastPrice = 105;
            Portfolio.Commissions = 0.61m;
            Portfolio.Tax = 13m;

            var when = DateTime.UtcNow.AddDays(-91);

            var paper = BondSample;
            PortfolioEngine.MoveMoney(2500, MoneyActionType.IncomeExternal, "пополнение счёта", null, when);
            PortfolioEngine.BuyPaper(paper, 1, 95, when);
            PortfolioEngine.BuyPaper(paper, 1, 98, when);

            when = DateTime.UtcNow;
            PortfolioEngine.SellPaper(paper, 1, 108, when);

            var content = PortfolioEngine.Build();
            await PortfolioEngine.FillPrice(content);

            content.ShouldNotBeNull();
            content.Sums.Count.ShouldBe(7);
            content.Sums[MoneyActionType.IncomeExternal].ShouldBe(2500);
            content.Sums[MoneyActionType.IncomeSellOnMarket].ShouldBe(1080);
            content.Sums[MoneyActionType.IncomeAci].ShouldBe(49.73m);
            content.Sums[MoneyActionType.OutcomeBuyOnMarket].ShouldBe(950 + 980);
            content.Sums[MoneyActionType.OutcomeAci].ShouldBe(25 + 25);
            content.Sums[MoneyActionType.OutcomeCommission].ShouldBe(18.969353m);
            content.Sums[MoneyActionType.OutcomeDelayTax].ShouldBe(16.9m);
            content.AvailSum.ShouldBe(1630.760647m);
            content.Profit.ShouldBe(70);
        }

        [Test]
        public async Task PortfolioEngine_Share_SmokeTest()
        {
            LastPrice = 300;
            Portfolio.Commissions = 0.61m;
            Portfolio.Tax = 13m;

            var paper = PaperRepository.Get().First();
            PortfolioEngine.MoveMoney(1000, MoneyActionType.IncomeExternal, "пополнение счёта");
            PortfolioEngine.BuyPaper(paper, 1, 100);
            PortfolioEngine.BuyPaper(paper, 1, 200);
            PortfolioEngine.SellPaper(paper, 1, 250);

            var content = PortfolioEngine.Build();
            await PortfolioEngine.FillPrice(content);

            content.ShouldNotBeNull();
            content.Sums.Count.ShouldBe(5);
            content.Sums[MoneyActionType.IncomeExternal].ShouldBe(1000);
            content.Sums[MoneyActionType.IncomeSellOnMarket].ShouldBe(250);
            content.Sums[MoneyActionType.OutcomeBuyOnMarket].ShouldBe(300);
            content.Sums[MoneyActionType.OutcomeCommission].ShouldBe(3.355m);
            content.Sums[MoneyActionType.OutcomeDelayTax].ShouldBe(19.5m);

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

        [Test]
        public void BuildPaperFifo_SmokeTest()
        {
            Actions.Add(new PortfolioPaperAction {PaperAction = PaperActionType.Buy, Count = 1, SecId = "S1", Sum = 100, Value = 100});
            Actions.Add(new PortfolioPaperAction {PaperAction = PaperActionType.Buy, Count = 1, SecId = "S1", Sum = 200, Value = 200});
            Actions.Add(new PortfolioPaperAction {PaperAction = PaperActionType.Sell, Count = 1, SecId = "S1", Sum = 250, Value = 250});

            var paper = PaperRepository.Get().First();
            var paperInPortfolio = PortfolioEngine.BuildPaperInPortfolio(paper, Actions);

            paperInPortfolio.ShouldNotBeNull();
            paperInPortfolio.AveragePrice.ShouldBe(200);
            paperInPortfolio.Count.ShouldBe(1);
            paperInPortfolio.OnDate.ShouldBeNull();
            paperInPortfolio.Paper.ShouldBe(paper);
            paperInPortfolio.Actions.Count.ShouldBe(3);

            var fifo = paperInPortfolio.FifoActions.ToList();
            fifo.ShouldNotBeNull();
            fifo[0].Item1.ShouldBe(Actions[0]);
            fifo[0].Item2.ShouldBe(Actions[2]);
            fifo[0].Item3.ShouldBe(0);
            fifo[1].Item1.ShouldBe(Actions[1]);
            fifo[1].Item2.ShouldBeNull();
            fifo[1].Item3.ShouldBe(1);
        }

        [Test]
        public async Task BuildFifio_SmokeTest2()
        {
            // основано на примере 1 из https://journal.open-broker.ru/taxes/chto-takoe-fifo/
            Actions.Add(new PortfolioPaperAction {PaperAction = PaperActionType.Buy, Count = 15, SecId = "S1", Value = 50});
            Actions.Add(new PortfolioPaperAction {PaperAction = PaperActionType.Buy, Count = 30, SecId = "S1", Value = 80});
            Actions.Add(new PortfolioPaperAction {PaperAction = PaperActionType.Sell, Count = 20, SecId = "S1", Value = 75});

            var paper = PaperRepository.Get().First();
            var paperInPortfolio = PortfolioEngine.BuildPaperInPortfolio(paper, Actions);

            paperInPortfolio.ShouldNotBeNull();
            paperInPortfolio.AveragePrice.ShouldBe(80);

            var fifo = paperInPortfolio.FifoActions.ToList();
            fifo.ShouldNotBeNull();
            fifo.Count.ShouldBe(2);
            fifo[0].Item1.ShouldBe(Actions[0]);
            fifo[0].Item2.ShouldBe(Actions[2]);
            fifo[0].Item3.ShouldBe(0);
            fifo[1].Item1.ShouldBe(Actions[1]);
            fifo[1].Item2.ShouldBe(Actions[2]);
            fifo[1].Item3.ShouldBe(25);
        }
    }
}