﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using BackTesting.Interfaces;
using BackTesting.Strategies;
using NLog;
using RfBondManagement.Engine;
using RfBondManagement.Engine.Common;
using RfBondManagement.Engine.Interfaces;
using RfFondPortfolio.Common.Dtos;
using RfFondPortfolio.Common.Interfaces;
using Unity;
using Unity.Resolution;

namespace BackTesting
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var container = ConfigureDI.Configure();
            container.RegisterInstance(container);

            var logger = container.Resolve<ILogger>();

            logger.Info("Start back testing");

            var paperRepository = container.Resolve<IPaperRepository>();
            var history = container.Resolve<IHistoryRepository>();

            var initialSum = 100_000m;
            var monthlyIncome = 10_000m;

            var startDate = new DateTime(2015, 1, 1);
            var endDate = new DateTime(2021, 12, 31);

            var portfolioPercent = new List<Tuple<string, decimal>>
            {
                //new Tuple<string, decimal>("SBERP", 100),
                new Tuple<string, decimal>("SBERP", 20),
                new Tuple<string, decimal>("MTSS", 20),
                new Tuple<string, decimal>("FXIT", 15),
                new Tuple<string, decimal>("FXUS", 20),
                new Tuple<string, decimal>("FXCN", 20),
                new Tuple<string, decimal>("FXRU", 5)
            };

            var importFactory = container.Resolve<IExternalImportFactory>();
            var importEngine = importFactory.GetImpl(ExternalImportType.Moex);
            var historyEngine = container.Resolve<HistoryEngine>();

            foreach (var t in portfolioPercent)
            {
                var p = paperRepository.Get().SingleOrDefault(x => x.SecId == t.Item1);
                if (null == p)
                {
                    logger.Info($"Importing {t.Item1} definition");
                    var pd = await importEngine.ImportPaper(logger, t.Item1);
                    paperRepository.Insert(pd);
                }

                var h = history.Get().FirstOrDefault(x => x.SecId == t.Item1);
                if (null == h || h.When > startDate)
                {
                    logger.Info($"Importing {t.Item1} history prices");
                    await historyEngine.ImportHistory(t.Item1);
                }
            }

            var useVa = false;
            var strategy = container.Resolve<BuyAndHoldStrategy>();
            var portfolio = strategy.Configure(useVa, true, initialSum, monthlyIncome, portfolioPercent, 13, 0.061m);

            var portfolioRepository = container.Resolve<IPortfolioRepository>();
            portfolioRepository.Insert(portfolio);

<<<<<<< HEAD
            var backtest = container.Resolve<BacktestRunner>(new ParameterOverride("portfolio", portfolio), new ParameterOverride("importType", ExternalImportType.Moex));
=======
            var backtest = container.Resolve<BacktestEngine>(new ParameterOverride("portfolio", portfolio), new ParameterOverride("importType", ExternalImportType.Moex));
>>>>>>> 800c56c5fb81ba95c72db44262f96c6929f7683a
            backtest.Configure(portfolio, ExternalImportType.Moex);
            backtest.Run(strategy, startDate, ref endDate);

            var nearEndDate = backtest.FindNearestDateWithPrices(strategy.Papers.ToList(), endDate);
            //var usdRubCourse = history.GetNearUsdRubCourse(nearEndDate);

            var portfolioBuilder = container.Resolve<IPortfolioBuilder>();

            var statistic = portfolioBuilder.FillStatistic(portfolio.Id, nearEndDate);
            var content = portfolioBuilder.Build(portfolio.Id, nearEndDate);

            logger.Info($"Portfolio cost on {nearEndDate} is {statistic.PortfolioCost:C}");
            logger.Info($"Unused sum: {content.AvailSum:C}");
            logger.Info($"Sum in USD: {statistic.UsdPortfolioCost.ToString("C", CultureInfo.InvariantCulture)}");

            var totalIncome = content.Sums.Where(s => MoneyActionTypeHelper.IncomeTypes.Contains(s.Key)).Sum(s => s.Value);
            logger.Info($"Total money income: {totalIncome:C}");

            //decimal totalIncomeUsd = 0;
            //foreach (var moneyMove in portfolio.MoneyMoves.Where(x => x.Sum >= 0))
            //{
            //    var usdRubCourseMove = history.GetNearUsdRubCourse(moneyMove.Date);
            //    totalIncomeUsd += moneyMove.Sum / usdRubCourseMove.Course;
            //}

            //logger.Info($"Total money income, USD: {totalIncomeUsd.ToString("C", CultureInfo.InvariantCulture)}");

            foreach (var t in content.Sums.Where(s => MoneyActionTypeHelper.IncomeTypes.Contains(s.Key)))
            {
                logger.Info($"Money income, type {t.Key}: {t.Value:C}");
            }

            var profit = content.AvailSum + statistic.PortfolioCost - totalIncome;
            //var profitUsd = profit / usdRubCourse.Course;

            var profitPercent = (content.AvailSum + statistic.PortfolioCost) / totalIncome;
            //var profitPercentUsd = ((portfolio.Sum + statistic.PortfolioCost) / usdRubCourse.Course) / totalIncomeUsd;

            var totalYears = Convert.ToDecimal((endDate - startDate).TotalDays / 365);

            logger.Info($"Profit: {profit:C}; profit percent: {profitPercent:P} (annual: {profitPercent / totalYears:P})");

            //logger.Info($"Profit: {profit:C}, in USD: {profitUsd.ToString("C", CultureInfo.InvariantCulture)}");
            //logger.Info($"Profit percent: {profitPercent:P} (annual: {profitPercent / totalYears:P}), in USD: {profitPercentUsd:P} (annual: {profitPercentUsd / totalYears:P})");

            foreach (var p in content.Papers)
            {
                var currPrice = history.GetNearHistoryPriceOnDate(p.Paper.SecId, nearEndDate);
                var totalSum = p.Count * currPrice.ClosePrice;
                var part = totalSum / statistic.PortfolioCost;
                logger.Info($"Paper {p.Paper.SecId}; count: {p.Count}; part in portfolio: {part:P}; avg price: {p.AveragePrice:C} (market: {currPrice.ClosePrice:C}); sum: {p.Count * p.AveragePrice:C} (market: {totalSum:C})");
            }

            var db = container.Resolve<IDatabaseLayer>();
            db.Dispose();
        }
    }
}
