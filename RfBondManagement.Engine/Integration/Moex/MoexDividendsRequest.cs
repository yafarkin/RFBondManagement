using RfBondManagement.Engine.Integration.Moex.Dto;

namespace RfBondManagement.Engine.Integration.Moex
{
    public class MoexDividendsRequest : MoexBaseRequest<JsonDividends>
    {
        protected string _ticker;

        protected override string _requestUrl => $"/securities/{_ticker}/dividends";

        public MoexDividendsRequest(string ticker)
        {
            _ticker = ticker;
        }
    }
}