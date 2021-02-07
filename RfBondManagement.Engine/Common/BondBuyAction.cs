namespace RfBondManagement.Engine
{
    public class BondBuyAction : BaseAction
    {
        public decimal NKD { get; set; }

        public BaseBondPaper BondPaper
        {
            get => Paper as BaseBondPaper;
            set => Paper = value;
        }

        public override bool IsBuy => true;
    }
}