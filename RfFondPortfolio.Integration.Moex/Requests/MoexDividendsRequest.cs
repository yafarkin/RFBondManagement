using NLog;
using RfFondPortfolio.Integration.Moex.JsonDto;

namespace RfFondPortfolio.Integration.Moex.Requests
{
    internal class MoexDividendsRequest : MoexBaseRequest<JsonDividends>
    {
        protected string _ticker;

        protected override string _requestUrl => $"/securities/{_ticker}/dividends";

        public MoexDividendsRequest(ILogger logger, string ticker)
            : base(logger)
        {
            _ticker = ticker;
        }
    }
}