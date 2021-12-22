using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog;
using RfFondPortfolio.Common.Dtos;
using RfFondPortfolio.Common.Interfaces;

namespace RfBondManagement.Engine.Calculations
{
    public static class BuyAdviser
    {
        public static async Task<IEnumerable<PortfolioAction>> Advise(ILogger logger, PortfolioEngine engine, decimal availSum, bool allowSell, bool useVa, DateTime? onDate, IExternalImport import, IHistoryRepository historyRepository)
        {
            var result = new List<PortfolioAction>();

            var content = engine.Build();

            var portfolio = engine.Portfolio;
            var rootLeaf = portfolio.RootLeaf;
            var flattenPapers = FlattenPaperStructure(rootLeaf, 1);
            var portfolioPapers = content.Papers.ToDictionary(x => x.Paper.SecId);
            var paperPrices = new Dictionary<string, decimal>(flattenPapers.Count);

            foreach (var paper in flattenPapers)
            {
                if (null == onDate)
                {
                    var price = await import.LastPrice(logger, paper.Paper);
                    paperPrices.Add(paper.Paper.SecId, price.Price);
                }
                else
                {
                    var historyPrice = historyRepository.GetNearHistoryPriceOnDate(paper.Paper.SecId, onDate.Value);
                    paperPrices.Add(paper.Paper.SecId, historyPrice.LegalClosePrice);
                }
            }

            foreach (var kv in portfolioPapers)
            {
                if (paperPrices.ContainsKey(kv.Key))
                {
                    continue;
                }

                if (null == onDate)
                {
                    var price = await import.LastPrice(logger, kv.Value.Paper);
                    paperPrices.Add(kv.Key, price.Price);
                }
                else
                {
                    var historyPrice = historyRepository.GetNearHistoryPriceOnDate(kv.Key, onDate.Value);
                    paperPrices.Add(kv.Key, historyPrice.LegalClosePrice);
                }
            }

            // количество бумаг к изменению
            var paperToChange = new Dictionary<string, long>();

            while (availSum > 0)
            {
                var totalVolume = 0m;
                foreach (var paperInPortfolio in content.Papers)
                {
                    totalVolume += paperInPortfolio.Count * paperPrices[paperInPortfolio.Paper.SecId];
                }

                // K: secId, V: price, объём в портфеле с учётом цены, необходимый объём, разница в объёмах
                var balance = new Dictionary<string, Tuple<decimal, decimal, decimal, decimal>>();

                foreach (var paper in flattenPapers)
                {
                    var secId = paper.Paper.SecId;
                    var price = paperPrices[secId];

                    var countInPortfolio = portfolioPapers.ContainsKey(secId) ? portfolioPapers[secId].Count : 0;
                    var changeCount = paperToChange.ContainsKey(secId) ? paperToChange[secId] : 0;
                    var count = countInPortfolio + changeCount;

                    var volume = count * price / totalVolume;
                    var needVolume = paper.Volume;

                    balance.Add(secId, new Tuple<decimal, decimal, decimal, decimal>(price, volume, needVolume, needVolume - volume));
                }

                foreach (var paper in portfolioPapers)
                {
                    if (balance.ContainsKey(paper.Key))
                    {
                        continue;
                    }

                    var secId = paper.Key;
                    var price = paperPrices[secId];

                    var countInPortfolio = portfolioPapers[secId].Count;
                    var changeCount = paperToChange.ContainsKey(secId) ? paperToChange[secId] : 0;
                    var count = countInPortfolio + changeCount;

                    var volume = count * price / totalVolume;
                    var needVolume = 0;

                    balance.Add(secId, new Tuple<decimal, decimal, decimal, decimal>(price, volume, needVolume, needVolume - volume));
                }

                if (allowSell)
                {
                    // выбираем бумаги, от которых надо избавится, и сумма больше чем цена за одну бумагу
                    var sellPapers = balance.Where(x => x.Value.Item4 < 0 && Math.Abs(x.Value.Item4) >= x.Value.Item1);
                    foreach (var kv in sellPapers)
                    {
                        // TODO: переделать движок, что бы он не писал в репозиторий. тогда и буду возвращать здесь список
                        var countToSell = Convert.ToInt64(kv.Value.Item4 / kv.Value.Item1);
                        if (!paperToChange.ContainsKey(kv.Key))
                        {
                            paperToChange.Add(kv.Key, countToSell);
                        }
                        else
                        {
                            paperToChange[kv.Key] += countToSell;
                        }

                        availSum += countToSell * kv.Value.Item1;
                    }
                }

                // берем саму разбаланисированную бумагу и пытаемся купить
                var buyPaper = balance
                    .Where(x => x.Value.Item4 > 0 && x.Value.Item4 >= x.Value.Item1 && x.Value.Item1 <= availSum)
                    .OrderByDescending(x => x.Value.Item4)
                    .FirstOrDefault();

                if (null == buyPaper.Key)
                {
                    // мы ничего уже не можем купить
                    break;
                }

                var countToBuy = buyPaper.Value.Item4 > availSum
                    ? Convert.ToInt64(availSum / buyPaper.Value.Item1)
                    : Convert.ToInt64(buyPaper.Value.Item4 / buyPaper.Value.Item1);

                if (!paperToChange.ContainsKey(buyPaper.Key))
                {
                    paperToChange.Add(buyPaper.Key, countToBuy);
                }
                else
                {
                    paperToChange[buyPaper.Key] += countToBuy;
                }

                availSum -= countToBuy * buyPaper.Value.Item1;
            }

            return result;
        }

        public static IList<PortfolioStructureLeafPaper> FlattenPaperStructure(PortfolioStructureLeaf entryLeaf, decimal percent)
        {
            var result = new List<PortfolioStructureLeafPaper>();

            if (entryLeaf == null)
            {
                return result;
            }

            var totalVolume =
                  (entryLeaf.Papers?.Sum(x => x.Volume) ?? 0)
                + (entryLeaf.Children?.Sum(x => x.Volume) ?? 0);

            if (entryLeaf.Papers != null)
            {
                foreach (var paper in entryLeaf.Papers)
                {
                    var paperLocalPercent = paper.Volume / totalVolume;
                    var paperGlobalPercent = paperLocalPercent * percent;
                    result.Add(new PortfolioStructureLeafPaper
                    {
                        Paper = paper.Paper,
                        Volume = paperGlobalPercent
                    });
                }
            }

            if (entryLeaf.Children != null)
            {
                foreach (var child in entryLeaf.Children)
                {
                    var childLocalPercent = child.Volume / totalVolume;
                    var childGlobalPercent = childLocalPercent * percent;

                    result.AddRange(FlattenPaperStructure(child, childGlobalPercent));
                }
            }

            return result;
        }
    }
}