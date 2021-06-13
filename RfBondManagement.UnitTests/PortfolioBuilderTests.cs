using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
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

        [SetUp]
        public void Setup()
        {
            Portfolio = new Portfolio();
            Actions = new List<PortfolioAction>();

            var paperRepositoryMock = new Mock<IPaperRepository>();
            paperRepositoryMock.Setup(m => m.Get()).Returns(new List<AbstractPaper> {new BondPaper {SecId = "S1", PaperType = PaperType.Bond}});

            var moneyActionRepositoryMock = new Mock<IPortfolioMoneyActionRepository>();
            moneyActionRepositoryMock.Setup(m => m.Get()).Returns(Actions.OfType<PortfolioMoneyAction>());

            var paperActionRepositoryMock = new Mock<IPortfolioPaperActionRepository>();
            paperActionRepositoryMock.Setup(m => m.Get()).Returns(Actions.OfType<PortfolioPaperAction>());

            BondCalculator = new BondCalculator();

            Import = new Mock<IExternalImport>().Object;
            PaperRepository = paperRepositoryMock.Object;
            PaperActionRepository = paperActionRepositoryMock.Object;
            MoneyActionRepository = moneyActionRepositoryMock.Object;

            PortfolioEngine = new PortfolioEngine(Portfolio, Import, PaperRepository, MoneyActionRepository, PaperActionRepository, BondCalculator);
        }

        [Test]
        public async Task SmokeTest()
        {
            Actions.Add(new PortfolioMoneyAction {MoneyAction = MoneyActionType.IncomeExternal, Sum = 1000});
            Actions.Add(new PortfolioPaperAction {PaperAction = PaperActionType.Buy, Count = 1, SecId = "S1", Sum = 100, Value = 100});
            Actions.Add(new PortfolioMoneyAction {MoneyAction = MoneyActionType.OutcomeBuyOnMarket, Sum = 100});
            Actions.Add(new PortfolioMoneyAction {MoneyAction = MoneyActionType.OutcomeCommission, Sum = 0.61m});
            Actions.Add(new PortfolioPaperAction {PaperAction = PaperActionType.Buy, Count = 1, SecId = "S1", Sum = 200, Value = 200});
            Actions.Add(new PortfolioMoneyAction {MoneyAction = MoneyActionType.OutcomeBuyOnMarket, Sum = 200});
            Actions.Add(new PortfolioMoneyAction {MoneyAction = MoneyActionType.OutcomeCommission, Sum = 1.22m});
            Actions.Add(new PortfolioPaperAction {PaperAction = PaperActionType.Sell, Count = 1, SecId = "S1", Sum = 250, Value = 250});
            Actions.Add(new PortfolioMoneyAction {MoneyAction = MoneyActionType.IncomeSellOnMarket, Sum = 250});
            Actions.Add(new PortfolioMoneyAction {MoneyAction = MoneyActionType.OutcomeCommission, Sum = 1.525m});
            Actions.Add(new PortfolioMoneyAction {MoneyAction = MoneyActionType.OutcomeDelayTax, Sum = 19.5m});

            var content = await PortfolioEngine.Build();

            content.ShouldNotBeNull();
            content.Sums.Count.ShouldBe(5);
            content.Sums[MoneyActionType.IncomeExternal].ShouldBe(1000);
            content.Sums[MoneyActionType.IncomeSellOnMarket].ShouldBe(250);
            content.Sums[MoneyActionType.OutcomeBuyOnMarket].ShouldBe(300);
            content.Sums[MoneyActionType.OutcomeCommission].ShouldBe(3.355m);
            content.Sums[MoneyActionType.OutcomeDelayTax].ShouldBe(19.5m);

            content.Papers.Count.ShouldBe(1);
            var paper = content.Papers[0];
            paper.Paper.SecId.ShouldBe("S1");
            paper.Count.ShouldBe(1);
            paper.AveragePrice.ShouldBe(200);

            var paperActions = new List<PortfolioPaperAction>(paper.Actions);
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
        }

        [Test]
        public async Task SmokeTest2()
        {
            // основано на примере 1 из https://journal.open-broker.ru/taxes/chto-takoe-fifo/
            Actions.Add(new PortfolioPaperAction {PaperAction = PaperActionType.Buy, Count = 15, SecId = "S1", Value = 50});
            Actions.Add(new PortfolioPaperAction {PaperAction = PaperActionType.Buy, Count = 30, SecId = "S1", Value = 80});
            Actions.Add(new PortfolioPaperAction {PaperAction = PaperActionType.Sell, Count = 20, SecId = "S1", Value = 75});

            var content = await PortfolioEngine.Build();

            content.ShouldNotBeNull();

            content.Papers.Count.ShouldBe(1);
            var paper = content.Papers[0];
            paper.Paper.SecId.ShouldBe("S1");
            paper.Count.ShouldBe(25);
            paper.AveragePrice.ShouldBe(80);
        }
    }
}