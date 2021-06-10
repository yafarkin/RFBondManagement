namespace RfBondManagement.Engine.Common
{
    public class BondBuyAction : BaseAction
    {
        public decimal Nkd { get; set; }

        public override bool IsBuy => true;
    }
}