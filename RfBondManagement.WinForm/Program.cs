using System;
using System.Windows.Forms;
using NLog;
using RfBondManagement.Engine;
using Unity;

namespace RfBondManagement.WinForm
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var container = ConfigureDI.Configure();
            var logger = container.Resolve<ILogger>();

            logger.Info("Start application");

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(container.Resolve<MainForm>());

            logger.Info("End application");
        }
    }
}
