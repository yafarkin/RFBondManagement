namespace RfFondPortfolio.Common.Dtos
{
    /// <summary>
    /// Тип бумаги
    /// </summary>
    public enum PaperType
    {
        /// <summary>
        /// Неизвестно
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Акция
        /// </summary>
        Share = 1,

        /// <summary>
        /// ETF
        /// </summary>
        Etf = 3,

        /// <summary>
        /// БПИФ
        /// </summary>
        Ppif = 4,

        /// <summary>
        /// Облигация
        /// </summary>
        Bond = 5,

        /// <summary>
        /// Депозитарная расписка
        /// </summary>
        DR = 6
    }
}