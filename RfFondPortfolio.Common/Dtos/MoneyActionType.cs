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
        /// НКД при продаже
        /// </summary>
        IncomeAci = 4,

        /// <summary>
        /// Продажа бумаги
        /// </summary>
        IncomeSellOnMarket = 5,

        /// <summary>
        /// Погашение облигации
        /// </summary>
        IncomeCloseBond = 6,

        /// <summary>
        /// Вывод денег со счёта
        /// </summary>
        OutcomeExternal = 100,

        /// <summary>
        /// Покупка бумаги
        /// </summary>
        OutcomeBuyOnMarket = 101,

        /// <summary>
        /// НКД при покупке
        /// </summary>
        OutcomeAci = 102,

        /// <summary>
        /// Комиссия
        /// </summary>
        OutcomeCommission = 103,

        /// <summary>
        /// Налог, рассчитываемый автоматически
        /// </summary>
        OutcomeTax = 104,

        /// <summary>
        /// "Грязный" (до уплаты налогов) результат продажи бумаги
        /// </summary>
        DraftProfit = 105
    }

    public static class MoneyActionTypeHelper
    {
        public static readonly MoneyActionType[] IncomeTypes = new[] {
            MoneyActionType.IncomeExternal,
            MoneyActionType.IncomeDividend,
            MoneyActionType.IncomeCoupon,
            MoneyActionType.IncomeAci,
            MoneyActionType.IncomeSellOnMarket,
            MoneyActionType.IncomeCloseBond
        };

        public static readonly MoneyActionType[] OutcomeTypes = new[] {
            MoneyActionType.OutcomeExternal,
            MoneyActionType.OutcomeBuyOnMarket,
            MoneyActionType.OutcomeAci,
            MoneyActionType.OutcomeCommission,
            MoneyActionType.OutcomeTax
        };
    }
}