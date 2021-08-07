using RfFondPortfolio.Common.Dtos;
using System;
using System.Windows.Forms;
using Unity;

namespace RfBondManagement.WinForm.Controls
{
    public partial class PortfolioUC : UserControl
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        public Portfolio Portfolio { get; set; }

        public PortfolioUC()
        {
            InitializeComponent();
        }

        private void PortfolioUC_Load(object sender, EventArgs e)
        {
        }
    }
}
