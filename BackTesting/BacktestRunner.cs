using System;
using System.Collections.Generic;
using System.Linq;
using BackTesting.Interfaces;
using NLog;
using RfBondManagement.Engine.Common;
using RfBondManagement.Engine.Interfaces;
using RfFondPortfolio.Common.Dtos;
using RfFondPortfolio.Common.Interfaces;

namespace BackTesting
{
    public class BacktestRunner
    {
        protected readonly ILogger _logger;
        protected readonly IPortfolioService _portfolioService;
        protected readonly IPortfolioCalculator _portfolioCalculator;
        protected readonly IPortfolioBuilder _portfolioBuilder;
        protected readonly IPortfolioActions _portfolioActions;
        protected readonly IHistoryRepository _historyRepository;

        public BacktestRunner(
            ILogger logger,
            IPortfolioService portfolioService,
            IPortfolioCalculator portfolioCalculator,
            IPortfolioBuilder portfolioBuilder,
            IPortfolioActions portfolioActions,
            IHistoryRepository historyRepository
            )
        {
            _logger = logger;
            _portfolioService = portfolioService;
            _portfolioCalculator = portfolioCalculator;
            _portfolioBuilder = portfolioBuilder;
            
            _portfolioActions = portfolioActions;

            _historyRepository = historyRepository;
        }

        public void Configure(Portfolio portfolio, ExternalImportType importType)
        {
            _portfolioService.Configure(portfolio, importType);
        }

        public void Run(IStrategy strategy, DateTime fromDate, ref DateTime toDate)
        {
            var statistics = new List<Statistic>();

            _logger.Info($"Start backtesting strategy '{strategy.Description}' from {fromDate} to {toDate}");

            var date = FindNearestDateWithPrices(strategy.Papers.ToList(), fromDate);
            if (date != fromDate)
            {
                _logger.Warn($"Shifted start date {fromDate} to nearest date with prices {date}");
            }

            strategy.Init(_portfolioService, date);

            if (date != fromDate)
            {
                _logger.Info($"Start date shifted from {fromDate} to {date}");
            }

            statistics.Add(_portfolioBuilder.FillStatistic(_portfolioService.Portfolio.Id, date));

            while (date <= toDate)
            {
                if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                {
                    date = date.AddDays(1);
                    continue;
                }

                var nextProcessDate = FindNearestDateWithPrices(strategy.Papers.ToList(), date);
                if (nextProcessDate != date)
                {
                    _logger.Warn($"Shifted current process date {date} to nearest date with prices {nextProcessDate}");
                }

                do
                {
                    var actions = _portfolioCalculator.Automate(_portfolioService.Portfolio, date);
                    _portfolioService.ApplyActions(actions);

                    if (date != nextProcessDate)
                    {
                        date = date.AddDays(1);
                    }
                    else
                    {
                        break;
                    }
                } while (true);

                var result = strategy.Process(date).GetAwaiter().GetResult();

                statistics.Add(_portfolioBuilder.FillStatistic(_portfolioService.Portfolio.Id, date));

                if (!result)
                {
                    break;
                }

                date = date.AddDays(1);
            }

            _logger.Info($"Complete at {date}");

            _portfolioBuilder.ExportToCsv(_portfolioService.Portfolio.Id, statistics);
        }

        public virtual DateTime FindNearestDateWithPrices(IList<string> codes, DateTime date)
        {
            var nearDate = date;
            for (var i = 0; i < codes.Count; i++)
            {
                var code = codes[i];

                var paperPrice = _historyRepository.GetHistoryPriceOnDate(code, nearDate);
                if (null == paperPrice)
                {
                    paperPrice = _historyRepository.GetNearHistoryPriceOnDate(code, nearDate);
                    if (null == paperPrice)
                    {
                        throw new ApplicationException($"Can't find paper price '{code}' after {date}");
                    }
                }

                if (paperPrice.When > nearDate)
                {
                    _logger.Warn($"Paper {code} shift date from {nearDate} to {paperPrice.When}");
                    nearDate = paperPrice.When;
                    i = -1;
                }
            }

            return nearDate;
        }
    }
}