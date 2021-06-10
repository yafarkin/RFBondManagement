using System;

namespace RfBondManagement.Engine.Common
{
    public class PaperOffer
    {
        public DateTime OfferDate { get; set; }

        public DateTime? OfferDateStart { get; set; }

        public DateTime? OfferDateEnd { get; set; }

        public decimal? Price { get; set; }

        public string OfferType { get; set; }
    }
}