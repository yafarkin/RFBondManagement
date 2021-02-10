using System;
using Terminal.Gui;

namespace RfBondManagement.Main.Windows
{
    public class MainMenu
    {
        public MenuBar Menu { get; protected set; }

        public Action OnExit;
        public Action OnChangeSettings;

        public MainMenu()
        {
            InitMenu();
        }

        public void InitMenu()
        {
            Menu = new MenuBar(new[]
            {
                new MenuBarItem("_Файл", new []
                {
                    new MenuItem("_Выход", string.Empty, () => OnExit?.Invoke())
                }),
                new MenuBarItem("_Настройки", new[]
                {
                    new MenuItem("_Общие настройки", string.Empty, () => OnChangeSettings?.Invoke())
                })
            });
        }
    }
}