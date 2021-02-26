using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BackTesting.Interfaces;
using BackTesting.Strategies;
using NLog;
using RfBondManagement.Engine;
using RfBondManagement.Engine.Common;
using RfBondManagement.Engine.Interfaces;
using Unity;
using Unity.Storage;

namespace BackTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = ConfigureDI.Configure();
            container.RegisterInstance(container);
            container.RegisterType<IBacktestEngine, BacktestEngine>();

            var logger = container.Resolve<ILogger>();
            logger.Info("Start import historical data");

            var historyImport = container.Resolve<HistoryImport>();
            historyImport.Run();

            logger.Info("Start back testing");

            var history = container.Resolve<IHistoryDatabaseLayer>();
            var strategy = container.Resolve<BuyAndHoldStrategy>();

            decimal initialSum = 10000;

            var portfolio = strategy.Configure(true, initialSum, 10000, new List<Tuple<string, decimal>>
            {
                //new Tuple<string, decimal>("SBERP", 100),
                new Tuple<string, decimal>("SBERP", 20),
                new Tuple<string, decimal>("MTSS", 20),
                new Tuple<string, decimal>("FXIT", 15),
                new Tuple<string, decimal>("FXUS", 20),
                new Tuple<string, decimal>("FXCN", 20),
                new Tuple<string, decimal>("FXRU", 5)
            }, new Settings
            {
                Commissions = 0.061m,
                Tax = 13
            });

            var startDate = new DateTime(2019, 1, 1);
            var endDate = new DateTime(2020, 12, 31);

            var backtest = container.Resolve<IBacktestEngine>();
            backtest.Run(portfolio, strategy, startDate, ref endDate);

            var nearEndDate = backtest.FindNearestDateWithPrices(strategy.Papers.ToList(), endDate);
            var usdRubCourse = history.GetNearUsdRubCourse(nearEndDate);

            var statistic = backtest.FillStatistic(portfolio, nearEndDate);

            logger.Info($"Portfolio cost on {nearEndDate} is {statistic.PortfolioCost:C}");
            logger.Info($"Unused sum: {portfolio.Sum:C}");
            logger.Info($"Sum in USD: {statistic.UsdPortfolioCost.ToString("C", CultureInfo.InvariantCulture)}");

            var totalIncome = portfolio.MoneyMoves.Where(x => x.Sum >= 0).Sum(x => x.Sum);
            logger.Info($"Total money income: {totalIncome:C}");

            decimal totalIncomeUsd = 0;
            foreach (var moneyMove in portfolio.MoneyMoves.Where(x => x.Sum >= 0))
            {
                var usdRubCourseMove = history.GetNearUsdRubCourse(moneyMove.Date);
                totalIncomeUsd += moneyMove.Sum / usdRubCourseMove.Course;
            }

            logger.Info($"Total money income, USD: {totalIncomeUsd.ToString("C", CultureInfo.InvariantCulture)}");

            var totalIncomeByType = portfolio.MoneyMoves.Where(x => x.Sum >= 0).GroupBy(x => x.MoneyMoveType);
            foreach (var t in totalIncomeByType)
            {
                var sum = t.Sum(x => x.Sum);
                logger.Info($"Money income, type {t.Key}: {sum:C}");
            }

            var profit = portfolio.Sum + statistic.PortfolioCost - totalIncome;
            var profitUsd = profit / usdRubCourse.Course;

            var profitPercent = (portfolio.Sum + statistic.PortfolioCost) / totalIncome;
            var profitPercentUsd = ((portfolio.Sum + statistic.PortfolioCost) / usdRubCourse.Course) / totalIncomeUsd;

            var totalYears = Convert.ToDecimal((endDate - startDate).TotalDays / 365);

            logger.Info($"Profit: {profit:C}, in USD: {profitUsd.ToString("C", CultureInfo.InvariantCulture)}");
            logger.Info($"Profit percent: {profitPercent:P} (annual: {profitPercent / totalYears:P}), in USD: {profitPercentUsd:P} (annual: {profitPercentUsd / totalYears:P})");

            foreach (var bp in portfolio.Bonds)
            {
                var currPrice = history.GetNearHistoryPriceOnDate(bp.Paper.Code, nearEndDate);
                var totalSum = bp.Count * currPrice.Price;
                var part = totalSum / statistic.PortfolioCost;
                logger.Info($"Bond {bp.Paper.Code}; count: {bp.Count}; part in portfolio: {part:P}; avg price: {bp.AvgBuySum:C} (market: {currPrice.Price:C}); sum: {bp.Count * bp.AvgBuySum:C} (market: {totalSum:C})");
            }

            foreach (var sp in portfolio.Shares)
            {
                var currPrice = history.GetNearHistoryPriceOnDate(sp.Paper.Code, nearEndDate);
                var totalSum = sp.Count * currPrice.Price;
                var part = totalSum / statistic.PortfolioCost;
                logger.Info($"Share {sp.Paper.Code}; count: {sp.Count}; part in portfolio: {part:P}; avg price: {sp.AvgBuySum:C} (market: {currPrice.Price:C}); sum: {sp.Count * sp.AvgBuySum:C} (market: {totalSum:C})");
            }
        }
    }
}
