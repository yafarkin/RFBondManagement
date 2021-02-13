using RfBondManagement.Engine.Calculations;
using RfBondManagement.Engine.Database;
using RfBondManagement.Engine.Interfaces;
using Unity;

namespace RfBondManagement.Engine
{
    public static class ConfigureDI
    {
        public static IUnityContainer Configure(IUnityContainer container = null)
        {
            container ??= new UnityContainer();
            container.RegisterType<IDatabaseLayer, DatabaseLayer>(TypeLifetime.Singleton);
            container.RegisterType<IBondCalculator, BondCalculator>();

            container.AddNewExtension<NLogExtension>();

            return container;
        }
    }
}