using System;
using System.Collections.Generic;
using System.Linq;
using BackTesting.Interfaces;
using NLog;
using RfBondManagement.Engine.Calculations;
using RfFondPortfolio.Common.Dtos;
using RfFondPortfolio.Common.Interfaces;

namespace BackTesting
{
    public class BacktestEngine : IBacktestEngine
    {
        protected ILogger _logger;

        protected PortfolioEngine _portfolioEngine;

        protected IHistoryRepository _historyRepository;
        protected IPortfolioPaperActionRepository _paperActionRepository;
        protected IPortfolioMoneyActionRepository _moneyActionRepository;

        public BacktestEngine(
            ILogger logger,
            PortfolioEngine portfolioEngine,
            IHistoryRepository historyRepository,
            IPortfolioPaperActionRepository paperActionRepository,
            IPortfolioMoneyActionRepository moneyActionRepository)
        {
            _logger = logger;
            _portfolioEngine = portfolioEngine;
            _historyRepository = historyRepository;

            _paperActionRepository = paperActionRepository;
            _moneyActionRepository = moneyActionRepository;
        }

        public Statistic FillStatistic(DateTime date)
        {
            //var usdRubCourse = _historyRepository.GetNearUsdRubCourse(date);

            var statistic = new Statistic {Date = date};

            long papersCount = 0;
            decimal portfolioCost = 0;

            var content = _portfolioEngine.Build();
            foreach (var contentPaper in content.Papers)
            {
                papersCount += contentPaper.Count;
                contentPaper.MarketPrice = _historyRepository.GetNearHistoryPriceOnDate(contentPaper.Paper.SecId, date)?.ClosePrice ?? 0;

                portfolioCost += contentPaper.Count * contentPaper.MarketPrice;
            }

            statistic.SumInPortfolio = content.AvailSum;
            statistic.PapersCount = papersCount;
            statistic.PortfolioCost = portfolioCost;
            //statistic.UsdPortfolioCost = statistic.PortfolioCost / usdRubCourse.Course;

            return statistic;
        }

        public PortfolioEngine PortfolioEngine => _portfolioEngine;

        public void Run(IStrategy strategy, DateTime fromDate, ref DateTime toDate)
        {
            var statistics = new List<Statistic>();

            _logger.Info($"Start backtesting strategy '{strategy.Description}' from {fromDate} to {toDate}");

            var date = FindNearestDateWithPrices(strategy.Papers.ToList(), fromDate);
            if (date != fromDate)
            {
                _logger.Warn($"Shifted start date {fromDate} to nearest date with prices {date}");
            }

            strategy.Init(this, _portfolioEngine.Portfolio, date);

            if (date != fromDate)
            {
                _logger.Info($"Start date shifted from {fromDate} to {date}");
            }

            statistics.Add(FillStatistic(date));

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
                    var actions = _portfolioEngine.Automate(date);
                    foreach (var action in actions)
                    {
                        if (action is PortfolioPaperAction p)
                        {
                            _paperActionRepository.Insert(p);
                        }
                        else if (action is PortfolioMoneyAction m)
                        {
                            _moneyActionRepository.Insert(m);
                        }
                        else
                        {
                            throw new ApplicationException($"Неизвестный тип действия: {action.GetType()}");
                        }
                    }

                    if (date != nextProcessDate)
                    {
                        date = date.AddDays(1);
                    }
                    else
                    {
                        break;
                    }
                } while (true);

                var result = strategy.Process(date);

                statistics.Add(FillStatistic(date));

                if (!result)
                {
                    break;
                }

                date = date.AddDays(1);
            }

            _logger.Info($"Complete at {date}");
        }

        public void BuyPaper(DateTime date, AbstractPaper paper, long count)
        {
            var priceEntity = _historyRepository.GetHistoryPriceOnDate(paper.SecId, date);
            var price = priceEntity?.ClosePrice ?? 0;
            if (0 == price)
            {
                throw new ApplicationException($"Нет цены для {paper.SecId} на {date}");
            }

            _portfolioEngine.BuyPaper(paper, count, price, date);
        }

        public void SellPaper(DateTime date, AbstractPaper paper, long count)
        {
            var priceEntity = _historyRepository.GetHistoryPriceOnDate(paper.SecId, date);
            var price = priceEntity?.ClosePrice ?? 0;
            if (0 == price)
            {
                throw new ApplicationException($"Нет цены для {paper.SecId} на {date}");
            }

            _portfolioEngine.SellPaper(paper, count, price, date);
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