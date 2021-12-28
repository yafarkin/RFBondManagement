using System;

namespace RfFondPortfolio.Common.Dtos
{
    /// <summary>
    /// Действие внутри портфеля
    /// </summary>
    public abstract class PortfolioAction : BaseSecurityEntity
    {
        /// <summary>
        /// Идентификатор портфеля
        /// </summary>
        public Guid PortfolioId { get; set; }

        /// <summary>
        /// Дата и время действия
        /// </summary>
        public DateTime When { get; set; }

        /// <summary>
        /// Сумма общая
        /// </summary>
        public abstract decimal Sum { get; set; }

        /// <summary>
        /// Текстовое примечание
        /// </summary>
        public string Comment { get; set; }

        public override string ToString()
        {
            return $"{When}: {Sum:C}, {Comment}";
        }
    }
}