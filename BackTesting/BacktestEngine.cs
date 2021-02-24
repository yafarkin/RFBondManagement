using System;
using System.Collections.Generic;
using System.Linq;
using BackTesting.Interfaces;
using NLog;
using RfBondManagement.Engine.Common;
using RfBondManagement.Engine.Interfaces;

namespace BackTesting
{
    public class BacktestEngine
    {
        protected ILogger _logger;
        protected IHistoryDatabaseLayer _history;

        public BacktestEngine(ILogger logger, IHistoryDatabaseLayer history)
        {
            _logger = logger;
            _history = history;
        }

        protected Statistic FillStatistic(Portfolio portfolio, DateTime date)
        {
            var statistic = new Statistic();
            statistic.Date = date;
            statistic.SumInPortfolio = portfolio.Sum;

            long papersCount = 0;
            decimal portfolioCost = 0;

            foreach (var bond in portfolio.Bonds)
            {
                var count = bond.Count;
                papersCount += count;

                var historyPrice = _history.GetHistoryPriceOnDate(bond.Paper.Code, date);
                portfolioCost += count * historyPrice.Price;
            }

            foreach (var share in portfolio.Shares)
            {
                var count = share.Count;
                papersCount += count;

                var historyPrice = _history.GetHistoryPriceOnDate(share.Paper.Code, date);
                portfolioCost += count * historyPrice.Price;
            }

            statistic.PapersCount = papersCount;
            statistic.PortfolioCost = portfolioCost;

            return statistic;
        }

        public void Run(Portfolio portfolio, IStrategy strategy, DateTime fromDate, DateTime toDate)
        {
            var statistics = new List<Statistic>();

            _logger.Info($"Start backtesting strategy '{strategy.Description}' from {fromDate} to {toDate}");

            var date = fromDate;
            strategy.Init(portfolio, date);

            statistics.Add(FillStatistic(portfolio, date));

            date = date.AddDays(1);

            while (date <= toDate)
            {
                var result = strategy.Process(date);

                statistics.Add(FillStatistic(portfolio, date));

                if (!result)
                {
                    break;
                }

                date = date.AddDays(1);
            }

            _logger.Info("Complete");
        }
    }
}