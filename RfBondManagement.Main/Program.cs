using System.Linq;
using LiteDB;
using RfBondManagement.Engine;
using RfBondManagement.Main.Windows;
using Terminal.Gui;

namespace RfBondManagement.Main
{
    class Program
    {
        static void Main(string[] args)
        {
            Application.Init();

            var db = new LiteDatabase("bondmanagement.db");

            var settingsColl = db.GetCollection<Settings>("settings");
            var settings = settingsColl.FindAll().FirstOrDefault();
            if (null == settings)
            {
                settings = new Settings();
                settingsColl.Insert(settings);
            }

            var mainWindow = new Window("Портфель облигаций")
            {
                X = 0,
                Y = 1,

                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            var settingsWindow = new GeneralSettingsWindow(mainWindow);

            var menu = new MenuBar(new[]
            {
                new MenuBarItem("_Файл", new []
                {
                    new MenuItem("_Загрузить портфель", string.Empty, () => Application.RequestStop()),
                    new MenuItem("_Загрузить настройки", string.Empty, () => Application.RequestStop()),
                    new MenuItem("_Выход", string.Empty, () => Application.RequestStop())
                }),
                new MenuBarItem("_Настройки", new[]
                {
                    new MenuItem("_Общие настройки", string.Empty, () =>
                    {
                        settingsWindow.DataBind(settings);

                        settingsWindow.OnSave += newSettings =>
                        {
                            settings = newSettings;
                            settingsColl.DeleteAll();
                            settingsColl.Insert(settings);
                        };

                        mainWindow.Add(settingsWindow);
                    })
                })
            });

            var top = Application.Top;
            top.Add(menu);
            top.Add(mainWindow);

            Application.Run();
        }
    }
}
