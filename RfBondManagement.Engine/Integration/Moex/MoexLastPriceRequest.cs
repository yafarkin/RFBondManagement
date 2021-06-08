using RfBondManagement.Engine.Integration.Moex.Dto;

namespace RfBondManagement.Engine.Integration.Moex
{
    public class MoexLastPriceRequest : MoexBaseRequest<JsonLastPrice>
    {
        protected string _market;
        protected string _board;
        protected string _ticker;

        protected override string _requestUrl => $"/engines/stock/markets/{_market}/boards/{_board}/securities/{_ticker}";

        public MoexLastPriceRequest(string market, string board, string ticker)
        {
            _market = market;
            _board = board;
            _ticker = ticker;
        }
    }
}