namespace RfFondPortfolio.Common.Dtos
{
    public class BondInPortfolio : PaperInPortfolio<BondPaper>
    {
        /// <summary>
        /// НКД, на 1 бумагу
        /// </summary>
        public decimal Aci { get; set; }

        /// <summary>
        /// Стоимость одной бумаги
        /// </summary>
        public decimal Value => Paper.FaceValue * AveragePrice;

        /// <summary>
        /// Стоимость бумаги с НКД
        /// </summary>
        public decimal TotalValue => Value + Aci;

        public BondInPortfolio(BondPaper paper)
        {
            Paper = paper;
        }
    }
}