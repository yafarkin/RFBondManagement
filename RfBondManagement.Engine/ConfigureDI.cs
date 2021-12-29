using RfBondManagement.Engine.Calculations;
using RfBondManagement.Engine.Common;
using RfBondManagement.Engine.Database;
using RfBondManagement.Engine.Interfaces;
using RfFondPortfolio.Common.Interfaces;
using RfFondPortfolio.Integration.Moex;
using Unity;

namespace RfBondManagement.Engine
{
    public static class ConfigureDI
    {
        public static IUnityContainer Configure(IUnityContainer container = null)
        {
            container ??= new UnityContainer();

            container
                .RegisterType<IDatabaseLayer, DatabaseLayer>(TypeLifetime.Singleton)
                .RegisterType<IBondCalculator, BondCalculator>()
                .RegisterType<IPaperRepository, PaperRepository>()
                .RegisterType<IPortfolioRepository, PortfolioRepository>()
                .RegisterType<IPortfolioMoneyActionRepository, PortfolioMoneyActionRepository>()
                .RegisterType<IPortfolioPaperActionRepository, PortfolioPaperActionRepository>()
                .RegisterType<IHistoryRepository, HistoryRepository>()
                .RegisterType<ISplitRepository, SplitRepository>()
                .RegisterType<IExternalImportFactory, ExternalImportFactory>(TypeLifetime.Singleton)
                .RegisterType<IExternalImport, MoexImport>(ExternalImportType.Moex.ToString())
                .RegisterType<IPortfolioActions, PortfolioActions>()
                .RegisterType<IPortfolioCalculator, PortfolioCalculator>()
                .RegisterType<IPortfolioBuilder, PortfolioBuilder>()
                .RegisterType<IPortfolioService, PortfolioService>()
                .RegisterType<IAdviser, BuyAdviser>(Constants.Adviser.BuyAndHold)
                ;

            container.AddNewExtension<NLogExtension>();

            return container;
        }
    }
}