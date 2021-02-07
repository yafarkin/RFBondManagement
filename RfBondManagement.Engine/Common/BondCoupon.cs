using System;

namespace RfBondManagement.Engine
{
    public class BondCoupon
    {
        public DateTime Date { get; set; }
        public decimal Value { get; set; }

        public override string ToString() => $"{Date}: {Value:C}";
    }
}