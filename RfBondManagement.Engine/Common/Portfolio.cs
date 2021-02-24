using System.Collections.Generic;

namespace RfBondManagement.Engine.Common
{
    public class Portfolio
    {
        public Settings Settings { get; set; }

        public List<BaseSharePaperInPortfolio> Shares { get; set; }

        public List<BaseBondPaperInPortfolio> Bonds { get; set; }

        public List<BaseMoneyMove> MoneyMoves { get; set; }

        public decimal Sum { get; set; }
    }
}