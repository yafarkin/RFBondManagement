using System;

namespace RfFondPortfolio.Common.Dtos
{
    /// <summary>
    /// Купон по облигации
    /// </summary>
    public class BondCoupon
    {
        public decimal IssueValue { get; set; }
        public DateTime CouponDate { get; set; }
        public DateTime RecordDate { get; set; }
        public DateTime StartDate { get; set; }
        public decimal InitialFaceValue { get; set; }
        public decimal FaceValue { get; set; }
        public string FaceUnit { get; set; }
        public decimal Value { get; set; }
        public decimal ValuePercent { get; set; }
        public decimal ValueRub { get; set; }
    }
}