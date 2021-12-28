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
        protected readonly IDictionary<string, string> _p;

        protected readonly ILogger _logger;
        protected readonly IPortfolioBuilder _portfolioBuilder;
        protected readonly IPortfolioLogic _portfolioLogic;
        protected readonly IPortfolioCalculator _portfolioCalculator;

        protected BaseAdviser(ILogger logger, IDictionary<string, string> p, IPortfolioBuilder portfolioBuilder, IPortfolioCalculator portfolioCalculator, IPortfolioLogic portfolioLogic)
        {
            _logger = logger;
            _p = p;

            _portfolioBuilder = portfolioBuilder;
            _portfolioCalculator = portfolioCalculator;
            _portfolioLogic = portfolioLogic;
        }

        protected decimal? GetAsDecimal(string key, decimal? defaultValue = null)
        {
            if (!_p.ContainsKey(key))
            {
                return defaultValue;
            }

            return Convert.ToDecimal(_p[key]);
        }

        protected bool? GetAsBool(string key, bool? defaultValue = null)
        {
            if (!_p.ContainsKey(key))
            {
                return defaultValue;
            }

            return Convert.ToBoolean(_p[key]);
        }

        protected DateTime? GetAsDateTime(string key, DateTime? defaultValue = null)
        {
            if (!_p.ContainsKey(key))
            {
                return defaultValue;
            }

            return Convert.ToDateTime(_p[key]);
        }

        public abstract Task<IEnumerable<PortfolioAction>> Advise(Portfolio portfolio);
    }
}