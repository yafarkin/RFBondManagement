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

        public IPaperInPortfolio<AbstractPaper> BuildPaperInPortfolio(AbstractPaper paper, IEnumerable<PortfolioAction> allActions, DateTime? onDate = null)
        {
            long count;

            IPaperInPortfolio<AbstractPaper> paperInPortfolio;
            if (paper.PaperType == PaperType.Bond)
            {
                var bondInPortfolio = new BondInPortfolio(paper as BondPaper);
                bondInPortfolio.Aci = _bondCalculator.CalculateAci(bondInPortfolio.Paper, onDate ?? DateTime.Today);
                paperInPortfolio = bondInPortfolio;
            }
            else
            {
                paperInPortfolio = new ShareInPortfolio(paper as SharePaper);
            }

            paperInPortfolio.OnDate = onDate;

            var fifo = new List<Tuple<PortfolioPaperAction, PortfolioPaperAction, long>>();

            var paperActions = allActions.OfType<PortfolioPaperAction>().Where(a => a.SecId == paper.SecId);
            if (onDate.HasValue)
            {
                paperActions = paperActions.Where(a => a.When <= onDate);
            }

            paperInPortfolio.Actions = paperActions.ToList();
            foreach (var paperAction in paperInPortfolio.Actions)
            {
                var isBuy = paperAction.PaperAction == PaperActionType.Buy;
                count = paperAction.Count;

                if (isBuy)
                {
                    fifo.Add(new Tuple<PortfolioPaperAction, PortfolioPaperAction, long>(paperAction, null, paperAction.Count));
                }
                else
                {
                    for (var i = 0; i < fifo.Count; i++)
                    {
                        var t = fifo[i];
                        if (0 == t.Item3)
                        {
                            // all already sold, skip
                            continue;
                        }

                        if (t.Item3 >= count)
                        {
                            t = new Tuple<PortfolioPaperAction, PortfolioPaperAction, long>(t.Item1, paperAction, t.Item3 - count);
                            fifo[i] = t;
                            break;
                        }

                        count -= t.Item3;
                        t = new Tuple<PortfolioPaperAction, PortfolioPaperAction, long>(t.Item1, paperAction, 0);
                        fifo[i] = t;
                    }
                }
            }

            var sum = 0m;
            count = 0;
            foreach (var t in fifo)
            {
                if (0 == t.Item3)
                {
                    continue;
                }

                count += t.Item3;
                sum += t.Item3 * t.Item1.Value;
            }

            paperInPortfolio.FifoActions = fifo;
            paperInPortfolio.Count = count;
            paperInPortfolio.AveragePrice = 0 == count ? 0 : sum / count;


            return paperInPortfolio;
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

            var paperPortfolioActions = _paperActionRepository.Get().ToList();
            if (onDate.HasValue)
            {
                paperPortfolioActions = paperPortfolioActions.Where(a => a.When <= onDate).ToList();
            }

            var papers = new Dictionary<string, IPaperInPortfolio<AbstractPaper>>();
            var uniqueSecIds = paperPortfolioActions.Select(a => a.SecId).Distinct();
            foreach (var secId in uniqueSecIds)
            {
                var paperDefinition = _paperRepository.Get().Single(p => p.SecId == secId);
                var paperInPortfolio = BuildPaperInPortfolio(paperDefinition, paperPortfolioActions, onDate);

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

        public void BuyPaper(AbstractPaper paper, long count, decimal price, DateTime when = default(DateTime))
        {
            if (when == default(DateTime))
            {
                when = DateTime.UtcNow;
            }

            var sum = price * count;
            var commission = sum * _portfolio.Commissions / 100;

            var moneyAction = new PortfolioMoneyAction
            {
                MoneyAction = MoneyActionType.OutcomeBuyOnMarket,
                PortfolioId = _portfolio.Id,
                SecId = paper.SecId,
                When = when,
                Sum = sum,
                Comment = $"Покупка бумаги {paper.SecId}, количество {count}, цена {price}"
            };
            _moneyActionRepository.Insert(moneyAction);

            if (paper.PaperType == PaperType.Bond)
            {
                var aci = _bondCalculator.CalculateAci(paper as BondPaper, DateTime.Today);
                var aciSum = aci * count;
                commission = (sum + aciSum) * _portfolio.Commissions / 100;

                moneyAction = new PortfolioMoneyAction
                {
                    MoneyAction = MoneyActionType.OutcomeAci,
                    PortfolioId = _portfolio.Id,
                    SecId = paper.SecId,
                    When = when,
                    Sum = aciSum,
                    Comment = $"НКД {aci}, сумма НКД {aciSum}"
                };
                _moneyActionRepository.Insert(moneyAction);
            }

            moneyAction = new PortfolioMoneyAction
            {
                MoneyAction = MoneyActionType.OutcomeCommission,
                PortfolioId = _portfolio.Id,
                SecId = paper.SecId,
                When = when,
                Sum = commission,
                Comment = $"Списание комиссии, ставка {_portfolio.Commissions:P}"
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
                When = when
            };
            _paperActionRepository.Insert(paperAction);
        }

        public void SellPaper(AbstractPaper paper, long count, DateTime when = default(DateTime))
        {
            if (when == default(DateTime))
            {
                when = DateTime.UtcNow;
            }



            throw new NotImplementedException();
        }
    }
}