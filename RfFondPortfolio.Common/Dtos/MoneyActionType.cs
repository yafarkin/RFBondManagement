namespace RfFondPortfolio.Common.Dtos
{
    /// <summary>
    /// Тип действия с деньгами
    /// </summary>
    public enum MoneyActionType
    {
        /// <summary>
        /// Пополнение счета
        /// </summary>
        IncomeExternal = 1,

        /// <summary>
        /// Пришедшие дивиденды
        /// </summary>
        IncomeDividend = 2,

        /// <summary>
        /// Пришедшие купоны
        /// </summary>
        IncomeCoupon = 3,

        /// <summary>
        /// Продажа бумаги
        /// </summary>
        IncomeSellOnMarket = 4,

        /// <summary>
        /// Погашение облигации
        /// </summary>
        IncomeCloseBond = 5,

        /// <summary>
        /// Вывод денег со счёта
        /// </summary>
        OutcomeExternal = 100,

        /// <summary>
        /// Покупка бумаги
        /// </summary>
        OutcomeBuyOnMarket = 101,

        /// <summary>
        /// Комиссия
        /// </summary>
        OutcomeCommission = 102,

        /// <summary>
        /// Налог, рассчитываемый автоматически
        /// </summary>
        OutcomeTax = 103,

        /// <summary>
        /// Отложенный налог (списывается при выводе денег)
        /// </summary>
        OutcomeDelayTax = 104,

        /// <summary>
        /// Списание отложенного налога
        /// </summary>
        OutcomeDelayTaxPay = 105,
    }

    public static class MoneyActionTypeHelper
    {
        public static readonly MoneyActionType[] IncomeTypes = new[] {
            MoneyActionType.IncomeExternal,
            MoneyActionType.IncomeDividend,
            MoneyActionType.IncomeCoupon,
            MoneyActionType.IncomeSellOnMarket,
            MoneyActionType.IncomeCloseBond
        };

        public static readonly MoneyActionType[] OutcomeTypes = new[] {
            MoneyActionType.OutcomeExternal,
            MoneyActionType.OutcomeBuyOnMarket,
            MoneyActionType.OutcomeCommission,
            MoneyActionType.OutcomeTax,
            MoneyActionType.OutcomeDelayTaxPay
        };
    }
}