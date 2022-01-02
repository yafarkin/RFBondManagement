﻿using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using RfBondManagement.Engine.Common;
using RfBondManagement.Engine.Interfaces;
using RfFondPortfolio.Common.Dtos;
using RfFondPortfolio.Common.Interfaces;

namespace BackTesting.Strategies
{
    public class BuyAndHoldStrategy : BaseEmptyStrategy
    {
        protected bool _reinvestIncome;
        protected decimal _initialSum;
        protected decimal _monthlyIncome;

        /// <summary>
        /// Use Value Averaging (VA) method instead Dollar Cost Avergaing (DCA)
        /// </summary>
        /// <remarks>see https://investprofit.info/value-averaging/</remarks>
        protected bool _useVaMethod;

        protected DateTime _nextMonthlyIncome;

        protected Dictionary<string, PortfolioStructureLeafPaper> _flattenPapers;
        protected Dictionary<string, AbstractPaper> _papers;

        protected readonly IPaperRepository _paperRepository;
        protected readonly IPortfolioService _portfolioService;
        protected readonly IPortfolioCalculator _portfolioCalculator;
        protected readonly IPortfolioBuilder _portfolioBuilder;
        protected readonly IAdviser _adviser;

        public BuyAndHoldStrategy(
            ILogger logger,
            IHistoryRepository historyRepository,
            IBondCalculator bondCalculator,
            IPortfolioService portfolioService,
            IPortfolioCalculator portfolioCalculator,
            IPortfolioBuilder portfolioBuilder,
            IPaperRepository paperRepository,
            IAdviserFactory adviserFactory
            )
            : base(logger, historyRepository, bondCalculator)
        {
            _paperRepository = paperRepository;
            
            _portfolioService = portfolioService;
            _portfolioCalculator = portfolioCalculator;
            _portfolioBuilder = portfolioBuilder;
            
            _adviser = adviserFactory.GetAdviser(Constants.Adviser.BuyAndHold.Name);
        }

        protected IList<PortfolioStructureLeafPaper> FlattenPaper(PortfolioStructureLeaf leaf)
        {
            var result = leaf.Papers != null && leaf.Papers.Count > 0 ? new List<PortfolioStructureLeafPaper>(leaf.Papers) : new List<PortfolioStructureLeafPaper>();

            if (leaf.Children != null)
            {
                foreach (var child in leaf.Children)
                {
                    result.AddRange(FlattenPaper(child));
                }
            }

            return result;
        }

        public Portfolio Configure(bool useVaMethod, bool reinvestIncome, decimal initialSum, decimal monthlyIncome, decimal tax, decimal commission, PortfolioStructureLeaf rootLeaf)
        {
            _useVaMethod = useVaMethod;
            _reinvestIncome = reinvestIncome;
            _initialSum = initialSum;
            _monthlyIncome = monthlyIncome;

            _portfolio = new Portfolio
            {
                Id = Guid.NewGuid(),
                Tax = tax,
                Commissions = commission,
                LongTermBenefit = true,
                Actual = true,
                Name = $"BackTesting {DateTime.Now}",
                RootLeaf = rootLeaf
            };
            
            var secIds = _historyRepository.Get().Select(x => x.SecId).Distinct().ToHashSet();
            _papers = _paperRepository.Get().Where(p => secIds.Contains(p.SecId)).ToDictionary(p => p.SecId);

            _flattenPapers = FlattenPaper(rootLeaf).ToDictionary(x => x.Paper.SecId);
            
            return _portfolio;
        }

        public override IEnumerable<string> Papers => _flattenPapers.Select(x => x.Key);

        public override string Description => "Buy and hold" + (_reinvestIncome ? " (with reinvest)" : string.Empty);

        public override void Init(Portfolio portfolio, DateTime date)
        {
            _portfolio = portfolio;
            _nextMonthlyIncome = date.AddMonths(1);

            _portfolioService.ApplyActions(_portfolioCalculator.MoveMoney(_initialSum, MoneyActionType.IncomeExternal, "Начальная сумма", null, date));
        }

        public override bool Process(DateTime date)
        {
            if (_monthlyIncome > 0 && date >= _nextMonthlyIncome)
            {
                _nextMonthlyIncome = _nextMonthlyIncome.AddMonths(1);

                _portfolioService.ApplyActions(_portfolioCalculator.MoveMoney(_monthlyIncome, MoneyActionType.IncomeExternal, "Ежемесячное пополнение", null, date));
                _logger.Info($"Monthly income, {_monthlyIncome:C}");
            }

            var content = _portfolioBuilder.Build(_portfolio.Id, date);

            if (_reinvestIncome || 0 == content.Papers.Count)
            {
                var actions = _adviser.Advise(_portfolio, new Dictionary<string, string>
                {
                    {Constants.Adviser.BuyAndHold.P_AvailSum, content.AvailSum.ToString()}
                }).GetAwaiter().GetResult();
                _portfolioService.ApplyActions(actions);
                //BalancePortfolio(date);
            }

            var statistic = _portfolioBuilder.FillStatistic(_portfolio.Id, date);
            var portfolioCost = statistic.PortfolioCost;
            _logger.Info($"Portfolio cost on {date} is {portfolioCost:C}; free sum: {content.AvailSum:C}");

            return true;
        }

