using System;
using System.Collections.Generic;

namespace RfFondPortfolio.Integration.Moex.Requests
{
    internal class MoexSecurityHistoryRequest : MoexBaseCursorRequest
    {
        protected string _market;
        protected string _board;
        protected string _ticker;

        protected override string _requestUrl => $"/history/engines/stock/markets/{_market}/boards/{_board}/securities/{_ticker}";

        protected override IEnumerable<Tuple<string, string>> _additionalParams => new List<Tuple<string, string>>
        {
            new Tuple<string, string>("iss.meta", "off")
        };

        public MoexSecurityHistoryRequest(string market, string board, string ticker, DateTime? from = null, DateTime? to = null)
        {
            _market = market;
            _board = board;
            _ticker = ticker;

            if (from != null || to != null)
            {
                _addGetParams = new List<Tuple<string, string>>();
                if (from != null)
                {
                    _addGetParams.Add(new Tuple<string, string>("from", from.Value.ToString("yyyy-MM-dd")));
                }

                if (to != null)
                {
                    _addGetParams.Add(new Tuple<string, string>("to", to.Value.ToString("yyyy-MM-dd")));
                }
            }
        }
    }
}