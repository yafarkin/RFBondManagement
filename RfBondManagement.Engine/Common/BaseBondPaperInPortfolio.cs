using System.Linq;

namespace RfBondManagement.Engine.Common
{
    public class BaseBondPaperInPortfolio : BaseStockPaperInPortfolio<BaseStockPaper>
    {
        public decimal TotalBuyNKD => Actions.OfType<BondBuyAction>().Sum(a => a.Nkd * a.Count);
    }
}