        // protected virtual void FindMaxDisbalance(DateTime date, decimal portfolioCost, PortfolioAggregatedContent content, out string secId, out decimal percentDisbalance)
        // {
        //     if (_portfolioPercent.Count() == 1)
        //     {
        //         secId = _portfolioPercent.First().Item1;
        //         percentDisbalance = -1;
        //
        //         return;
        //     }
        //
        //     var l = new List<Tuple<string, decimal, decimal>>();
        //
        //     foreach (var p in _portfolioPercent)
        //     {
        //         decimal paperSum = 0;
        //
        //         var paperInPortfolio = content.Papers.SingleOrDefault(x => x.Paper.SecId == p.Item1);
        //         if (paperInPortfolio != null)
        //         {
        //             var historyPrice = _historyRepository.GetHistoryPriceOnDate(paperInPortfolio.Paper.SecId, date);
        //             var count = paperInPortfolio.Count;
        //             paperSum = count * historyPrice.ClosePrice;
        //         }
        //
        //         var actualPercent = 0 == portfolioCost ? 0 : paperSum / portfolioCost * 100;
        //
        //         var t = new Tuple<string, decimal, decimal>(p.Item1, p.Item2, actualPercent);
        //         l.Add(t);
        //     }
        //
        //     var sl = l.OrderByDescending(x => x.Item2 - x.Item3).First();
        //
        //     secId = sl.Item1;
        //     percentDisbalance = Math.Abs(sl.Item2 - sl.Item3);
        // }

        // protected virtual void BalancePortfolio(DateTime date)
        // {
        //     while (true)
        //     {
        //         string secId;
        //         decimal needPercent;
        //
        //         var content = _portfolioBuilder.Build(_portfolio.Id, date);
        //         var statistic = _portfolioBuilder.FillStatistic(_portfolio.Id, date);
        //         var portfolioCost = statistic.PortfolioCost;
        //
        //         FindMaxDisbalance(date, portfolioCost, content, out secId, out needPercent);
        //
        //         var priceEntity = _historyRepository.GetHistoryPriceOnDate(secId, date);
        //         var price = priceEntity.LegalClosePrice;
        //
        //         var paper = _papers[secId];
        //         if (paper.PaperType == PaperType.Bond)
        //         {
        //             var aci = _bondCalculator.CalculateAci(paper as BondPaper, date);
        //             price += aci;
        //         }
        //
        //         long paperCount = 0;
        //         var sumToPaper = needPercent == -1 ? content.AvailSum : (portfolioCost + content.AvailSum) * needPercent / 100;
        //
        //         if (_useVaMethod)
        //         {
        //             var incomeTotal = content.TotalIncome;
        //
        //             var paperInPortfolio = content.Papers.SingleOrDefault(p => p.Paper.SecId == secId);
        //             if (null == paperInPortfolio)
        //             {
        //                 paperCount = Convert.ToInt64(Math.Floor(sumToPaper / price));
        //             }
        //             else
        //             {
        //                 var alreadyExist = paperInPortfolio.Count;
        //
        //                 paperCount = Convert.ToInt64((incomeTotal - price * alreadyExist) / price);
        //                 if (paperCount <= 0)
        //                 {
        //                     break;
        //                 }
        //             }
        //         }
        //         else
        //         {
        //             paperCount = Convert.ToInt64(Math.Floor(sumToPaper / price));
        //         }
        //
        //         if (0 == paperCount && content.AvailSum > price)
        //         {
        //             paperCount = 1;
        //         }
        //
        //         if (paperCount > 0)
        //         {
        //             var totalSum = paperCount * price;
        //             if (totalSum > content.AvailSum)
        //             {
        //                 break;
        //             }
        //
        //             _logger.Info($"Buy {secId}, price {price:C}, count: {paperCount:N0}, total sum: {totalSum:C}; free sum: {content.AvailSum:C}");
        //             var actions = _portfolioCalculator.BuyPaper(paper, paperCount, price, date);
        //             _portfolioService.ApplyActions(actions);
        //         }
        //         else
        //         {
        //             break;
        //             //_logger.Error($"Can't buy paper {p.Item1} to portfolio, price {p.Item2:C} too high ever for one paper (budget to paper {sumToPaper:C}");
        //         }
        //     }
        // }
    }
}