using RfBondManagement.Engine.Database;
using RfBondManagement.Main.Windows;
using Terminal.Gui;

namespace RfBondManagement.Main
{
    class Program
    {
        static void Main(string[] args)
        {
            Application.Init();

            var db = new DatabaseLayer();
            var settings = db.LoadSettings();

            var mainWindow = new MainWindow(db);

            var mainMenu = new MainMenu();
            mainMenu.OnExit += Application.RequestStop;
            mainMenu.OnChangeSettings += () =>
            {
                var settingsWindow = new GeneralSettingsWindow(mainWindow);
                settingsWindow.DataBind(settings);

                settingsWindow.OnSave += newSettings =>
                {
                    settings = newSettings;
                    db.SaveSettings(settings);
                };

                mainWindow.Add(settingsWindow);
            };

            var top = Application.Top;
            top.Add(mainMenu.Menu);
            top.Add(mainWindow);

            Application.Run();
        }
    }
}
