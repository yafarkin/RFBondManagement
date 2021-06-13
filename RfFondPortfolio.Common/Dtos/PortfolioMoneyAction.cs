namespace RfFondPortfolio.Common.Dtos
{
    public class PortfolioMoneyAction : PortfolioAction
    {
        /// <summary>
        /// Тип действия по деньгам.
        /// </summary>
        public MoneyActionType MoneyAction { get; set; }

        /// <summary>
        /// Сумма действия.
        /// </summary>
        public override decimal Sum { get; set; }
    }
}