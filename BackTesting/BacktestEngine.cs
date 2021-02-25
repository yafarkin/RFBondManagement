using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using BackTesting.Interfaces;
using NLog;
using RfBondManagement.Engine.Common;
using RfBondManagement.Engine.Interfaces;

namespace BackTesting
{
    public class BacktestEngine : IBacktestEngine
    {
        protected ILogger _logger;
        protected IHistoryDatabaseLayer _history;
        protected IList<SplitInfo> _splits;
        protected IList<DividendInfo> _dividends;

        public BacktestEngine(ILogger logger, IHistoryDatabaseLayer history)
        {
            _logger = logger;
            _history = history;
            _splits = _history.GetSplitInfo();
            _dividends = _history.GetDividendInfo();
        }

        public Statistic FillStatistic(Portfolio portfolio, DateTime date)
        {
            var usdRubCourse = _history.GetNearUsdRubCourse(date);

            var statistic = new Statistic {Date = date, SumInPortfolio = portfolio.Sum};

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
            statistic.UsdPortfolioCost = statistic.PortfolioCost / usdRubCourse.Course;

            return statistic;
        }

        public void Run(Portfolio portfolio, IStrategy strategy, DateTime fromDate, ref DateTime toDate)
        {
            var statistics = new List<Statistic>();

            _logger.Info($"Start backtesting strategy '{strategy.Description}' from {fromDate} to {toDate}");

            var date = FindNearestDateWithPrices(strategy.Papers.ToList(), fromDate);
            if (date != fromDate)
            {
                _logger.Warn($"Shifted start date {fromDate} to nearest date with prices {date}");
            }

            strategy.Init(portfolio, date);

            if (date != fromDate)
            {
                _logger.Info($"Start date shifted from {fromDate} to {date}");
            }

            statistics.Add(FillStatistic(portfolio, date));

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
                    ProcessSplitIfNeeds(portfolio, date);
                    CalculateIncomeCoupons(portfolio, date);
                    CheckBondCloseDates(portfolio, date);
                    CalculateIncomeDividends(portfolio, date);

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

                statistics.Add(FillStatistic(portfolio, date));

                if (!result)
                {
                    break;
                }

                date = date.AddDays(1);
            }

            _logger.Info($"Complete at {date}");
        }

        protected void CalculateIncomeDividends(Portfolio portfolio, DateTime date)
        {
            var codes = portfolio.Shares.Select(x => x.Paper.Code).ToList();
            var dividendsOnDate = _dividends.Where(x => x.CutOffDate == date && codes.Contains(x.Code)).ToList();
            if (!dividendsOnDate.Any())
            {
                return;
            }

            foreach (var dividendInfo in dividendsOnDate)
            {
                var code = dividendInfo.Code;
                var dividend = dividendInfo.Dividend;
                var dividendTax = dividend * portfolio.Settings.Tax / 100;

                var paper = portfolio.Shares.Single(x => x.Paper.Code == code);

                var totalSum = (dividend - dividendTax) * paper.Count;

                portfolio.Sum += totalSum;

                portfolio.MoneyMoves.Add(new BaseMoneyMove
                {
                    Comment = $"Income dividends from {code}, per one paper is {dividend:C} without tax",
                    Date = date,
                    MoneyMoveType = MoneyMoveType.IncomeDividend,
                    Sum = totalSum
                });

                _logger.Info($"Income dividends from {code}, value {dividend:C}, tax {dividendTax:C}, total sum: {totalSum:C}");
            }
        }

        protected void ProcessSplitIfNeeds(Portfolio portfolio, DateTime date)
        {
            var splitOnDate = _splits.Where(x => x.Date == date).ToList();
            if (!splitOnDate.Any())
            {
                return;
            }

            var splitCodes = splitOnDate.Select(x => x.Code).ToList();

            var bondsInPortfolio = portfolio.Bonds.Where(x => splitCodes.Contains(x.Paper.Code)).ToList();
            foreach (var bondInPortfolio in bondsInPortfolio)
            {
                var split = splitOnDate.First(x => x.Date == date && x.Code == bondInPortfolio.Paper.Code);

                _logger.Info($"Perform split for {bondInPortfolio.Paper.Code}, multiplier {split.Multiplier:N4}");

                foreach (var action in bondInPortfolio.Actions)
                {
                    action.Count = Convert.ToInt64(Math.Floor(action.Count * split.Multiplier));
                    action.Price /= split.Multiplier;
                }
            }

            var sharesInPortfolio = portfolio.Shares.Where(x => splitCodes.Contains(x.Paper.Code)).ToList();
            foreach (var shareInPortfolio in sharesInPortfolio)
            {
                var split = splitOnDate.First(x => x.Date == date && x.Code == shareInPortfolio.Paper.Code);

                _logger.Info($"Perform split for {shareInPortfolio.Paper.Code}, multiplier {split.Multiplier:N4}");

                foreach (var action in shareInPortfolio.Actions)
                {
                    action.Count = Convert.ToInt64(Math.Floor(action.Count * split.Multiplier));
                    action.Price /= split.Multiplier;
                }
            }
        }

