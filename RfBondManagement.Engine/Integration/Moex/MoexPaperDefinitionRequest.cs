using RfBondManagement.Engine.Integration.Moex.Dto;

namespace RfBondManagement.Engine.Integration.Moex
{
    public class MoexPaperDefinitionRequest : MoexBaseRequest<JsonPaperDefinition>
    {
        protected string _ticker;

        public MoexPaperDefinitionRequest(string ticker)
        {
            _ticker = ticker;
        }

        protected override string _requestUrl => $"/securities/{_ticker}";
    }
}