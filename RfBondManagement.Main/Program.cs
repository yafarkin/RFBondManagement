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
                new MenuBarItem("_File", new MenuItem[]
                {
                    new MenuItem("_Quit", "", () => Application.RequestStop())
                }), // end of file menu
                new MenuBarItem("_Help", new MenuItem[]
                {
                    new MenuItem("_About", "", ()
                        => MessageBox.Query(10, 5, "About", "Written by Ali Bahraminezhad\nVersion: 0.0001", "Ok"))
                }) // end of the help menu
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
