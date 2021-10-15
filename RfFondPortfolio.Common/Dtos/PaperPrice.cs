using System;

namespace RfFondPortfolio.Common.Dtos
{
    /// <summary>
    /// Стоимость бумаги на текущий момент времени
    /// </summary>
    public class PaperPrice : BaseSecurityEntity
    {
        /// <summary>
        /// Дата и время
        /// </summary>
        public DateTime When { get; set; }

        /// <summary>
        /// Размер лота
        /// </summary>
        public long LotSize { get; set; }

        /// <summary>
        /// Цена бумаги, за 1 единицу
        /// </summary>
        public decimal Price { get; set; }
    }
}