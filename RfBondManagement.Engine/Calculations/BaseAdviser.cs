using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog;
using RfBondManagement.Engine.Common;
using RfBondManagement.Engine.Interfaces;
using RfFondPortfolio.Common.Dtos;
using RfFondPortfolio.Common.Interfaces;

namespace RfBondManagement.Engine.Calculations
{
    public abstract class BaseAdviser : IAdviser
    {
        protected readonly ILogger _logger;
        protected readonly IPortfolioBuilder _portfolioBuilder;
        protected readonly IPortfolioService _portfolioService;
        protected readonly IPortfolioCalculator _portfolioCalculator;

        /// <summary>
        /// Цены на бумаги
        /// </summary>
        protected IDictionary<string, decimal> _paperPrices;

        /// <summary>
        /// Количество бумаг к изменению
        /// </summary>
        protected IDictionary<string, long> _paperToChange;

        /// <summary>
        /// Схлопнутый список бумаг из структуры, с учётом их весов
        /// </summary>
        protected Dictionary<string, PortfolioStructureLeafPaper> _flattenPapers;

        /// <summary>
        /// Список бумаг из портфеля
        /// </summary>
        protected Dictionary<string, IPaperInPortfolio<AbstractPaper>> _portfolioPapers;

        /// <summary>
        /// Агрегированное содержимое портфеля
        /// </summary>
        protected PortfolioAggregatedContent _content;

        protected BaseAdviser(ILogger logger, IPortfolioBuilder portfolioBuilder, IPortfolioCalculator portfolioCalculator, IPortfolioService portfolioService)
        {
            _logger = logger;

            _portfolioBuilder = portfolioBuilder;
            _portfolioCalculator = portfolioCalculator;
            _portfolioService = portfolioService;
        }

        protected decimal? GetAsDecimal(IDictionary<string, string> p, string key, decimal? defaultValue = null)
        {
            if (!p.ContainsKey(key))
            {
                return defaultValue;
            }

            return Convert.ToDecimal(p[key]);
        }

        protected bool? GetAsBool(IDictionary<string, string> p, string key, bool? defaultValue = null)
        {
            if (!p.ContainsKey(key))
            {
                return defaultValue;
            }

            return Convert.ToBoolean(p[key]);
        }

        protected DateTime? GetAsDateTime(IDictionary<string, string> p, string key, DateTime? defaultValue = null)
        {
            if (!p.ContainsKey(key))
            {
                return defaultValue;
            }

            return Convert.ToDateTime(p[key]);
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

        public async Task<IDictionary<string, decimal>> FillPaperPrices(DateTime? onDate = null)
        {
            var papers = _flattenPapers.Select(x => x.Value.Paper).Concat(_content.Papers.Select(x => x.Paper));

            var result = new Dictionary<string, decimal>();

            foreach (var paper in papers)
            {
                if (result.ContainsKey(paper.SecId))
                {
                    continue;
                }

                var price = await _portfolioService.GetPrice(paper, onDate);
                result.Add(paper.SecId, price);
            }

            return result;
        }

        public decimal CalcTotalVolume()
        {
            var totalVolume = 0m;

            foreach (var paperInPortfolio in _content.Papers)
            {
                totalVolume += paperInPortfolio.Count * _paperPrices[paperInPortfolio.Paper.SecId];
            }

            foreach (var kv in _paperToChange)
            {
                var price = _paperPrices[kv.Key];
                totalVolume += price * kv.Value;
            }

            return totalVolume;
        }

        public Dictionary<string, BalanceInfo> CalcBalance(decimal totalVolume)
        {
            var balance = new Dictionary<string, BalanceInfo>();

            foreach (var paper in _flattenPapers)
            {
                var secId = paper.Key;
                var price = _paperPrices[secId];

                var countInPortfolio = _portfolioPapers.ContainsKey(secId) ? _portfolioPapers[secId].Count : 0;
                var changeCount = _paperToChange.ContainsKey(secId) ? _paperToChange[secId] : 0;
                var count = countInPortfolio + changeCount;

                var volume = count * price / totalVolume;
                var needVolume = paper.Value.Volume;

                balance.Add(secId, new BalanceInfo(price, volume, needVolume));
            }

            foreach (var paper in _portfolioPapers)
            {
                if (balance.ContainsKey(paper.Key))
                {
                    continue;
                }

                var secId = paper.Key;
                var price = _paperPrices[secId];

                var countInPortfolio = _portfolioPapers[secId].Count;
                var changeCount = _paperToChange.ContainsKey(secId) ? _paperToChange[secId] : 0;
                var count = countInPortfolio + changeCount;

                var volume = count * price / totalVolume;
                var needVolume = 0;

                balance.Add(secId, new BalanceInfo(price, volume, needVolume));
            }

            return balance;
        }

        protected async Task Prepare(Portfolio portfolio, ExternalImportType importType, DateTime? onDate)
        {
            _portfolioCalculator.Configure(portfolio);
            _portfolioService.Configure(portfolio, importType);

            _content = _portfolioBuilder.Build(portfolio.Id);
            _flattenPapers = FlattenPaperStructure(portfolio.RootLeaf, 1).ToDictionary(x => x.Paper.SecId);
            _portfolioPapers = _content.Papers.ToDictionary(x => x.Paper.SecId);
            _paperPrices = await FillPaperPrices(onDate);
            _paperToChange = new Dictionary<string, long>();
        }

        protected IEnumerable<PortfolioAction> ChangeCount(AbstractPaper paper, long count, DateTime? onDate = null)
        {
            IEnumerable<PortfolioAction> actions;

            if (count > 0)
            {
                actions = _portfolioCalculator.BuyPaper(
                        paper,
                        count,
                        _paperPrices[paper.SecId],
                        onDate ?? DateTime.UtcNow)
                    .OfType<PortfolioMoneyAction>();
            }
            else
            {
                actions = _portfolioCalculator.SellPaper(
                        paper,
                        -count,
                        _paperPrices[paper.SecId],
                        onDate ?? DateTime.UtcNow)
                    .OfType<PortfolioMoneyAction>();
            }

            if (!_paperToChange.ContainsKey(paper.SecId))
            {
                _paperToChange.Add(paper.SecId, count);
            }
            else
            {
                _paperToChange[paper.SecId] += count;
            }

            return actions;
        }

        protected IEnumerable<PortfolioAction> Finish(DateTime? onDate)
        {
            var result = new List<PortfolioAction>();

            foreach (var kv in _paperToChange)
            {
                var paper = _flattenPapers.ContainsKey(kv.Key) ? _flattenPapers[kv.Key].Paper : _portfolioPapers[kv.Key].Paper;
                var price = _paperPrices[kv.Key];
                var count = Math.Abs(kv.Value);

                if (kv.Value > 0)
                {
                    result.AddRange(_portfolioCalculator.BuyPaper(paper, count, price, onDate ?? DateTime.UtcNow));
                }
                else
                {
                    result.AddRange(_portfolioCalculator.SellPaper(paper, count, price, onDate ?? DateTime.UtcNow));
                }
            }

            return result;
        }

        public abstract Task<IEnumerable<PortfolioAction>> Advise(Portfolio portfolio, ExternalImportType importType, IDictionary<string, string> p);
    }
}