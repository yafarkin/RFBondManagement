namespace RfFondPortfolio.Common.Dtos
{
    /// <summary>
    /// Параметры портфеля
    /// </summary>
    public class Portfolio : BaseEntity
    {
        /// <summary>
        /// Актуальный ли портфель
        /// </summary>
        public bool Actual { get; set; }

        /// <summary>
        /// Название портфеля
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Опциональный номер счёта
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// Комисии на каждую сделку
        /// </summary>
        public decimal Commissions { get; set; }

        /// <summary>
        /// Налог на доход
        /// </summary>
        public decimal Tax { get; set; }
    }
}