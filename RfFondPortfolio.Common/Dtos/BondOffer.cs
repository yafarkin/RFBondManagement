using System;

namespace RfFondPortfolio.Common.Dtos
{
    /// <summary>
    /// Оферты по облигации
    /// </summary>
    public class BondOffer
    {
        public decimal IssueValue { get; set; }
        public DateTime OfferDate { get; set; }
        public DateTime? OfferDateStart { get; set; }
        public DateTime? OfferDateEnd { get; set; }
        public decimal FaceValue { get; set; }
        public string FaceUnit { get; set; }
        public decimal? Price { get; set; }
        public decimal? Value { get; set; }
        public string OfferType { get; set; }
    }
}