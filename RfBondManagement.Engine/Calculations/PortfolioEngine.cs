using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using RfBondManagement.Engine.Interfaces;
using RfFondPortfolio.Common.Dtos;
using RfFondPortfolio.Common.Interfaces;

namespace RfBondManagement.Engine.Calculations
{
    public class PortfolioEngine
    {
        protected readonly Portfolio _portfolio;
        protected readonly IPaperRepository _paperRepository;
        protected readonly IPortfolioPaperActionRepository _paperActionRepository;
        protected readonly IPortfolioMoneyActionRepository _moneyActionRepository;

        protected readonly IExternalImport _import;
        protected readonly IBondCalculator _bondCalculator;
        
        public PortfolioEngine(Portfolio portfolio, IExternalImport import, IPaperRepository paperRepository, IPortfolioMoneyActionRepository moneyActionRepository, IPortfolioPaperActionRepository paperActionRepository, IBondCalculator bondCalculator)
        {
            _portfolio = portfolio;
            _import = import;
            _paperRepository = paperRepository;
            _moneyActionRepository = moneyActionRepository;
            _paperActionRepository = paperActionRepository;
            _bondCalculator = bondCalculator;
        }

        public async Task<PortfolioAggregatedContent> Build(DateTime? onDate = null, bool importPrice = false)
        {
            var sums = new Dictionary<MoneyActionType, decimal>();
            var moneyPortfolioActions = _moneyActionRepository.Get();
            if (onDate.HasValue)
            {
                moneyPortfolioActions = moneyPortfolioActions.Where(a => a.When <= onDate);
            }

            foreach (var moneyAction in moneyPortfolioActions)
            {
                if (!sums.ContainsKey(moneyAction.MoneyAction))
                {
                    sums.Add(moneyAction.MoneyAction, moneyAction.Sum);
                }
                else
                {
                    sums[moneyAction.MoneyAction] += moneyAction.Sum;
                }
            }

            var paperActions = new Dictionary<string, List<PortfolioPaperAction>>();
            var paperCounts = new Dictionary<string, List<KeyValuePair<long, decimal>>>();
            var paperPortfolioActions = _paperActionRepository.Get();
            if (onDate.HasValue)
            {
                paperPortfolioActions = paperPortfolioActions.Where(a => a.When <= onDate);
            }

            foreach (var paperAction in paperPortfolioActions)
            {
                var isBuy = paperAction.PaperAction == PaperActionType.Buy;
                var count = paperAction.Count;
                var price = paperAction.Value;
                var secId = paperAction.SecId;

                if (!paperActions.ContainsKey(secId))
                {
                    paperActions.Add(secId, new List<PortfolioPaperAction>());
                }
                paperActions[secId].Add(paperAction);

                if (!paperCounts.ContainsKey(secId))
                {
                    paperCounts.Add(secId, new List<KeyValuePair<long, decimal>>());
                }
                var countList = paperCounts[secId];

                if (isBuy)
                {
                    countList.Add(new KeyValuePair<long, decimal>(count, price));
                }
                else
                {
                    while (count > 0 && countList.Count > 0)
                    {
                        var firstRecord = countList[0];
                        var firstCount = firstRecord.Key;
                        if (firstCount > count)
                        {
                            countList[0] = new KeyValuePair<long, decimal>(firstRecord.Key - count, firstRecord.Value);
                            count = 0;
                        }
                        else
                        {
                            count -= firstCount;
                            countList.RemoveAt(0);
                        }
                    }
                }
            }

            var papers = new Dictionary<string, IPaperInPortfolio<AbstractPaper>>();
            foreach (var paperCount in paperCounts)
            {
                var secId = paperCount.Key;
                var count = paperCount.Value.Sum(x => x.Key);
                var sum = paperCount.Value.Sum(x => x.Value) * count;
                var averagePrice = sum / count;

                var paperDefinition = _paperRepository.Get().Single(p => p.SecId == secId);

                decimal marketPrice = 0;
                if (importPrice)
                {
                    if (onDate.HasValue)
                    {
                        var historyPrice = await _import.HistoryPrice(paperDefinition, onDate, onDate);
                        marketPrice = historyPrice.FirstOrDefault()?.LegalClosePrice ?? 0;
                    }
                    else
                    {
                        var lastPrice = await _import.LastPrice(paperDefinition);
                        marketPrice = lastPrice.Price;
                    }
                }

                IPaperInPortfolio<AbstractPaper> paperInPortfolio;
                if (paperDefinition.PaperType == PaperType.Bond)
                {
                    paperInPortfolio = new BondInPortfolio(paperDefinition as BondPaper);
                }
                else
                {
                    paperInPortfolio = new ShareInPortfolio(paperDefinition as SharePaper);
                }

                paperInPortfolio.AveragePrice = averagePrice;
                paperInPortfolio.Count = count;
                paperInPortfolio.Actions = paperActions[secId];
                paperInPortfolio.MarketPrice = marketPrice;

                papers.Add(secId, paperInPortfolio);
            }

            var content = new PortfolioAggregatedContent
            {
                Papers = new ReadOnlyCollection<IPaperInPortfolio<AbstractPaper>>(papers.Select(x => x.Value).ToList()),
                Sums = new ReadOnlyDictionary<MoneyActionType, decimal>(sums)
            };

            return content;
        }

        public void BuyPaper(AbstractPaper paper, long count, decimal price)
        {
            var sum = price * count;
            var commission = sum * _portfolio.Commissions / 100;

            var moneyAction = new PortfolioMoneyAction
            {
                MoneyAction = MoneyActionType.OutcomeBuyOnMarket,
                PortfolioId = _portfolio.Id,
                SecId = paper.SecId,
                When = DateTime.UtcNow,
                Sum = sum
            };
            _moneyActionRepository.Insert(moneyAction);

            moneyAction = new PortfolioMoneyAction
            {
                MoneyAction = MoneyActionType.OutcomeCommission,
                PortfolioId = _portfolio.Id,
                SecId = paper.SecId,
                When = DateTime.UtcNow,
                Sum = commission
            };
            _moneyActionRepository.Insert(moneyAction);

            var paperAction = new PortfolioPaperAction
            {
                Count = count,
                PaperAction = PaperActionType.Buy,
                PortfolioId = _portfolio.Id,
                SecId = paper.SecId,
                Sum = sum,
                Value = price,
                When = DateTime.UtcNow
            };
            _paperActionRepository.Insert(paperAction);
        }
    }
}