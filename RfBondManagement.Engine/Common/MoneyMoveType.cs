namespace RfBondManagement.Engine.Common
{
    public enum MoneyMoveType
    {
        IncomeExternal = 1,
        IncomeDividend = 2,
        IncomeCoupon = 3,
        IncomeSellOnMarket = 4,

        OutcomeBuyOnMarket = 101,
        OutcomeCommission = 102,
        OutcomeTax = 103
    }
}