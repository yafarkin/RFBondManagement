using NLog;
using RfFondPortfolio.Integration.Moex.JsonDto;

namespace RfFondPortfolio.Integration.Moex.Requests
{
    internal class MoexBondCouponsRequest : MoexBaseRequest<JsonBondization>
    {
        protected string _ticker;

        public MoexBondCouponsRequest(ILogger logger, string ticker)
            : base(logger)
        {
            _ticker = ticker;
        }

        protected override string _requestUrl => $"/securities/{_ticker}/bondization";
    }
}