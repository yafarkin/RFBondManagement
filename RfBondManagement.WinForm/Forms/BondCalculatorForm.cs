using System;
using System.Collections.Generic;
using System.Windows.Forms;
using RfBondManagement.Engine.Common;
using RfBondManagement.Engine.Interfaces;

namespace RfBondManagement.WinForm.Forms
{
    public partial class BondCalculatorForm : Form
    {
        public BaseBondPaperInPortfolio SelectedPaper;

        protected readonly IDatabaseLayer _db;
        protected readonly IBondCalculator _calculator;

        public BondCalculatorForm(IDatabaseLayer db, IBondCalculator calculator)
        {
            _db = db;
            _calculator = calculator;

            InitializeComponent();

            psBond.InitDI(_db);
        }

        private void cbUntilMaturityDate_CheckedChanged(object sender, EventArgs e)
        {
            lblSell.Enabled = !cbUntilMaturityDate.Checked;
            tbSellPrice.Enabled = lblSell.Enabled;
            lblSellWhen.Enabled = lblSell.Enabled;
            dtpSellDate.Enabled = lblSell.Enabled;
        }

        private void BondCalculatorForm_Load(object sender, EventArgs e)
        {
            cbUntilMaturityDate.Checked = true;

            var settings = _db.LoadSettings();
            tbComission.Text = settings.Comissions.ToString("F3");
            tbTax.Text = settings.Tax.ToString("F");
            tbInflation.Text = 0.ToString("F");

            if (SelectedPaper != null)
            {
                tbCount.Text = SelectedPaper.Count.ToString();
                tbBuyPrice.Text = SelectedPaper.AvgBuySum.ToString("F");
                tbNKD.Text = SelectedPaper.TotalBuyNKD.ToString("F");
            }
            else
            {
                tbCount.Text = "1";
                tbBuyPrice.Text = 100.ToString("F");
                tbNKD.Text = 0.ToString("F");
            }
        }

        private void psBond_OnSelectStockPaper(BaseStockPaper obj)
        {
            decimal comissions, tax, inflation, buyPrice, nkd, sellPrice = 0;
            long count;

            var untilMaturity = cbUntilMaturityDate.Checked;

            if (null == obj)
            {
                return;
            }

            if (!decimal.TryParse(tbComission.Text, out comissions))
            {
                tbComission.Focus();
                MessageBox.Show($"Укажите корректное значение в поле '{label1.Text}'", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!decimal.TryParse(tbTax.Text, out tax))
            {
                tbTax.Focus();
                MessageBox.Show($"Укажите корректное значение в поле '{label2.Text}'", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!decimal.TryParse(tbInflation.Text, out inflation))
            {
                tbInflation.Focus();
                MessageBox.Show($"Укажите корректное значение в поле '{label3.Text}'", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!long.TryParse(tbCount.Text, out count))
            {
                tbCount.Focus();
                MessageBox.Show($"Укажите корректное значение в поле '{label4.Text}'", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (count < 1)
            {
                count = 1;
                tbCount.Text = count.ToString();
            }

            if (!decimal.TryParse(tbBuyPrice.Text, out buyPrice))
            {
                tbBuyPrice.Focus();
                MessageBox.Show($"Укажите корректное значение в поле '{label5.Text}'", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!decimal.TryParse(tbNKD.Text, out nkd))
            {
                tbNKD.Focus();
                MessageBox.Show($"Укажите корректное значение в поле '{label6.Text}'", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (dtpBuyDate.Value < DateTime.Today)
            {
                dtpBuyDate.Value = DateTime.Today;
            }

            if (!untilMaturity)
            {
                if (!decimal.TryParse(tbSellPrice.Text, out sellPrice))
                {
                    tbSellPrice.Focus();
                    MessageBox.Show($"Укажите корректное значение в поле '{lblSell.Text}'", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (dtpSellDate.Value < dtpBuyDate.Value)
                {
                    dtpSellDate.Value = dtpBuyDate.Value.AddDays(1);
                }
            }

            var bondPaper = obj as BaseBondPaper;

            var bii = new BondIncomeInfo();
            bii.PaperInPortfolio = new BaseBondPaperInPortfolio
            {
                Paper = bondPaper,
                Actions = new List<BaseAction<BaseBondPaper>>
                {
                    new BondBuyAction
                    {
                        Paper = bondPaper,
                        Count = count,
                        Price = buyPrice,
                        Date = dtpBuyDate.Value
                    }
                }
            };

            if (!untilMaturity)
            {
                bii.SellPrice = sellPrice;
            }

            var settings = new Settings
            {
                Comissions = comissions,
                Tax = tax
            };

            _calculator.CalculateIncome(bii, settings, untilMaturity ? bondPaper.MaturityDate : dtpSellDate.Value);

            lblRealIncomePercent.Text = (bii.RealIncomePercent/100m).ToString("P");
            lblExpectedIncome.Text = bii.ExpectedIncome.ToString("C");
            lblBreakevenDate.Text = bii.BreakevenDate.ToShortDateString();
            lblStartBalance.Text = bii.BalanceOnBuy.ToString("C");
            lblPaymentByCoupons.Text = bii.IncomeByCoupons.ToString("C");
            lblTaxOnSell.Text = bii.SellTax.ToString("C");
            lblTotalBalance.Text = bii.BalanceOnSell.ToString("C");
        }
    }
}
