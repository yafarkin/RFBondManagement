using System;

namespace RfBondManagement.Engine.Common
{
    public class BalanceInfo : Tuple<decimal, decimal, decimal>
    {
        public decimal Price => Item1;

        public decimal Volume => Item2;

        public decimal NeedVolume => Item3;

        public decimal Difference => NeedVolume - Volume;

        public BalanceInfo(decimal price, decimal volume, decimal needVolume)
            : base(price, volume, needVolume)
        {
        }

        public override string ToString()
        {
            return $"{Price:C}, V: {Volume}, NV: {NeedVolume}, D: {Difference}";
        }
    }
}