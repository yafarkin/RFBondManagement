namespace RfBondManagement.Engine.Common
{
    public class BondSellAction : BaseAction
    {
        public decimal Nkd { get; set; }

        public override bool IsBuy => false;
    }
}