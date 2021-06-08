using System;
using System.Collections.Generic;
using System.Linq;
using BackTesting.Interfaces;
using NLog;
using RfBondManagement.Engine.Common;
using RfBondManagement.Engine.Interfaces;

namespace BackTesting.Strategies
{
    public class BuyAndHoldStrategy : BaseEmptyStrategy
    {
        protected bool _reinvestIncome;
        protected IEnumerable<Tuple<string, decimal>> _portfolioPercent;

        protected decimal _initialSum;
        protected decimal _monthlyIncome;

        protected DateTime _nextMonthlyIncome;

        protected Dictionary<string, BaseStockPaper> _papers;

        public BuyAndHoldStrategy(ILogger logger, IHistoryDatabaseLayer history, IBondCalculator bondCalculator, IBacktestEngine backtestEngine)
            : base(logger, history, bondCalculator, backtestEngine)
        {
        }
        
        public Portfolio Configure(bool reinvestIncome, decimal initialSum, decimal monthlyIncome, IEnumerable<Tuple<string, decimal>> portfolioPercent, Settings settings)
        {
            _reinvestIncome = reinvestIncome;
            _portfolioPercent = portfolioPercent;
            _initialSum = initialSum;
            _monthlyIncome = monthlyIncome;

            var portfolio = new Portfolio
            {
                Settings = settings,
                Sum = initialSum
            };

            _papers = _history.GetHistoryPapers().ToDictionary(x => x.SecId);

            return portfolio;
        }

        public override IEnumerable<string> Papers => _portfolioPercent.Select(x => x.Item1);

        public override string Description => "Buy and hold" + (_reinvestIncome ? " (with reinvest)" : string.Empty);

        public override void Init(Portfolio portfolio, DateTime date)
        {
            _portfolio = portfolio;
            _nextMonthlyIncome = date.AddMonths(1);

            portfolio.MoneyMoves.Add(new BaseMoneyMove
            {
                Comment = "Initial sum",
                Sum = _initialSum,
                Date = date,
                MoneyMoveType = MoneyMoveType.IncomeExternal
            });

        }

        public override bool Process(DateTime date)
        {
            if (_monthlyIncome > 0 && date >= _nextMonthlyIncome)
            {
                _portfolio.Sum += _monthlyIncome;
                _nextMonthlyIncome = _nextMonthlyIncome.AddMonths(1);

                _portfolio.MoneyMoves.Add(new BaseMoneyMove
                {
                    Comment = $"Monthly income {_monthlyIncome:C}",
                    Date = date,
                    MoneyMoveType = MoneyMoveType.IncomeExternal,
                    Sum = _monthlyIncome
                });

                _logger.Info($"Monthly income, {_monthlyIncome:C}");
            }

            if (_reinvestIncome || (0 == _portfolio.Bonds.Count && 0 == _portfolio.Shares.Count))
            {
                BalancePortfolio(date);
            }

            var portfolioCost = CalcPortfolioCost(date);
            _logger.Info($"Portfolio cost on {date} is {portfolioCost:C}; free sum: {_portfolio.Sum:C}");

            return true;
        }

        protected virtual decimal CalcPortfolioCost(DateTime date)
        {
            var portfolioCost = 0m;

            foreach (var bond in _portfolio.Bonds)
            {
                var count = bond.Count;

                var historyPrice = _history.GetHistoryPriceOnDate(bond.Paper.SecId, date);
                portfolioCost += count * historyPrice.Price;
            }

            foreach (var share in _portfolio.Shares)
            {
                var count = share.Count;

                var historyPrice = _history.GetHistoryPriceOnDate(share.Paper.SecId, date);
                portfolioCost += count * historyPrice.Price;
            }

            return portfolioCost;
        }

        protected virtual void FindMaxDisbalance(DateTime date, decimal portfolioCost, out string code, out decimal percentDisbalance)
        {
            if (_portfolioPercent.Count() == 1)
            {
                code = _portfolioPercent.First().Item1;
                percentDisbalance = -1;

                return;
            }

            var l = new List<Tuple<string, decimal, decimal>>();

            foreach (var p in _portfolioPercent)
            {
                decimal paperSum = 0;

                var bp = _portfolio.Bonds.SingleOrDefault(x => x.Paper.SecId == p.Item1);
                if (bp != null)
                {
                    var historyPrice = _history.GetHistoryPriceOnDate(bp.Paper.SecId, date);
                    var count = bp.Count;
                    paperSum = count * historyPrice.Price;
                }
                else
                {
                    var sp = _portfolio.Shares.SingleOrDefault(x => x.Paper.SecId == p.Item1);

                    if (sp != null)
                    {
                        var historyPrice = _history.GetHistoryPriceOnDate(sp.Paper.SecId, date);
                        var count = sp.Count;
                        paperSum = count * historyPrice.Price;
                    }
                }

                var actualPercent = 0 == portfolioCost ? 0 : paperSum / portfolioCost * 100;

                var t = new Tuple<string, decimal, decimal>(p.Item1, p.Item2, actualPercent);
                l.Add(t);
            }

            var sl = l.OrderByDescending(x => x.Item2 - x.Item3).First();

            code = sl.Item1;
            percentDisbalance = Math.Abs(sl.Item2 - sl.Item3);
        }

        protected virtual void BalancePortfolio(DateTime date)
        {
            while (true)
            {
                string code;
                decimal needPercent;

                var portfolioCost = CalcPortfolioCost(date);

                FindMaxDisbalance(date, portfolioCost, out code, out needPercent);

                var paperPrice = _history.GetHistoryPriceOnDate(code, date);
                var price = paperPrice.Price;

                var paper = _papers[code];
                if (paper is BaseBondPaper bondPaper)
                {
                    var nkd = _bondCalculator.CalculateNkd(bondPaper, date);
                    price += nkd;
                }

                var sumToPaper = needPercent == -1 ? _portfolio.Sum : (portfolioCost + _portfolio.Sum) * needPercent / 100;
                var paperCount = Convert.ToInt64(Math.Floor(sumToPaper / price));

                if (0 == paperCount && _portfolio.Sum > price)
                {
                    paperCount = 1;
                }

                if (paperCount > 0)
                {
                    var totalSum = paperCount * price;

                    if (totalSum > _portfolio.Sum)
                    {
                        break;
                    }

                    _backtestEngine.BuyPaper(_portfolio, date, paper, paperCount, _bondCalculator);
                    _logger.Info($"Buy {code}, price {price:C}, count: {paperCount:N0}, total sum: {totalSum:C}; free sum: {_portfolio.Sum:C}");
                }
                else
                {
                    break;
                    //_logger.Error($"Can't buy paper {p.Item1} to portfolio, price {p.Item2:C} too high ever for one paper (budget to paper {sumToPaper:C}");
                }
            }
        }
    }
}