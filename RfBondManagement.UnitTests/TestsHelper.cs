using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NLog;
using RfBondManagement.Engine.Calculations;
using RfBondManagement.Engine.Common;
using RfBondManagement.Engine.Interfaces;
using RfFondPortfolio.Common.Dtos;
using RfFondPortfolio.Common.Interfaces;

namespace RfBondManagement.UnitTests
{
    public static class TestsHelper
    {
        public static List<PortfolioAction> Actions = new List<PortfolioAction>();
        public static List<AbstractPaper> Papers = new List<AbstractPaper>();
        public static List<PaperSplit> Splits = new List<PaperSplit>();

        public static IExternalImport Import;

        public static decimal LastPrice;
        public static HistoryPrice LastHistoryPrice;

        public static ILogger Logger;
        public static IBondCalculator BondCalculator;
        public static IPaperRepository PaperRepository;
        public static IPortfolioActions PortfolioActions;
        public static IPortfolioMoneyActionRepository PortfolioMoneyActionRepository;
        public static IPortfolioPaperActionRepository PortfolioPaperActionRepository;
        public static ISplitRepository SplitRepository;
        public static IHistoryRepository HistoryRepository;

        public static void Reset()
        {
            Actions = new List<PortfolioAction>();
            Papers = new List<AbstractPaper>();
            Splits = new List<PaperSplit>();

            //Logger = null;
            //BondCalculator = null;
            //PaperRepository = null;
            //PortfolioActions = null;
        }

        public static IPaperRepository CreatePaperRepository()
        {
            if (null == PaperRepository)
            {
                var mock = new Mock<IPaperRepository>();
                mock.Setup(m => m.Get()).Returns(() => Papers);
                mock.Setup(m => m.Insert(It.IsAny<AbstractPaper>()))
                    .Callback<AbstractPaper>(i =>
                    {
                        i.Id = Guid.NewGuid();
                        Papers.Add(i);
                    }).Returns(() => Papers.LastOrDefault());

                PaperRepository = mock.Object;
            }

            return PaperRepository;
        }

        public static IPortfolioActions CreatePortfolioActions()
        {
            if (null == PortfolioActions)
            {
                var mock = new Mock<IPortfolioActions>();
                mock.Setup(m => m.PaperActions(It.IsAny<Guid>())).Returns(() => Actions.OfType<PortfolioPaperAction>());
                mock.Setup(m => m.MoneyActions(It.IsAny<Guid>())).Returns(() => Actions.OfType<PortfolioMoneyAction>());

                PortfolioActions = mock.Object;
            }

            return PortfolioActions;
        }

        public static IPortfolioMoneyActionRepository CreateMoneyActionRepository()
        {
            if (null == PortfolioMoneyActionRepository)
            {
                var mock = new Mock<IPortfolioMoneyActionRepository>();
                mock.Setup(m => m.Get()).Returns(() => Actions.OfType<PortfolioMoneyAction>().OrderBy(a => a.When));
                mock.Setup(m => m.Insert(It.IsAny<PortfolioMoneyAction>()))
                    .Callback<PortfolioMoneyAction>(i =>
                    {
                        i.Id = Guid.NewGuid();
                        Actions.Add(i);
                    }).Returns(() => Actions.LastOrDefault() as PortfolioMoneyAction);

                PortfolioMoneyActionRepository = mock.Object;
            }

            return PortfolioMoneyActionRepository;
        }

        public static IPortfolioPaperActionRepository CreatePaperActionRepository()
        {
            if (null == PortfolioPaperActionRepository)
            {
                var mock = new Mock<IPortfolioPaperActionRepository>();
                mock.Setup(m => m.Get()).Returns(() => Actions.OfType<PortfolioPaperAction>().OrderBy(a => a.When));
                mock.Setup(m => m.Insert(It.IsAny<PortfolioPaperAction>()))
                    .Callback<PortfolioPaperAction>(i =>
                    {
                        i.Id = Guid.NewGuid();
                        Actions.Add(i);
                    }).Returns(() => Actions.LastOrDefault() as PortfolioPaperAction);

                PortfolioPaperActionRepository = mock.Object;
            }

            return PortfolioPaperActionRepository;
        }

        public static ISplitRepository CreateSplitRepository()
        {
            if (null == SplitRepository)
            {
                var mock = new Mock<ISplitRepository>();
                mock.Setup(m => m.Get()).Returns(() => Splits);

                SplitRepository = mock.Object;
            }

            return SplitRepository;
        }

        public static IBondCalculator GetBondCalculator()
        {
            return BondCalculator ??= new BondCalculator();
        }

        public static ILogger CreateLogger()
        {
            if (null == Logger)
            {
                var mock = new Mock<ILogger>();

                Logger = mock.Object;
            }

            return Logger;
        }

        public static IHistoryRepository CreateHistoryRepository()
        {
            if (null == HistoryRepository)
            {
                var mock = new Mock<IHistoryRepository>();
                mock.Setup(m => m.GetHistoryPriceOnDate(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(() => LastHistoryPrice);
                mock.Setup(m => m.GetNearHistoryPriceOnDate(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(() => LastHistoryPrice);

                HistoryRepository = mock.Object;
            }

            return HistoryRepository;
        }

        public static IExternalImport CreateExternalImport(bool setAsDefault)
        {
            var mock = new Mock<IExternalImport>();
            mock
                .Setup(m => m.LastPrice(It.IsAny<ILogger>(), It.IsAny<AbstractPaper>()))
                .Returns(() => Task.FromResult(new PaperPrice { Price = LastPrice }));

            if (setAsDefault)
            {
                Import = mock.Object;
            }

            return mock.Object;
        }

        public static IExternalImportFactory CreateExternalImportFactory()
        {
            var mock = new Mock<IExternalImportFactory>();
            mock.Setup(m => m.GetImpl(It.IsAny<ExternalImportType>())).Returns(() => Import);
            mock.Setup(m => m.GetDefaultImpl()).Returns(() => Import);

            return mock.Object;
        }

        public static IPortfolioBuilder CreateBuilder()
        {
            var builder = new PortfolioBuilder(GetBondCalculator(), CreatePortfolioActions(), CreatePaperRepository());
            return builder;
        }

        public static IPortfolioCalculator CreateCalculator(Portfolio portfolio)
        {
            var calc = new PortfolioCalculator(CreateBuilder(), CreatePortfolioActions(), CreatePaperRepository(), CreateSplitRepository(), GetBondCalculator(), CreateLogger());
            calc.Configure(portfolio);
            return calc;
        }

        public static IPortfolioLogic CreateLogic(Portfolio portfolio)
        {
            var logic = new PortfolioLogic(CreateLogger(), CreateExternalImportFactory(), CreateMoneyActionRepository(), CreatePaperActionRepository(), CreateHistoryRepository());
            logic.Configure(portfolio, ExternalImportType.Moex);
            return logic;
        }
    }
}