using RfBondManagement.Engine.Integration.Moex.Dto;

namespace RfBondManagement.Engine.Integration.Moex
{
    public class MoexBondCouponsRequest : MoexBaseRequest<JsonBondization>
    {
        protected string _ticker;

        public MoexBondCouponsRequest(string ticker)
        {
            _ticker = ticker;
        }

        protected override string _requestUrl => $"/securities/{_ticker}/bondization";
    }
}