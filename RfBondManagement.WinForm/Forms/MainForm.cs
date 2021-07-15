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
using Unity.Resolution;

namespace RfBondManagement.WinForm.Forms
{
    public partial class MainForm : Form
    {
        [Dependency]
        public ILogger Logger { get; set; }

        [Dependency]
        public IPortfolioRepository PortfolioRepository { get; set; }

        [Dependency]
        public IUnityContainer Container { get; set; }

        protected Portfolio _portfolio;

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
            using (var f = Container.Resolve<SettingsForm>())
            {
                f.Portfolio = _portfolio;
                if (f.ShowDialog() == DialogResult.OK)
                {
                    _portfolio = f.Portfolio;
                    PortfolioRepository.Update(_portfolio);
                }
            }
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            _portfolio = PortfolioRepository.Get().FirstOrDefault() ?? new Portfolio {Id = Guid.NewGuid()};

            var engine = Container.Resolve<PortfolioEngine>(new ParameterOverride("portfolio", _portfolio));
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

                    var calc = Container.Resolve<IBondCalculator>();
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
                    calc.CalculateIncome(biiToToday, _portfolio, DateTime.UtcNow.Date.AddDays(30));

                    var lvi = new ListViewItem(new[]
                    {
                        paper.Name,
                        paper.FaceValue.ToString("C"),
                        paperInPortfolio.Count.ToString("### ### ###"),
                        biiToClose.BalanceOnSell.ToString("C"),
                        biiToClose.ExpectedIncome.ToString("C"),
                        (biiToClose.RealIncomePercent / 100).ToString("P"),
                        biiToClose.BreakevenDate.ToShortDateString(),
                        (bondPaper.MatDate - DateTime.UtcNow.Date).Days.ToString(),
                    });
                    lvPapers.Items.Add(lvi);
                }
            }

            lvPapers.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void menuItemBondCalculator_Click(object sender, EventArgs e)
        {
            using (var f = Container.Resolve<BondCalculatorForm>())
            {
                f.ShowDialog();
            }
        }

        private void menuItemPapers_Click(object sender, EventArgs e)
        {
            using (var f = Container.Resolve<PaperListForm>())
            {
                f.ShowDialog();
            }
        }

        private void btnAddPaper_Click(object sender, EventArgs e)
        {
            using (var f = Container.Resolve<PaperActionForm>())
            {
                f.ShowDialog();
            }
        }
    }
}