        public decimal CalculateIncomeCoupons(Portfolio portfolio, DateTime date)
        {
            decimal incomeCoupons = 0;

            if (0 == portfolio.Bonds.Count)
            {
                return incomeCoupons;
            }

            foreach (var bondPaperInPortfolio in portfolio.Bonds)
            {
                var bond = bondPaperInPortfolio.Paper;
                var coupon = bond.Coupons.FirstOrDefault(c => c.Date == date);
                if (null == coupon)
                {
                    continue;
                }

                var nkd = coupon.Value * bondPaperInPortfolio.Count;
                var tax = nkd * portfolio.Settings.Tax / 100;
                var totalSum = nkd - tax;

                portfolio.Sum += totalSum;
                incomeCoupons += totalSum;

                portfolio.MoneyMoves.Add(new BaseMoneyMove
                {
                    Comment = $"Income coupons {bond.Code}, value: {coupon.Value}, tax: {tax:C}, total sum: {totalSum:C}",
                    Date = date,
                    MoneyMoveType = MoneyMoveType.IncomeCoupon,
                    Sum = totalSum,
                });

                if (tax > 0)
                {
                    portfolio.MoneyMoves.Add(new BaseMoneyMove
                    {
                        Comment = $"Income coupons {bond.Code}, tax: {tax:C}",
                        Date = date,
                        MoneyMoveType = MoneyMoveType.OutcomeTax,
                        Sum = -tax,
                    });
                }
            }

            return incomeCoupons;
        }

        public decimal CheckBondCloseDates(Portfolio portfolio, DateTime date)
        {
            decimal incomeClosedBonds = 0;

            if (0 == portfolio.Bonds.Count)
            {
                return incomeClosedBonds;
            }

            foreach (var bondInPortfolio in portfolio.Bonds)
            {
                var bond = bondInPortfolio.Paper;
                if (bond.MaturityDate == date || bond.OfferDate == date)
                {
                    var totalSum = bond.BondPar * bondInPortfolio.Count;

                    portfolio.Sum += totalSum;

                    bondInPortfolio.Actions.Add(new BondSellAction
                    {
                        Nkd = 0,
                        Date = date,
                        Paper = bond,
                        Count = bondInPortfolio.Count,
                        Price = bond.BondPar
                    });

                    portfolio.MoneyMoves.Add(new BaseMoneyMove
                    {
                        Comment = $"Close bond {bond.Code}, count: {bondInPortfolio.Count:N0}, close par: {bond.BondPar:C}, total sum: {totalSum:C}",
                        Date = date,
                        MoneyMoveType = MoneyMoveType.IncomeCloseBond,
                        Sum = totalSum,
                    });
                }
            }

            return incomeClosedBonds;
        }

        public void BuyPaper(Portfolio portfolio, DateTime date, BaseStockPaper paper, long count, IBondCalculator bondCalculator)
        {
            var historyPrice = _history.GetHistoryPriceOnDate(paper.Code, date);
            var price = historyPrice.Price;

            var sum = count * price;
            var commission = sum * portfolio.Settings.Commissions / 100;
            var totalSum = sum + commission;

            //if (totalSum > _portfolio.Sum)
            //{
            //    throw new ApplicationException("Money is not enough");
            //}

            portfolio.MoneyMoves.Add(new BaseMoneyMove
            {
                Comment = $"Buy {paper.Code}, count: {count:N0}, price: {price:C}, sum: {sum:C}, commission: {commission:C}, total sum: {totalSum:C}",
                Date = date,
                MoneyMoveType = MoneyMoveType.OutcomeBuyOnMarket,
                Sum = -sum,
            });

            portfolio.MoneyMoves.Add(new BaseMoneyMove
            {
                Comment = $"Buy {paper.Code}, commission: {commission:C}",
                Date = date,
                MoneyMoveType = MoneyMoveType.OutcomeCommission,
                Sum = -commission,
            });

            portfolio.Sum -= totalSum;

            if (paper is BaseBondPaper bondPaper)
            {
                var bp = portfolio.Bonds.FirstOrDefault(p => p.Paper.Code == paper.Code);

                if (null == bp)
                {
                    bp = new BaseBondPaperInPortfolio
                    {
                        Paper = bondPaper,
                        Actions = new List<BaseAction<BaseBondPaper>>()
                    };

                    portfolio.Bonds.Add(bp);
                }

                var nkd = bondCalculator.CalculateNkd(bondPaper, date);

                bp.Actions.Add(new BondBuyAction
                {
                    Nkd = nkd * count,
                    Date = date,
                    Paper = bondPaper,
                    Count = count,
                    Price = price
                });
            }
            else if (paper is BaseSharePaper sharePaper)
            {
                var sp = portfolio.Shares.FirstOrDefault(p => p.Paper.Code == paper.Code);

                if (null == sp)
                {
                    sp = new BaseSharePaperInPortfolio
                    {
                        Paper = sharePaper,
                        Actions = new List<BaseAction<BaseSharePaper>>()
                    };

                    portfolio.Shares.Add(sp);
                }

                sp.Actions.Add(new ShareBuyAction
                {
                    Date = date,
                    Paper = sharePaper,
                    Count = count,
                    Price = price
                });
            }
            else
            {
                throw new NotSupportedException($"Unknown paper type: {paper.GetType()}");
            }
        }

