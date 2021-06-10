using System;

namespace RfFondPortfolio.Common.Dtos
{
    /// <summary>
    /// Курс обмена валюты
    /// </summary>
    public class ExchangeCurrency
    {
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Валюта
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Дата курса
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Значение курса
        /// </summary>
        public decimal Value { get; set; }
    }
}