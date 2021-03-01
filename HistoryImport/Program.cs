using System;
using NLog;
using RfBondManagement.Engine;
using RfBondManagement.Engine.Interfaces;
using Unity;

namespace HistoryImport
{
    class Program
    {
        static int Main(string[] args)
        {
            var container = ConfigureDI.Configure();
            container.RegisterInstance(container);

            var logger = container.Resolve<ILogger>();

            if (0 == args.Length)
            {
                logger.Error("Please, provide tiker list to import historical data.");
                //return -1;
            }

            var history = container.Resolve<IHistoryDatabaseLayer>();

            logger.Info("Start import historical data");

            var historyImport = container.Resolve<HistoryImport>();
            //historyImport.RunLocally();
            historyImport.RunOnline();

            history.Dispose();

            return 0;
        }
    }
}
