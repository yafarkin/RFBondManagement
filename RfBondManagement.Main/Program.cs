using System;
using Terminal.Gui;

namespace RfBondManagement.Main
{
    class Program
    {
        static void Main(string[] args)
        {
            Application.Init();

            var top = Application.Top;

            var menu = new MenuBar(new[]
            {
                new MenuBarItem("_Файл", new []
                {
                    new MenuItem("_Загрузить портфель", "", () => Application.RequestStop()),
                    new MenuItem("_Загрузить настройки", "", () => Application.RequestStop()),
                    new MenuItem("_Выход", string.Empty, () => Application.RequestStop())
                }), // end of file menu
            });
            top.Add(menu);

            var mainWindow = new Window("Retro Chat")
            {
                X = 0,
                Y = 1, // Leave one row for the toplevel menu

                // By using Dim.Fill(), it will automatically resize without manual intervention
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            var loginWindow = new LoginWindow(mainWindow) {OnExit = () => Application.RequestStop()};
            mainWindow.Add(loginWindow);
            mainWindow.Add(menu);
            Application.Run(mainWindow);
        }
    }
}
