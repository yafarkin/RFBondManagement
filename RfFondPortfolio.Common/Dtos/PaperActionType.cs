namespace RfFondPortfolio.Common.Dtos
{
    /// <summary>
    /// Тип действия с бумагой
    /// </summary>
    public enum PaperActionType
    {
        /// <summary>
        /// Продажа
        /// </summary>
        Sell,

        /// <summary>
        /// Покупка
        /// </summary>
        Buy,

        /// <summary>
        /// Закрытие позиции
        /// </summary>
        /// <remarks>Например, погашение облигации</remarks>
        Close,
    }
}