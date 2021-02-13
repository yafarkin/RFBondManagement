using System;
using System.Linq;
using System.Windows.Forms;
using NLog;
using RfBondManagement.Engine;
using RfBondManagement.Engine.Calculations;
using RfBondManagement.Engine.Common;
using RfBondManagement.Engine.Interfaces;
using RfBondManagement.WinForm.Forms;
using Unity;

namespace RfBondManagement.WinForm
{
    public partial class MainForm : Form
    {
        protected ILogger _logger;
        protected IDatabaseLayer _db;
        protected IUnityContainer _container;

        protected Settings _settings;

        public MainForm(ILogger logger, IDatabaseLayer db, IUnityContainer container)
        {
            _logger = logger;
            _container = container;
            _db = db;
            InitializeComponent();
        }

        private void menuItemExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void menuItemGeneralSettings_Click(object sender, EventArgs e)
        {
            using (var f = _container.Resolve<SettingsForm>())
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
            _settings = _db.LoadSettings();

            var papers = _db.GetPapersInPortfolio();

            lvPapers.Items.Clear();
            foreach (var paperInPortfolio in papers.OfType<BaseBondPaperInPortfolio>())
            {
                var bondPaper = paperInPortfolio.BondPaper;
                var calc = _container.Resolve<IBondCalculator>();
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
