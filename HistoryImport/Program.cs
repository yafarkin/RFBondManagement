using System;
using NLog;
using RfBondManagement.Engine;
using Unity;

namespace HistoryImport
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = ConfigureDI.Configure();
            container.RegisterInstance(container);

            var logger = container.Resolve<ILogger>();
            logger.Info("Start import historical data");

            var historyImport = container.Resolve<HistoryImport>();
            historyImport.Run();

        }
    }
}
