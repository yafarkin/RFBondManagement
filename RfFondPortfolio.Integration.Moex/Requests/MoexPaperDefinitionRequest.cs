using NLog;
using RfFondPortfolio.Integration.Moex.JsonDto;

namespace RfFondPortfolio.Integration.Moex.Requests
{
    internal class MoexPaperDefinitionRequest : MoexBaseRequest<JsonPaperDefinition>
    {
        protected string _ticker;

        public MoexPaperDefinitionRequest(ILogger logger, string ticker)
            : base(logger)
        {
            _ticker = ticker;
        }

        protected override string _requestUrl => $"/securities/{_ticker}";
    }
}