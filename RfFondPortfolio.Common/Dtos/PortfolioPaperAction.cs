namespace RfFondPortfolio.Common.Dtos
{
    public class PortfolioPaperAction : PortfolioAction
    {
        /// <summary>
        /// Тип действия по бумаге
        /// </summary>
        public PaperActionType PaperAction { get; set; }

        /// <summary>
        /// Количество бумаг
        /// </summary>
        public long Count { get; set; }

        /// <summary>
        /// Сумма за единицу
        /// </summary>
        public decimal Value { get; set; }

        public override decimal Sum
        {
            get => Count * Value;
            set { }
        }
    }
}