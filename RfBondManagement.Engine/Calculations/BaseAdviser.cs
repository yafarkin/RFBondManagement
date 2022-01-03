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
        protected readonly IPortfolioBuilder _portfolioBuilder;
        protected readonly IPortfolioCalculator _portfolioCalculator;
        protected readonly IPaperRepository _paperRepository;

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

        protected BaseAdviser(IPortfolioBuilder portfolioBuilder, IPortfolioCalculator portfolioCalculator, IPaperRepository paperRepository)
        {
            _portfolioBuilder = portfolioBuilder;
            _portfolioCalculator = portfolioCalculator;
            _paperRepository = paperRepository;
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
                        SecId = paper.SecId,
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

        public async Task<IDictionary<string, decimal>> FillPaperPrices(IPortfolioService portfolioService, DateTime? onDate = null)
        {
            var papers = _flattenPapers.Select(x => x.Value.SecId).Concat(_content.Papers.Select(x => x.Paper.SecId));

            var result = new Dictionary<string, decimal>();

            foreach (var secId in papers)
            {
                if (result.ContainsKey(secId))
                {
                    continue;
                }

                var paper = _paperRepository.Get(secId);
                var price = await portfolioService.GetPrice(paper, onDate);
                result.Add(secId, price);
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

        protected async Task Prepare(IPortfolioService portfolioService, DateTime? onDate)
        {
            _content = _portfolioBuilder.Build(portfolioService.Portfolio.Id);
            _flattenPapers = FlattenPaperStructure(portfolioService.Portfolio.RootLeaf, 1).ToDictionary(x => x.SecId);
            _portfolioPapers = _content.Papers.ToDictionary(x => x.Paper.SecId);
            _paperPrices = await FillPaperPrices(portfolioService, onDate);
            _paperToChange = new Dictionary<string, long>();
        }

        protected IEnumerable<PortfolioAction> ChangeCount(Portfolio portfolio, string secId, long count, DateTime? onDate = null)
        {
            IEnumerable<PortfolioAction> actions;

            var paper = _paperRepository.Get(secId);

            if (count > 0)
            {
                actions = _portfolioCalculator.BuyPaper(
                        portfolio,
                        paper,
                        count,
                        _paperPrices[secId],
                        onDate ?? DateTime.UtcNow)
                    .OfType<PortfolioMoneyAction>();
            }
            else
            {
                actions = _portfolioCalculator.SellPaper(
                        portfolio,
                        paper,
                        -count,
                        _paperPrices[secId],
                        onDate ?? DateTime.UtcNow)
                    .OfType<PortfolioMoneyAction>();
            }

            if (!_paperToChange.ContainsKey(paper.SecId))
            {
                _paperToChange.Add(secId, count);
            }
            else
            {
                _paperToChange[secId] += count;
            }

            return actions;
        }

        protected IEnumerable<PortfolioAction> Finish(Portfolio portfolio, DateTime? onDate)
        {
            var result = new List<PortfolioAction>();

            foreach (var kv in _paperToChange)
            {
                var secId = _flattenPapers.ContainsKey(kv.Key) ? _flattenPapers[kv.Key].SecId : _portfolioPapers[kv.Key].Paper.SecId;
                var price = _paperPrices[kv.Key];
                var count = Math.Abs(kv.Value);

                var paper = _paperRepository.Get(secId);

                result.AddRange(kv.Value > 0
                    ? _portfolioCalculator.BuyPaper(portfolio, paper, count, price, onDate ?? DateTime.UtcNow)
                    : _portfolioCalculator.SellPaper(portfolio, paper, count, price, onDate ?? DateTime.UtcNow));
            }

            return result;
        }

        public abstract Task<IEnumerable<PortfolioAction>> Advise(IPortfolioService portfolioService, IDictionary<string, string> p);
    }
}