        public void SellPaper(Portfolio portfolio, DateTime date, BaseStockPaper paper, long count, IBondCalculator bondCalculator)
        {
            var historyPrice = _history.GetHistoryPriceOnDate(paper.Code, date);
            var price = historyPrice.Price;

            bool isShare;
            if (paper is BaseBondPaper)
            {
                isShare = false;
            }
            else if (paper is BaseSharePaper)
            {
                isShare = true;
            }
            else
            {
                throw new NotSupportedException($"Unknown paper type: {paper.GetType()}");
            }

            decimal avgPrice;
            decimal nkd = 0;

            BaseSharePaperInPortfolio shareInPortfolio = null;
            BaseBondPaperInPortfolio bondInPortfolio = null;
            if (isShare)
            {
                shareInPortfolio = portfolio.Shares.First(p => p.Paper.Code == paper.Code);
                avgPrice = shareInPortfolio.AvgBuySum;
            }
            else
            {
                bondInPortfolio = portfolio.Bonds.First(p => p.Paper.Code == paper.Code);
                avgPrice = bondInPortfolio.AvgBuySum;
                nkd = bondCalculator.CalculateNkd(bondInPortfolio.Paper, date);
            }

            var sum = count * price;
            var commission = sum * portfolio.Settings.Commissions;
            var tax = price > avgPrice ? 0 : (avgPrice - price) * portfolio.Settings.Tax / 100;
            var totalSum = sum - commission - tax + nkd;

            portfolio.MoneyMoves.Add(new BaseMoneyMove
            {
                Comment = $"Sell {paper.Code}, count: {count:N0}, price: {price:C}, sum: {sum:C}, commission: {commission:C}, tax: {tax:C}, nkd: {nkd:C}, total sum: {totalSum:C}",
                Date = date,
                MoneyMoveType = MoneyMoveType.IncomeSellOnMarket,
                Sum = sum,
            });

            if (nkd > 0)
            {
                portfolio.MoneyMoves.Add(new BaseMoneyMove
                {
                    Comment = $"Sell {paper.Code}, nkd: {nkd:C}",
                    Date = date,
                    MoneyMoveType = MoneyMoveType.IncomeCoupon,
                    Sum = nkd,
                });
            }

            portfolio.MoneyMoves.Add(new BaseMoneyMove
            {
                Comment = $"Sell {paper.Code}, commission: {commission:C}",
                Date = date,
                MoneyMoveType = MoneyMoveType.OutcomeCommission,
                Sum = -commission,
            });

            if (tax > 0)
            {
                portfolio.MoneyMoves.Add(new BaseMoneyMove
                {
                    Comment = $"Sell {paper.Code}, tax: {tax:C}",
                    Date = date,
                    MoneyMoveType = MoneyMoveType.OutcomeTax,
                    Sum = -tax,
                });
            }

            portfolio.Sum += totalSum;

            if (!isShare)
            {
                bondInPortfolio.Actions.Add(new BondSellAction
                {
                    Nkd = nkd * count,
                    Date = date,
                    Paper = paper as BaseBondPaper,
                    Count = count,
                    Price = price
                });
            }
            else
            {
                shareInPortfolio.Actions.Add(new ShareSellAction
                {
                    Date = date,
                    Paper = paper as BaseSharePaper,
                    Count = count,
                    Price = price
                });
            }
        }

        public virtual DateTime FindNearestDateWithPrices(IList<string> codes, DateTime date)
        {
            var nearDate = date;
            for (var i = 0; i < codes.Count; i++)
            {
                var code = codes[i];

                var paperPrice = _history.GetHistoryPriceOnDate(code, nearDate);
                if (null == paperPrice)
                {
                    paperPrice = _history.GetNearHistoryPriceOnDate(code, nearDate);
                    if (null == paperPrice)
                    {
                        throw new ApplicationException($"Can't find paper price '{code}' after {date}");
                    }
                }

                if (paperPrice.Date > nearDate)
                {
                    _logger.Warn($"Paper {code} shift date from {nearDate} to {paperPrice.Date}");
                    nearDate = paperPrice.Date;
                    i = -1;
                }
            }

            return nearDate;
        }

    }
}