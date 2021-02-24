using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using BackTesting.Interfaces;
using NLog;
using RfBondManagement.Engine.Common;
using RfBondManagement.Engine.Interfaces;

namespace BackTesting
{
    public abstract class BaseEmptyStrategy : IStrategy
    {
        protected readonly ILogger _logger;
        protected readonly IHistoryDatabaseLayer _history;
        protected readonly IBondCalculator _bondCalculator;

        protected Portfolio _portfolio { get; set; }

        public abstract string Description { get; }
        public abstract void Init(Portfolio portfolio, DateTime date);
        public abstract bool Process(DateTime date);

        protected BaseEmptyStrategy(ILogger logger, IHistoryDatabaseLayer history, IBondCalculator bondCalculator)
        {
            _logger = logger;
            _history = history;
            _bondCalculator = bondCalculator;
        }

        protected virtual void BuyPaper(DateTime date, BaseStockPaper paper, long count)
        {
            var historyPrice = _history.GetHistoryPriceOnDate(paper.Code, date);
            var price = historyPrice.Price;

            var sum = count * price;
            var commission = sum * _portfolio.Settings.Commissions;
            var totalSum = sum + commission;

            //if (totalSum > _portfolio.Sum)
            //{
            //    throw new ApplicationException("Money is not enough");
            //}

            _portfolio.MoneyMoves.Add(new BaseMoneyMove
            {
                Comment = $"Buy {paper.Code}, count: {count:N0}, price: {price:C}, sum: {sum:C}, commission: {commission:C}, total sum: {totalSum:C}",
                Date = date,
                MoneyMoveType = MoneyMoveType.OutcomeBuyOnMarket,
                Sum = -sum,
            });

            _portfolio.MoneyMoves.Add(new BaseMoneyMove
            {
                Comment = $"Buy {paper.Code}, commission: {commission:C}",
                Date = date,
                MoneyMoveType = MoneyMoveType.OutcomeCommission,
                Sum = -commission,
            });

            _portfolio.Sum -= totalSum;

            if (paper is BaseBondPaper bondPaper)
            {
                var bp = _portfolio.Bonds.FirstOrDefault(p => p.Paper.Code == paper.Code);

                if (null == bp)
                {
                    bp = new BaseBondPaperInPortfolio
                    {
                        Paper = bondPaper,
                        Actions = new List<BaseAction<BaseBondPaper>>()
                    };

                    _portfolio.Bonds.Add(bp);
                }

                var nkd = _bondCalculator.CalculateNkd(bondPaper, date);

                bp.Actions.Add(new BondBuyAction
                {
                    Nkd = nkd * count,
                    Date = date,
                    Paper = bondPaper,
                    Count = count,
                    Price = price
                });
            }
            else if(paper is BaseSharePaper sharePaper)
            {
                var sp = _portfolio.Shares.FirstOrDefault(p => p.Paper.Code == paper.Code);

                if (null == sp)
                {
                    sp = new BaseSharePaperInPortfolio
                    {
                        Paper = sharePaper,
                        Actions = new List<BaseAction<BaseSharePaper>>()
                    };

                    _portfolio.Shares.Add(sp);
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

        protected virtual void SellPaper(DateTime date, BaseStockPaper paper, long count)
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
                shareInPortfolio = _portfolio.Shares.First(p => p.Paper.Code == paper.Code);
                avgPrice = shareInPortfolio.AvgBuySum;
            }
            else
            {
                bondInPortfolio = _portfolio.Bonds.First(p => p.Paper.Code == paper.Code);
                avgPrice = bondInPortfolio.AvgBuySum;
                nkd = _bondCalculator.CalculateNkd(bondInPortfolio.Paper, date);
            }

            var sum = count * price;
            var commission = sum * _portfolio.Settings.Commissions;
            var tax = price > avgPrice ? 0 : (avgPrice - price) * _portfolio.Settings.Tax / 100;
            var totalSum = sum - commission - tax + nkd;

            _portfolio.MoneyMoves.Add(new BaseMoneyMove
            {
                Comment = $"Sell {paper.Code}, count: {count:N0}, price: {price:C}, sum: {sum:C}, commission: {commission:C}, tax: {tax:C}, nkd: {nkd:C}, total sum: {totalSum:C}",
                Date = date,
                MoneyMoveType = MoneyMoveType.IncomeSellOnMarket,
                Sum = sum,
            });

            if (nkd > 0)
            {
                _portfolio.MoneyMoves.Add(new BaseMoneyMove
                {
                    Comment = $"Sell {paper.Code}, nkd: {nkd:C}",
                    Date = date,
                    MoneyMoveType = MoneyMoveType.IncomeCoupon,
                    Sum = nkd,
                });
            }

            _portfolio.MoneyMoves.Add(new BaseMoneyMove
            {
                Comment = $"Sell {paper.Code}, commission: {commission:C}",
                Date = date,
                MoneyMoveType = MoneyMoveType.OutcomeCommission,
                Sum = -commission,
            });

            if (tax > 0)
            {
                _portfolio.MoneyMoves.Add(new BaseMoneyMove
                {
                    Comment = $"Sell {paper.Code}, tax: {tax:C}",
                    Date = date,
                    MoneyMoveType = MoneyMoveType.OutcomeTax,
                    Sum = -tax,
                });
            }

            _portfolio.Sum += totalSum;

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
    }
}