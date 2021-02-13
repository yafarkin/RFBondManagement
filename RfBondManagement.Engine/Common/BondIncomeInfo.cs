using System;

namespace RfBondManagement.Engine.Common
{
    public class BondIncomeInfo
    {
        public BaseBondPaperInPortfolio PaperInPortfolio;


        public bool CloseByMaturityDate;
        public decimal SellPrice;
        public decimal SellTax;

        public decimal BalanceOnBuy;
        public decimal IncomeByCoupons;
        public decimal BalanceOnSell => SellPrice + IncomeByCoupons;
        public decimal ExpectedIncome => BalanceOnSell - BalanceOnBuy;
        public decimal RealIncomePercent;
        public DateTime BreakevenDate;
    }
}