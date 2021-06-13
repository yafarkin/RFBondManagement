using System;
using RfFondPortfolio.Common.Dtos;

namespace RfBondManagement.Engine.Common
{
    public class BondIncomeInfo
    {
        public BondInPortfolio BondInPortfolio;

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