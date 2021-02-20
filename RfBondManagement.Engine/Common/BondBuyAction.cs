namespace RfBondManagement.Engine.Common
{
    public class BondBuyAction : BaseBondAction
    {
        public decimal NKD { get; set; }

        public override bool IsBuy => true;
    }
}