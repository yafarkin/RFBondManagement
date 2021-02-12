using System;
using System.Linq;
using System.Windows.Forms;
using RfBondManagement.Engine;
using RfBondManagement.Engine.Calculations;
using RfBondManagement.Engine.Database;

namespace RfBondManagement.WinForm
{
    public partial class MainForm : Form
    {
        protected DatabaseLayer _db;
        protected Settings _settings;

        public MainForm()
        {
            InitializeComponent();
        }

        private void menuItemExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void menuItemGeneralSettings_Click(object sender, EventArgs e)
        {
            using (var f = new SettingsForm())
            {
                f.Settings = _settings;
                if (f.ShowDialog() == DialogResult.OK)
                {
                    _settings = f.Settings;
                    _db.SaveSettings(_settings);
                }
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _db = new DatabaseLayer();
            _settings = _db.LoadSettings();

            var papers = _db.GetPapersInPortfolio();

            lvPapers.Items.Clear();
            foreach (var paperInPortfolio in papers.OfType<BaseBondPaperInPortfolio>())
            {
                var bondPaper = paperInPortfolio.BondPaper;
                var calc = new BondCalculator();
                var biiToClose = new BondIncomeInfo
                {
                    PaperInPortfolio = paperInPortfolio
                };

                var biiToToday = new BondIncomeInfo
                {
                    PaperInPortfolio = paperInPortfolio,
                    SellPrice = bondPaper.LastPrice.Price
                };

                calc.CalculateIncome(biiToClose, _settings, bondPaper.MaturityDate);
                calc.CalculateIncome(biiToToday, _settings, DateTime.Today.AddDays(30));

                var lvi = new ListViewItem(new[]
                {
                    bondPaper.Name,
                    bondPaper.Currency,
                    bondPaper.BondPar.ToString("C"),
                    paperInPortfolio.Count.ToString("### ### ###"),
                    biiToClose.BalanceOnSell.ToString("C"),
                    biiToClose.ExpectedIncome.ToString("C"),
                    (biiToClose.RealIncomePercent/100).ToString("P"),
                    biiToClose.ExpectedPositiveDate.ToShortDateString(),
                    (bondPaper.MaturityDate - DateTime.Today).Days.ToString(),
                });
                lvPapers.Items.Add(lvi);
            }

            lvPapers.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }
    }
}
