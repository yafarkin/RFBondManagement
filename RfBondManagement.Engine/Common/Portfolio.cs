using System.Collections.Generic;

namespace RfBondManagement.Engine.Common
{
    public class Portfolio
    {
        public Settings Settings { get; set; } = new Settings();

        public List<BaseSharePaperInPortfolio> Shares { get; set; } = new List<BaseSharePaperInPortfolio>();

        public List<BaseBondPaperInPortfolio> Bonds { get; set; } = new List<BaseBondPaperInPortfolio>();

        public List<BaseMoneyMove> MoneyMoves { get; set; } = new List<BaseMoneyMove>();

        public decimal Sum { get; set; }
    }
}