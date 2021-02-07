namespace RfBondManagement.Engine
{
    public class BondIncomeInfo
    {
        public BondBuyAction BuyAction;

        public bool CloseByMaturityDate;
        public decimal SellPrice;
        public decimal SellTax;

        public decimal BalanceOnBuy;
        public decimal IncomeByCoupons;
        public decimal BalanceOnSell => SellPrice + IncomeByCoupons;
        public decimal ExpectedIncome => BalanceOnSell - BalanceOnBuy;
        public decimal RealIncomePercent;
    }
}