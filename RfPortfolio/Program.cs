using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.ReactiveUI;
using System;
using NLog;
using RfBondManagement.Engine;
using Unity;

namespace RfPortfolio
{
    class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            var container = ConfigureDI.Configure();
            container.RegisterInstance(container);

            var logger = container.Resolve<ILogger>();

            logger.Info("Start application");

            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);

            logger.Info("End application");
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
        {
            return AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI();
        }
    }
}
