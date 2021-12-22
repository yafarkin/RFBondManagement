using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog;
using RfFondPortfolio.Common.Dtos;
using RfFondPortfolio.Common.Interfaces;

namespace RfBondManagement.Engine.Calculations
{
    public class BuyAdviser
    {
        public static async Task<IEnumerable<PortfolioAction>> Advise(ILogger logger, PortfolioEngine engine, long availSum, bool useVa, DateTime? onDate, IExternalImport import, IHistoryRepository historyRepository)
        {
            var result = new List<PortfolioAction>();

            var portfolio = engine.Portfolio;
            var rootLeaf = portfolio.RootLeaf;
            var flattenPapers = FlattenPaperStructure(rootLeaf, 1);
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

            // количество бумаг к изменению
            var paperToChange = new Dictionary<string, long>();

            throw new NotImplementedException();

            var content = engine.Build();

            while (availSum > 0)
            {
                // secId, price, объём в портфеле с учётом цены, текущее количество, количество к покупке
                var papersList = new List<Tuple<string, decimal, decimal, long, long>>();

                foreach (var paperValue in flattenPapers)
                {
                    var secId = paperValue.Paper.SecId;
                    var price = paperPrices[secId];

                    var countInPortfolio = content.Papers.FirstOrDefault(x => x.Paper.SecId == secId)?.Count ?? 0;
                    var changeCount = paperToChange.ContainsKey(secId) ? paperToChange[secId] : 0;


                    //papersList.Add(new Tuple<string, decimal, decimal, long, long>());
                }

                var maxDisablanced = FindMaxDisablancedPaper(portfolio, content, rootLeaf);

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

        private static PortfolioStructureLeafPaper FindMaxDisablancedPaper(Portfolio portfolio, PortfolioAggregatedContent content, PortfolioStructureLeaf entryLeaf)
        {
            throw new NotImplementedException();
        }
    }
}