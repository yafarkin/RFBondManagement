using System.Linq;

namespace RfBondManagement.Engine.Common
{
    public class BaseBondPaperInPortfolio : BaseStockPaperInPortfolio
    {
        public BaseBondPaper BondPaper
        {
            get => Paper as BaseBondPaper;
            set => Paper = value;
        }

        public decimal TotalBuyNKD => Actions.OfType<BondBuyAction>().Sum(a => a.NKD * a.Count);
    }
}