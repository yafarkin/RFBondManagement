using System;
using System.Windows.Forms;
using RfBondManagement.Engine.Common;

namespace RfBondManagement.WinForm.Forms
{
    public partial class SettingsForm : Form
    {
        public Settings Settings { get; set; }

        public SettingsForm()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            decimal comissions, tax;

            if (!decimal.TryParse(tbComission.Text, out comissions))
            {
                tbComission.Focus();
                MessageBox.Show($"Укажите корректное значение в поле '{lblComission.Text}'", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!decimal.TryParse(tbTax.Text, out tax))
            {
                tbTax.Focus();
                MessageBox.Show($"Укажите корректное значение в поле '{lblTax.Text}'", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Settings.Comissions = comissions;
            Settings.Tax = tax;

            DialogResult = DialogResult.OK;
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            tbComission.Text = Settings.Comissions.ToString("F3");
            tbTax.Text = Settings.Tax.ToString("F");
        }
    }
}
