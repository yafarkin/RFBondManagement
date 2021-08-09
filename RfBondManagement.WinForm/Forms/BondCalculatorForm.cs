using RfBondManagement.Engine.Common;
using RfBondManagement.Engine.Interfaces;
using RfFondPortfolio.Common.Dtos;
using RfFondPortfolio.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Unity;

namespace RfBondManagement.WinForm.Forms
{
    public partial class BondCalculatorForm : Form
    {
        public BondInPortfolio SelectedPaper;

        [Dependency]
        public IPortfolioRepository PortfolioRepository { get; set; }

        [Dependency]
        public IBondCalculator BondCalculator { get; set; }

        public BondCalculatorForm(IUnityContainer container)
        {
            InitializeComponent();

            container.BuildUp(psBond);
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

            var portfolio = PortfolioRepository.Get().FirstOrDefault() ?? new Portfolio();
            tbComission.Text = portfolio.Commissions.ToString("F3");
            tbTax.Text = portfolio.Tax.ToString("F");
            tbInflation.Text = 0.ToString("F");

            if (SelectedPaper != null)
            {
                tbCount.Text = SelectedPaper.Count.ToString();
                tbBuyPrice.Text = SelectedPaper.AveragePrice.ToString("F");
                tbAci.Text = (SelectedPaper.Aci * SelectedPaper.Count).ToString("F");
            }
            else
            {
                tbCount.Text = "1";
                tbBuyPrice.Text = 100.ToString("F");
                tbAci.Text = 0.ToString("F");
            }
        }

        private void psBond_OnSelectStockPaper(BondPaper obj)
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

            if (!decimal.TryParse(tbAci.Text, out nkd))
            {
                tbAci.Focus();
                MessageBox.Show($"Укажите корректное значение в поле '{label6.Text}'", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (dtpBuyDate.Value < DateTime.UtcNow.Date)
            {
                dtpBuyDate.Value = DateTime.UtcNow.Date;
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

            var bii = new BondIncomeInfo();
            bii.BondInPortfolio = new BondInPortfolio(obj)
            {
                Actions = new List<PortfolioPaperAction>
                {
                    new PortfolioPaperAction
                    {
                        When = dtpBuyDate.Value,
                        Count = count,
                        Value = buyPrice,
                    }
                }
            };

            if (!untilMaturity)
            {
                bii.SellPrice = sellPrice;
            }

            var portfolio = new Portfolio
            {
                Commissions = comissions,
                Tax = tax
            };

            BondCalculator.CalculateIncome(bii, portfolio, untilMaturity ? obj.MatDate : dtpSellDate.Value);

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
