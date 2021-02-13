using System;

namespace RfBondManagement.Engine.Common
{
    public class BondCoupon
    {
        public DateTime Date { get; set; }
        public decimal Value { get; set; }

        public override string ToString() => $"{Date}: {Value:C}";
    }
}