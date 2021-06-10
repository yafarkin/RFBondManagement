using System;

namespace RfFondPortfolio.Common.Dtos
{
    /// <summary>
    /// Стоимость бумаги на определенный момент времени
    /// </summary>
    public class PaperPrice
    {
        /// <summary>
        /// Уникальный норме
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Дата и время
        /// </summary>
        public DateTime When { get; set; }

        /// <summary>
        /// Код бумаги
        /// </summary>
        public string SecId { get; set; }

        /// <summary>
        /// Цена бумаги
        /// </summary>
        public decimal Value { get; set; }
    }
}