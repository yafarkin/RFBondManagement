using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RfBondManagement.Engine.Interfaces;
using RfFondPortfolio.Common.Dtos;
using RfFondPortfolio.Common.Interfaces;

namespace RfBondManagement.Engine.Calculations
{
    public class PortfolioBuilder : IPortfolioBuilder
    {
        protected readonly IBondCalculator _bondCalculator;
        protected readonly IPortfolioActions _portfolioActions;
        protected readonly IPaperRepository _paperRepository;

        public PortfolioBuilder(IBondCalculator bondCalculator, IPortfolioActions portfolioActions, IPaperRepository paperRepository)
        {
            _bondCalculator = bondCalculator;
            _portfolioActions = portfolioActions;
            _paperRepository = paperRepository;
        }

        protected void PerformFifoSplit(decimal multiplier, IList<FifoAction> fifo)
        {
            if (1 == multiplier || 0 == fifo.Count)
            {
                return;
            }

            var lastFifo = fifo.Last();

            var buyPaperAction = new PortfolioPaperAction
            {
                Comment = lastFifo.BuyAction.Comment,
                Id = Guid.Empty,
                PaperAction = lastFifo.BuyAction.PaperAction,
                PortfolioId = lastFifo.BuyAction.PortfolioId,
                SecId = lastFifo.BuyAction.SecId,

                Count = Convert.ToInt64(Math.Floor(lastFifo.BuyAction.Count * multiplier)),
                Value = lastFifo.BuyAction.Value / multiplier,

                When = lastFifo.BuyAction.When
            };

            var newCount = Convert.ToInt64(Math.Floor(lastFifo.Count * multiplier));

            var updatedFifo = new FifoAction(buyPaperAction, lastFifo.SellAction, newCount);
            fifo.Remove(lastFifo);
            fifo.Add(updatedFifo);
        }

        /// <summary>
        /// Возвращает информацию о бумаге в портфеле
        /// </summary>
        /// <param name="paper">Бумага</param>
        /// <param name="allPaperActions">Все действия с бумагами в портфеле</param>
        /// <param name="onDate">Дата, на которую надо построить информацию, null - на текущий момент</param>
        /// <returns>Информация по бумаге</returns>
        public IPaperInPortfolio<AbstractPaper> BuildPaperInPortfolio(AbstractPaper paper, IEnumerable<PortfolioPaperAction> allPaperActions, DateTime? onDate = null)
        {
            long count;

            IPaperInPortfolio<AbstractPaper> paperInPortfolio;
            if (paper.PaperType == PaperType.Bond)
            {
                var bondInPortfolio = new BondInPortfolio(paper as BondPaper);
                bondInPortfolio.Aci = _bondCalculator.CalculateAci(bondInPortfolio.Paper, onDate ?? DateTime.UtcNow.Date);
                paperInPortfolio = bondInPortfolio;
            }
            else
            {
                paperInPortfolio = new ShareInPortfolio(paper as SharePaper);
            }

            paperInPortfolio.OnDate = onDate;

            var fifo = new List<FifoAction>();

            var paperActions = allPaperActions.Where(a => a.SecId == paper.SecId);
            if (onDate.HasValue)
            {
                paperActions = paperActions.Where(a => a.When <= onDate);
            }

            paperInPortfolio.Actions = paperActions.ToList();
            foreach (var paperAction in paperInPortfolio.Actions)
            {
                if (paperAction.PaperAction == PaperActionType.Split)
                {
                    PerformFifoSplit(paperAction.Value, fifo);
                    continue;
                }

                if (paperAction.PaperAction == PaperActionType.Coupon ||
                    paperAction.PaperAction == PaperActionType.Dividend)
                {
                    continue;
                }

                var isBuy = paperAction.PaperAction == PaperActionType.Buy;
                count = paperAction.Count;

                if (isBuy)
                {
                    fifo.Add(new FifoAction(paperAction, null, paperAction.Count));
                }
                else
                {
                    for (var i = 0; i < fifo.Count; i++)
                    {
                        var t = fifo[i];
                        if (0 == t.Count)
                        {
                            // all already sold, skip
                            continue;
                        }

                        if (t.Count >= count)
                        {
                            t = new FifoAction(t.BuyAction, paperAction, t.Count - count);
                            fifo[i] = t;
                            break;
                        }

                        count -= t.Count;
                        t = new FifoAction(t.BuyAction, paperAction, 0);
                        fifo[i] = t;
                    }
                }
            }

            var sum = 0m;
            count = 0;
            foreach (var t in fifo)
            {
                if (0 == t.Count)
                {
                    continue;
                }

                count += t.Count;
                sum += t.Count * t.BuyAction.Value;
            }

            paperInPortfolio.FifoActions = fifo;
            paperInPortfolio.Count = count;
            paperInPortfolio.AveragePrice = 0 == count ? 0 : Math.Round(sum / count, 2);

            return paperInPortfolio;
        }

        public PortfolioAggregatedContent Build(Guid portfolioId, DateTime? onDate = null)
        {
            var sums = new Dictionary<MoneyActionType, decimal>();
            var moneyPortfolioActions = _portfolioActions.MoneyActions(portfolioId).ToList();
            if (onDate.HasValue)
            {
                moneyPortfolioActions = moneyPortfolioActions.Where(a => a.When <= onDate).ToList();
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

            var paperPortfolioActions = _portfolioActions.PaperActions(portfolioId).ToList();
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

                papers.Add(secId, paperInPortfolio);
            }

            var content = new PortfolioAggregatedContent
            {
                Papers = new ReadOnlyCollection<IPaperInPortfolio<AbstractPaper>>(papers.Select(x => x.Value).ToList()),
                Sums = new ReadOnlyDictionary<MoneyActionType, decimal>(sums)
            };

            return content;
        }
    }
}