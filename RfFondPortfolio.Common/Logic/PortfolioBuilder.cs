using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RfFondPortfolio.Common.Dtos;
using RfFondPortfolio.Common.Interfaces;

namespace RfFondPortfolio.Common.Logic
{
    public static class PortfolioBuilder
    {
        public static PortfolioAggregatedContent Build(IEnumerable<PortfolioAction> actions, IPaperRepository paperRepository)
        {
            var sums = new Dictionary<MoneyActionType, decimal>();
            var paperActions = new Dictionary<string, List<PortfolioPaperAction>>();
            var paperCounts = new Dictionary<string, List<KeyValuePair<long, decimal>>>();

            foreach (var action in actions)
            {
                if (action is PortfolioMoneyAction moneyAction)
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
                else if (action is PortfolioPaperAction paperAction)
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
                else
                {
                    throw new ApplicationException($"Неизвестный тип действия в портфеле: {action.GetType()}");
                }
            }

            var papers = new Dictionary<string, PaperInPortfolio<AbstractPaper>>();
            foreach (var paperCount in paperCounts)
            {
                var secId = paperCount.Key;
                var count = paperCount.Value.Sum(x => x.Key);
                var sum = paperCount.Value.Sum(x => x.Value);
                var averagePrice = sum / count;

                var paperDefinition = paperRepository.Get().Single(p => p.SecId == secId);

                papers.Add(secId, new PaperInPortfolio<AbstractPaper>
                {
                    Paper = paperDefinition,
                    AveragePrice = averagePrice,
                    Count = count,
                    Actions = paperActions[secId]
                });
            }

            var content = new PortfolioAggregatedContent
            {
                Papers = new ReadOnlyCollection<PaperInPortfolio<AbstractPaper>>(papers.Select(x => x.Value).ToList()),
                Sums = new ReadOnlyDictionary<MoneyActionType, decimal>(sums)
            };

            return content;
        }
    }
}