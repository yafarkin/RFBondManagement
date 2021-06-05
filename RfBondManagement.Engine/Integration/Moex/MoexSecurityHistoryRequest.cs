using System;
using System.Collections.Generic;
using RfBondManagement.Engine.Integration.Moex.Dto;

namespace RfBondManagement.Engine.Integration.Moex
{
    public class MoexSecurityHistoryRequest : MoexBaseCursorRequest
    {
        protected string _engine;
        protected string _market;
        protected string _security;

        protected override string _requestUrl => $"/history/engines/{_engine}/markets/{_market}/securities/{_security}";

        public MoexSecurityHistoryRequest(string engine, string market, string security, DateTime? from = null, DateTime? to = null)
        {
            _engine = engine;
            _market = market;
            _security = security;

            if (from != null || to != null)
            {
                _addGetParams = new List<Tuple<string, string>>();
                if (from != null)
                {
                    _addGetParams.Add(new Tuple<string, string>("from", from.ToString()));
                }

                if (to != null)
                {
                    _addGetParams.Add(new Tuple<string, string>("to", to.ToString()));
                }
            }
        }
    }
}