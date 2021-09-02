using RfFondPortfolio.Common.Dtos;
using System;
using System.Windows.Forms;
using Unity;

namespace RfBondManagement.WinForm.Controls
{
    public partial class PortfolioUC : UserControl
    {
        public readonly Portfolio Portfolio;

        public PortfolioUC(IUnityContainer container, Portfolio portfolio)
        {
            InitializeComponent();

            Portfolio = portfolio;

            container.BuildUp(portfolioTree);
            container.BuildUp(portfolioActions);

            portfolioTree.Portfolio = Portfolio;
            portfolioActions.Portfolio = Portfolio;
        }

        private void PortfolioUC_Load(object sender, EventArgs e)
        {
            lblPortfolio.Text = $"{Portfolio.Name} ({Portfolio.AccountNumber})";
        }
    }
}
