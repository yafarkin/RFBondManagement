using System;
using System.Collections.Generic;
using RfBondManagement.Engine.Integration.Moex.Dto;

namespace RfBondManagement.Engine.Integration.Moex
{
    public class MoexLastPriceRequest : MoexBaseRequest<JsonLastPrice>
    {
        protected string _market;
        protected string _board;
        protected string _ticker;

        protected override string _requestUrl => $"/engines/stock/markets/{_market}/boards/{_board}/securities/{_ticker}";

        protected override IEnumerable<Tuple<string, string>> _additionalParams => new List<Tuple<string, string>>
        {
            new Tuple<string, string>("iss.meta", "off"),
            new Tuple<string, string>("securities.columns", "SECID,PREVADMITTEDQUOTE")
        };

        public MoexLastPriceRequest(string market, string board, string ticker)
        {
            _market = market;
            _board = board;
            _ticker = ticker;
        }
    }
}