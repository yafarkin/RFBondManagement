using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using RfFondPortfolio.Common.Dtos;
using RfFondPortfolio.Common.Interfaces;
using RfFondPortfolio.Common.Logic;
using Shouldly;

namespace RfBondManagement.UnitTests
{
    [TestFixture]
    public class PortfolioBuilderTests
    {
        public IPaperRepository PaperRepository;

        [SetUp]
        public void Setup()
        {
            var paperRepositoryMock = new Mock<IPaperRepository>();
            paperRepositoryMock.Setup(m => m.Get()).Returns(new List<AbstractPaper> {new BondPaper {SecId = "S1"}});
            PaperRepository = paperRepositoryMock.Object;
        }

        [Test]
        public void SmokeTest()
        {
            var actions = new List<PortfolioAction>();
            actions.Add(new PortfolioMoneyAction {MoneyAction = MoneyActionType.IncomeExternal, Sum = 1000});
            actions.Add(new PortfolioPaperAction {PaperAction = PaperActionType.Buy, Count = 1, SecId = "S1", Sum = 100, Value = 100});
            actions.Add(new PortfolioMoneyAction {MoneyAction = MoneyActionType.OutcomeBuyOnMarket, Sum = 100});
            actions.Add(new PortfolioMoneyAction {MoneyAction = MoneyActionType.OutcomeCommission, Sum = 0.61m});
            actions.Add(new PortfolioPaperAction {PaperAction = PaperActionType.Buy, Count = 1, SecId = "S1", Sum = 200, Value = 200});
            actions.Add(new PortfolioMoneyAction {MoneyAction = MoneyActionType.OutcomeBuyOnMarket, Sum = 200});
            actions.Add(new PortfolioMoneyAction {MoneyAction = MoneyActionType.OutcomeCommission, Sum = 1.22m});
            actions.Add(new PortfolioPaperAction {PaperAction = PaperActionType.Sell, Count = 1, SecId = "S1", Sum = 250, Value = 250});
            actions.Add(new PortfolioMoneyAction {MoneyAction = MoneyActionType.IncomeSellOnMarket, Sum = 250});
            actions.Add(new PortfolioMoneyAction {MoneyAction = MoneyActionType.OutcomeCommission, Sum = 1.525m});
            actions.Add(new PortfolioMoneyAction {MoneyAction = MoneyActionType.OutcomeDelayTax, Sum = 19.5m});

            var content = PortfolioBuilder.Build(actions, PaperRepository);

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
        public void SmokeTest2()
        {
            // основано на примере 1 из https://journal.open-broker.ru/taxes/chto-takoe-fifo/
            var actions = new List<PortfolioAction>();
            actions.Add(new PortfolioPaperAction {PaperAction = PaperActionType.Buy, Count = 15, SecId = "S1", Value = 50});
            actions.Add(new PortfolioPaperAction {PaperAction = PaperActionType.Buy, Count = 30, SecId = "S1", Value = 80});
            actions.Add(new PortfolioPaperAction {PaperAction = PaperActionType.Sell, Count = 20, SecId = "S1", Value = 75});

            var content = PortfolioBuilder.Build(actions, PaperRepository);

            content.ShouldNotBeNull();

            content.Papers.Count.ShouldBe(1);
            var paper = content.Papers[0];
            paper.Paper.SecId.ShouldBe("S1");
            paper.Count.ShouldBe(25);
            paper.AveragePrice.ShouldBe(80);
        }
    }
}