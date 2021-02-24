namespace RfBondManagement.Engine.Common
{
    public class BondBuyAction : BaseBondAction
    {
        public decimal Nkd { get; set; }

        public override bool IsBuy => true;
    }
}