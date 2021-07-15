using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RfFondPortfolio.Common.Dtos;
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

            plBond.Portfolio = Portfolio;
            plBond.PaperTypes = new List<PaperType> {PaperType.Bond};
            plFond.Portfolio = Portfolio;
            plFond.PaperTypes = new List<PaperType> {PaperType.Etf, PaperType.Ppif};
            plShare.Portfolio = Portfolio;
            plShare.PaperTypes = new List<PaperType> {PaperType.Share, PaperType.DR, PaperType.Unknown};
        }

        private void PortfolioUC_Load(object sender, EventArgs e)
        {
        }
    }
}
