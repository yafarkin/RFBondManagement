using System;
using System.Linq;
using System.Windows.Forms;
using NLog;
using RfBondManagement.Engine.Calculations;
using RfBondManagement.Engine.Common;
using RfBondManagement.Engine.Interfaces;
using RfFondPortfolio.Common.Dtos;
using RfFondPortfolio.Common.Interfaces;
using Unity;

namespace RfBondManagement.WinForm.Forms
{
    public partial class MainForm : Form
    {
        protected ILogger _logger;
        protected IPortfolioRepository _portfolioRepository;
        protected IUnityContainer _container;

        protected Portfolio _portfolio;

        public MainForm(ILogger logger, IPortfolioRepository portfolioRepository, IUnityContainer container)
        {
            _logger = logger;
            _container = container;
            _portfolioRepository = portfolioRepository;

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
                f.Portfolio = _portfolio;
                if (f.ShowDialog() == DialogResult.OK)
                {
                    _portfolio = f.Portfolio;
                    _portfolioRepository.Update(_portfolio);
                }
            }
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            _portfolio = _portfolioRepository.Get().FirstOrDefault() ?? new Portfolio();

            var engine = _container.Resolve<PortfolioEngine>();
            var content = engine.Build();
            await engine.FillPrice(content);

            var papers = content.Papers;

            lvPapers.Items.Clear();
            foreach (var paperInPortfolio in papers)
            {
                var paper = paperInPortfolio.Paper;
                if (paper.PaperType == PaperType.Bond)
                {
                    var bondPaper = paper as BondPaper;
                    var bondInPortfolio = paperInPortfolio as BondInPortfolio;

                    var calc = _container.Resolve<IBondCalculator>();
                    var biiToClose = new BondIncomeInfo
                    {
                        BondInPortfolio = bondInPortfolio
                    };

                    var biiToToday = new BondIncomeInfo
                    {
                        BondInPortfolio = bondInPortfolio,
                        SellPrice = paperInPortfolio.MarketPrice
                    };

                    calc.CalculateIncome(biiToClose, _portfolio, bondPaper.MatDate);
                    calc.CalculateIncome(biiToToday, _portfolio, DateTime.Today.AddDays(30));

                    var lvi = new ListViewItem(new[]
                    {
                        paper.Name,
                        paper.FaceValue.ToString("C"),
                        paperInPortfolio.Count.ToString("### ### ###"),
                        biiToClose.BalanceOnSell.ToString("C"),
                        biiToClose.ExpectedIncome.ToString("C"),
                        (biiToClose.RealIncomePercent / 100).ToString("P"),
                        biiToClose.BreakevenDate.ToShortDateString(),
                        (bondPaper.MatDate - DateTime.Today).Days.ToString(),
                    });
                    lvPapers.Items.Add(lvi);
                }
            }

            lvPapers.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void menuItemBondCalculator_Click(object sender, EventArgs e)
        {
            using (var f = _container.Resolve<BondCalculatorForm>())
            {
                f.ShowDialog();
            }
        }

        private void menuItemPapers_Click(object sender, EventArgs e)
        {
            using (var f = _container.Resolve<PaperListForm>())
            {
                f.ShowDialog();
            }
        }

        private void btnAddPaper_Click(object sender, EventArgs e)
        {
            using (var f = _container.Resolve<PaperActionForm>())
            {
                f.ShowDialog();
            }
        }
    }
}
