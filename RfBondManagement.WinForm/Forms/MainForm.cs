using NLog;
using RfFondPortfolio.Common.Dtos;
using RfFondPortfolio.Common.Interfaces;
using System;
using System.Linq;
using System.Windows.Forms;
using RfBondManagement.Engine.Interfaces;
using RfBondManagement.WinForm.Controls;
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

        public IUnityContainer DiContainer { get; set; }

        public Portfolio SelectedPortfolio
        {
            get
            {
                var selectedTab = tcData.SelectedTab;
                if (null == selectedTab || 0 == tcData.SelectedIndex)
                {
                    return null;
                }

                return selectedTab.Tag as Portfolio;
            }
        }

        public MainForm(IUnityContainer diContainer)
        {
            InitializeComponent();

            DiContainer = diContainer;
            DiContainer.BuildUp(watchList);
        }

        private void menuItemExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void menuItemGeneralSettings_Click(object sender, EventArgs e)
        {
        }

        public void DataBind()
        {
            var selectedPortfolio = SelectedPortfolio;
            for (var i = 1; i < tcData.TabPages.Count; i++)
            {
                tcData.TabPages.RemoveAt(i);
            }

            var portfolios = PortfolioRepository.Get().Where(x => x.Actual);
            foreach (var portfolio in portfolios)
            {
                var p = DiContainer.Resolve<PortfolioUC>(new ParameterOverride("portfolio", portfolio));
                p.Dock = DockStyle.Fill;

                var tp = new TabPage(portfolio.Name);
                tp.Controls.Add(p);
                tp.Tag = portfolio;

                tcData.TabPages.Add(tp);

                if (portfolio == selectedPortfolio)
                {
                    tcData.SelectedTab = tp;
                }
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            DataBind();
        }

        private void menuItemBondCalculator_Click(object sender, EventArgs e)
        {
            using (var f = DiContainer.Resolve<BondCalculatorForm>())
            {
                f.ShowDialog();
            }
        }

        private void menuItemPapers_Click(object sender, EventArgs e)
        {
            using (var f = DiContainer.Resolve<PaperListForm>())
            {
                f.ShowDialog();
                watchList.DataBind();
            }
        }

        private void menuItemAddPortfolio_Click(object sender, EventArgs e)
        {
            using (var f = DiContainer.Resolve<SettingsForm>())
            {
                if (f.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                PortfolioRepository.Insert(f.Portfolio);
                DataBind();
            }
        }

        private void menuItemEditPortfolio_Click(object sender, EventArgs e)
        {
            using (var f = DiContainer.Resolve<SettingsForm>())
            {
                f.Portfolio = SelectedPortfolio;
                if (f.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                SelectedPortfolio.Actual = f.Portfolio.Actual;
                SelectedPortfolio.Commissions = f.Portfolio.Commissions;
                SelectedPortfolio.Tax = f.Portfolio.Tax;
                SelectedPortfolio.Name = f.Portfolio.Name;
                SelectedPortfolio.AccountNumber = f.Portfolio.AccountNumber;
                SelectedPortfolio.LongTermBenefit = f.Portfolio.LongTermBenefit;
                PortfolioRepository.Update(SelectedPortfolio);
                DataBind();
            }
        }
    }
}
