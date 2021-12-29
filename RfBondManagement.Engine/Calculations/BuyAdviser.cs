using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog;
using RfBondManagement.Engine.Common;
using RfBondManagement.Engine.Interfaces;
using RfFondPortfolio.Common.Dtos;

namespace RfBondManagement.Engine.Calculations
{
    public class BuyAdviser : BaseAdviser
    {
        public BuyAdviser(ILogger logger, IDictionary<string, string> p, IPortfolioBuilder portfolioBuilder, IPortfolioCalculator portfolioCalculator, IPortfolioService portfolioService)
            : base(logger, p, portfolioBuilder, portfolioCalculator, portfolioService)
        {
        }

        public override async Task<IEnumerable<PortfolioAction>> Advise(Portfolio portfolio)
        {
            var availSum = GetAsDecimal(Constants.Adviser.P_AvailSum) ?? 0;
            var allowSell = GetAsBool(Constants.Adviser.P_AllowSell, false);
            var onDate = GetAsDateTime(Constants.Adviser.P_OnDate);

            var result = new List<PortfolioAction>();

            if (0 == availSum)
            {
                return result;
            }

            var content = _portfolioBuilder.Build(portfolio.Id);

            var rootLeaf = portfolio.RootLeaf;
            var flattenPapers = FlattenPaperStructure(rootLeaf, 1).ToDictionary(x => x.Paper.SecId);
            var portfolioPapers = content.Papers.ToDictionary(x => x.Paper.SecId);
            var paperPrices = new Dictionary<string, decimal>(flattenPapers.Count);

            foreach (var paper in flattenPapers)
            {
                var price = await PortfolioService.GetPrice(paper.Value.Paper, onDate);
                paperPrices.Add(paper.Key, price);
            }

            foreach (var kv in portfolioPapers)
            {
                if (paperPrices.ContainsKey(kv.Key))
                {
                    continue;
                }

                var price = await PortfolioService.GetPrice(kv.Value.Paper, onDate);
                paperPrices.Add(kv.Key, price);
            }

            // количество бумаг к изменению
            var paperToChange = new Dictionary<string, long>();

            while (availSum > 0)
            {
                var totalVolume = availSum;
                foreach (var paperInPortfolio in content.Papers)
                {
                    totalVolume += paperInPortfolio.Count * paperPrices[paperInPortfolio.Paper.SecId];
                }

                foreach (var kv in paperToChange)
                {
                    var price = paperPrices[kv.Key];
                    totalVolume += price * kv.Value;
                }

                // K: secId, V: price, объём в портфеле с учётом цены, необходимый объём, разница в объёмах
                var balance = new Dictionary<string, Tuple<decimal, decimal, decimal, decimal>>();

                foreach (var paper in flattenPapers)
                {
                    var secId = paper.Key;
                    var price = paperPrices[secId];

                    var countInPortfolio = portfolioPapers.ContainsKey(secId) ? portfolioPapers[secId].Count : 0;
                    var changeCount = paperToChange.ContainsKey(secId) ? paperToChange[secId] : 0;
                    var count = countInPortfolio + changeCount;

                    var volume = count * price / totalVolume;
                    var needVolume = paper.Value.Volume;

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

                if (allowSell == true)
                {
                    // выбираем бумаги, от которых надо избавится, и сумма больше чем цена за одну бумагу
                    var sellPapers = balance.Where(x => x.Value.Item4 < 0 && Math.Abs(x.Value.Item4) >= x.Value.Item1);
                    foreach (var kv in sellPapers)
                    {
                        var countToSell = Convert.ToInt64(kv.Value.Item4 / kv.Value.Item1);

                        var foundPaper = flattenPapers.ContainsKey(kv.Key) ? flattenPapers[kv.Key].Paper : portfolioPapers[kv.Key].Paper;

                        var sellActions = _portfolioCalculator.SellPaper(
                            foundPaper,
                            countToSell,
                            paperPrices[kv.Key],
                            onDate ?? DateTime.Now)
                            .OfType<PortfolioMoneyAction>();

                        foreach (var action in sellActions)
                        {
                            availSum -= action.Sum;
                        }

                        if (!paperToChange.ContainsKey(kv.Key))
                        {
                            paperToChange.Add(kv.Key, countToSell);
                        }
                        else
                        {
                            paperToChange[kv.Key] += countToSell;
                        }
                    }
                }

                // берем саму разбаланисированную бумагу и пытаемся купить
                var buyPaper = balance
                    .Where(x => x.Value.Item4 > 0 && x.Value.Item1 <= availSum)
                    .OrderByDescending(x => x.Value.Item4)
                    .FirstOrDefault();

                if (null == buyPaper.Key)
                {
                    // мы ничего уже не можем купить
                    break;
                }

                var countToBuy = 1;

                var buyActions = _portfolioCalculator.BuyPaper(
                    flattenPapers[buyPaper.Key].Paper,
                    countToBuy,
                    paperPrices[buyPaper.Key],
                    onDate ?? DateTime.Now)
                    .OfType<PortfolioMoneyAction>();

                foreach(var action in buyActions)
                {
                    availSum -= action.Sum;
                }

                if (!paperToChange.ContainsKey(buyPaper.Key))
                {
                    paperToChange.Add(buyPaper.Key, countToBuy);
                }
                else
                {
                    paperToChange[buyPaper.Key] += countToBuy;
                }
            }

            foreach(var kv in paperToChange)
            {
                var paper = flattenPapers.ContainsKey(kv.Key) ? flattenPapers[kv.Key].Paper : portfolioPapers[kv.Key].Paper;
                var price = paperPrices[kv.Key];
                var count = Math.Abs(kv.Value);

                if(kv.Value > 0)
                {
                    result.AddRange(_portfolioCalculator.BuyPaper(paper, count, price));
                }
                else
                {
                    result.AddRange(_portfolioCalculator.SellPaper(paper, count, price));
                }
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