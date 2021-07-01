using System;

namespace RfFondPortfolio.Common.Dtos
{
    /// <summary>
    /// Стоимость бумаги, исторические данные
    /// </summary>
    public class HistoryPrice : BaseSecurityEntity
    {
        /// <summary>
        /// Дата и время
        /// </summary>
        public DateTime When { get; set; }

        public long NumTrades { get; set; }

        public decimal Value { get; set; }

        public decimal Volume { get; set; }

        /// <summary>
        /// Цена бумаги при открытии
        /// </summary>
        public decimal OpenPrice { get; set; }

        public decimal LowPrice { get; set; }

        public decimal HighPrice { get; set; }

        /// <summary>
        /// Цена бумаги при закрытии
        /// </summary>
        public decimal ClosePrice { get; set; }

        public decimal LegalClosePrice { get; set; }

        public string Index => $"{SecId},{When:yyyyMMdd}";
    }
}