using System;
using RfBondManagement.Engine;
using Terminal.Gui;

namespace RfBondManagement.Main.Windows
{
    public class GeneralSettingsWindow : Window
    {
        protected readonly View _parent;
        protected TextField _comissionText;
        protected TextField _taxText;

        public Settings Settings;

        public Action<Settings> OnSave { get; set; }

        public GeneralSettingsWindow(View parent)
            : base("Общие настройки", 5)
        {
            _parent = parent;
            InitControls();
            InitStyle();
        }

        public void InitStyle()
        {
            X = Pos.Center();
            Width = Dim.Percent(75);
            Height = 20;
        }

        public void DataBind(Settings settings)
        {
            Settings = settings;
            _comissionText.Text = Settings.Comissions.ToString("F");
            _taxText.Text = Settings.Tax.ToString("F");
        }

        public void Close()
        {
            _parent.Remove(this);
        }

        protected void InitControls()
        {
            var comissionLabel = new Label(0, 0, "Комиссии, %");
            _comissionText = new TextField
            {
                X = Pos.Left(comissionLabel),
                Y = Pos.Top(comissionLabel) + 1,
                Width = Dim.Fill()
            };

            Add(comissionLabel);
            Add(_comissionText);

            var taxLabel = new Label("Налог, %")
            {
                X = Pos.Left(_comissionText),
                Y = Pos.Top(_comissionText) + 1
            };

            _taxText = new TextField
            {
                X = Pos.Left(taxLabel),
                Y = Pos.Top(taxLabel) + 1,
                Width = Dim.Fill()
            };

            Add(taxLabel);
            Add(_taxText);

            var saveButton = new Button("Сохранить")
            {
                X = Pos.Left(_taxText) + 2,
                Y = Pos.Top(_taxText) + 2
            };

            var cancelButton = new Button("Отменить")
            {
                X = Pos.Right(saveButton) + 5,
                Y = Pos.Top(saveButton)
            };

            Add(saveButton);
            Add(cancelButton);

            saveButton.Clicked += () =>
            {
                decimal comissions, tax;

                if (!decimal.TryParse(_comissionText.Text.ToString(), out comissions))
                {
                    MessageBox.ErrorQuery(20, 6, "Ошибка", $"Укажите корректное значение в поле '{comissionLabel.Text}'");
                    return;
                }

                if (!decimal.TryParse(_taxText.Text.ToString(), out tax))
                {
                    MessageBox.ErrorQuery(20, 6, "Ошибка", $"Укажите корректное значение в поле '{taxLabel.Text}'");
                    return;
                }

                Settings.Comissions = comissions;
                Settings.Tax = tax;

                OnSave?.Invoke(Settings);
                Close();
            };

            cancelButton.Clicked += () =>
            {
                Close();
            };
        }
    }
}