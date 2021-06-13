using RfFondPortfolio.Integration.Moex.JsonDto;

namespace RfFondPortfolio.Integration.Moex.Requests
{
    internal class MoexPaperDefinitionRequest : MoexBaseRequest<JsonPaperDefinition>
    {
        protected string _ticker;

        public MoexPaperDefinitionRequest(string ticker)
        {
            _ticker = ticker;
        }

        protected override string _requestUrl => $"/securities/{_ticker}";
    }
}