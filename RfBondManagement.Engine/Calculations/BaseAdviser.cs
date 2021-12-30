using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NLog;
using RfBondManagement.Engine.Interfaces;
using RfFondPortfolio.Common.Dtos;

namespace RfBondManagement.Engine.Calculations
{
    public abstract class BaseAdviser : IAdviser
    {
        protected readonly ILogger _logger;
        protected readonly IPortfolioBuilder _portfolioBuilder;
        protected readonly IPortfolioService _portfolioService;
        protected readonly IPortfolioCalculator _portfolioCalculator;

        protected BaseAdviser(ILogger logger, IPortfolioBuilder portfolioBuilder, IPortfolioCalculator portfolioCalculator, IPortfolioService portfolioService)
        {
            _logger = logger;

            _portfolioBuilder = portfolioBuilder;
            _portfolioCalculator = portfolioCalculator;
            _portfolioService = portfolioService;
        }

        protected decimal? GetAsDecimal(IDictionary<string, string> p, string key, decimal? defaultValue = null)
        {
            if (!p.ContainsKey(key))
            {
                return defaultValue;
            }

            return Convert.ToDecimal(p[key]);
        }

        protected bool? GetAsBool(IDictionary<string, string> p, string key, bool? defaultValue = null)
        {
            if (!p.ContainsKey(key))
            {
                return defaultValue;
            }

            return Convert.ToBoolean(p[key]);
        }

        protected DateTime? GetAsDateTime(IDictionary<string, string> p, string key, DateTime? defaultValue = null)
        {
            if (!p.ContainsKey(key))
            {
                return defaultValue;
            }

            return Convert.ToDateTime(p[key]);
        }

        public abstract Task<IEnumerable<PortfolioAction>> Advise(Portfolio portfolio, IDictionary<string, string> p);
    }
}