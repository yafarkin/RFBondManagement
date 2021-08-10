using System;
using System.Windows.Forms;
using RfFondPortfolio.Common.Dtos;

namespace RfBondManagement.WinForm.Forms
{
    public partial class SettingsForm : Form
    {
        public Portfolio Portfolio { get; set; }

        public SettingsForm()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            decimal comissions, tax;

            DialogResult = DialogResult.None;

            if (string.IsNullOrWhiteSpace(tbName.Text))
            {
                tbName.Focus();
                MessageBox.Show($"Заполните название портфеля", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

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

            Portfolio.Actual = cbActual.Checked;
            Portfolio.Name = tbName.Text.Trim();
            Portfolio.AccountNumber = tbAccount.Text.Trim();
            Portfolio.LongTermBenefit = cbLTB.Checked;
            Portfolio.Commissions = comissions;
            Portfolio.Tax = tax;

            DialogResult = DialogResult.OK;
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            Portfolio ??= new Portfolio
            {
                Actual = true,
                LongTermBenefit = true
            };

            cbActual.Checked = Portfolio.Actual;
            tbName.Text = Portfolio.Name;
            tbAccount.Text = Portfolio.AccountNumber;
            cbLTB.Checked = Portfolio.LongTermBenefit;
            tbComission.Text = Portfolio.Commissions.ToString("F3");
            tbTax.Text = Portfolio.Tax.ToString("F");

            tbName.Focus();
        }
    }
}
