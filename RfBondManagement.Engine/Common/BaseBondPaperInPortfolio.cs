using System.Linq;

namespace RfBondManagement.Engine.Common
{
    public class BaseBondPaperInPortfolio : BaseStockPaperInPortfolio<BaseBondPaper>
    {
        public decimal TotalBuyNKD => Actions.OfType<BondBuyAction>().Sum(a => a.NKD * a.Count);
    }
}