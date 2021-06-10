using System;

namespace RfFondPortfolio.Common.Dtos
{
    /// <summary>
    /// Действие внутри портфеля
    /// </summary>
    public abstract class PortfolioAction
    {
        /// <summary>
        /// Уникальный номер действия
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Идентификатор портфеля
        /// </summary>
        public Guid PortfolioId { get; set; }

        /// <summary>
        /// Дата и время действия
        /// </summary>
        public DateTime When { get; set; }

        /// <summary>
        /// Опциональная ссылка на исходное действие
        /// </summary>
        public Guid? RefPortfolioActionId { get; set; }

        /// <summary>
        /// Сумма общая
        /// </summary>
        public decimal Sum { get; set; }

        /// <summary>
        /// Тикер бумаги
        /// </summary>
        public string SecId { get; set; }
    }
}