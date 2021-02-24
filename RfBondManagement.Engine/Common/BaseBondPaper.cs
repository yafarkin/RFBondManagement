using System;
using System.Collections.Generic;

namespace RfBondManagement.Engine.Common
{
    public class BaseBondPaper : BaseStockPaper
    {
        public DateTime PublishDate { get; set; }
        public DateTime? OfferDate { get; set; }
        public DateTime MaturityDate { get; set; }

        public int Duration { get; set; }

        public decimal BondPar { get; set; }
        public string Currency { get; set; }

        public string Rating { get; set; }

        public List<BondCoupon> Coupons { get; set; }
    }
}