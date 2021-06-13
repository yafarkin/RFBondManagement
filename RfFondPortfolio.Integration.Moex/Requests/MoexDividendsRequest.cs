using RfFondPortfolio.Integration.Moex.JsonDto;

namespace RfFondPortfolio.Integration.Moex.Requests
{
    internal class MoexDividendsRequest : MoexBaseRequest<JsonDividends>
    {
        protected string _ticker;

        protected override string _requestUrl => $"/securities/{_ticker}/dividends";

        public MoexDividendsRequest(string ticker)
        {
            _ticker = ticker;
        }
    }
}