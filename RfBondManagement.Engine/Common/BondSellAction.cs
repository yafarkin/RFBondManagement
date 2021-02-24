namespace RfBondManagement.Engine.Common
{
    public class BondSellAction : BaseBondAction
    {
        public decimal Nkd { get; set; }

        public override bool IsBuy => false;
    }
}