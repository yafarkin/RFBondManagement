using RfBondManagement.Engine.Calculations;
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
            container.RegisterType<IDatabaseLayer, DatabaseLayer>(TypeLifetime.Singleton);
            container.RegisterType<IHistoryDatabaseLayer, HistoryDatabaseLayer>(TypeLifetime.Singleton);
            container.RegisterType<IBondCalculator, BondCalculator>();
            container.RegisterType<IExternalImport, MoexImport>();

            container.AddNewExtension<NLogExtension>();

            return container;
        }
    }
}