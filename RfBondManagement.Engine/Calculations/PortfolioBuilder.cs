using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using NLog;
using RfBondManagement.Engine.Interfaces;
using RfFondPortfolio.Common.Dtos;
using RfFondPortfolio.Common.Interfaces;

namespace RfBondManagement.Engine.Calculations
{
    public class PortfolioBuilder : IPortfolioBuilder
    {
        protected readonly ILogger _logger;
        protected readonly IBondCalculator _bondCalculator;
        protected readonly IPortfolioActions _portfolioActions;
        protected readonly IPaperRepository _paperRepository;
        protected readonly IHistoryRepository _historyRepository;

        public PortfolioBuilder(
            ILogger logger,
            IBondCalculator bondCalculator,
            IPortfolioActions portfolioActions,
            IPaperRepository paperRepository,
            IHistoryRepository historyRepository)
        {
            _logger = logger;
            _bondCalculator = bondCalculator;
            _portfolioActions = portfolioActions;
            _paperRepository = paperRepository;
            _historyRepository = historyRepository;
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
                Papers = new ReadOnlyCollection<IPaperInPortfolio<AbstractPaper>>(papers.Where(x => x.Value.Count > 0).Select(x => x.Value).ToList()),
                Sums = new ReadOnlyDictionary<MoneyActionType, decimal>(sums)
            };

            return content;
        }
        
        public Statistic FillStatistic(Guid portfolioId, DateTime date)
        {
            //var usdRubCourse = _historyRepository.GetNearUsdRubCourse(date);

            var statistic = new Statistic {Date = date};

            var content = Build(portfolioId);

            var papers = new List<Tuple<string, long, decimal, decimal>>();
            statistic.Sum = new Dictionary<MoneyActionType, decimal>(content.Sums);
            statistic.Papers = papers;

            foreach (var contentPaper in content.Papers)
            {
                contentPaper.MarketPrice = _historyRepository.GetNearHistoryPriceOnDate(contentPaper.Paper.SecId, date)?.ClosePrice ?? 0;
                papers.Add(new Tuple<string, long, decimal, decimal>(contentPaper.Paper.SecId, contentPaper.Count, contentPaper.AveragePrice, contentPaper.MarketPrice));
            }

            statistic.Cash = content.AvailSum;

            return statistic;
        }

        public void ExportToCsv(Guid portfolioId, IList<Statistic> statistic)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var enc1251 = Encoding.GetEncoding(1251);

            var date = DateTime.Now;
            var filename = $"{date:yyyy_MM_dd}__{date:HH_mm_ss}_statistic.csv";
            _logger.Info($"Export statistics ({statistic.Count} record(s)) to {filename} file");

            var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                NewLine = Environment.NewLine,
                Delimiter = ";",
                HasHeaderRecord = true
            };

            using (var writer = new StreamWriter(filename, false, enc1251))
            {
                using (var csv = new CsvWriter(writer, csvConfig))
                {
                    // header
                    csv.WriteField("date");
                    var secIds = statistic.SelectMany(s => s.Papers).Select(p => p.Item1).Distinct().OrderBy(p => p).ToList();
                    foreach (var secId in secIds)
                    {
                        csv.WriteField($"{secId}.count");
                        csv.WriteField($"{secId}.avgprice");
                        csv.WriteField($"{secId}.mrkprice");
                        csv.WriteField($"{secId}.profit");
                        csv.WriteField($"{secId}.value");
                    }

                    csv.WriteField("PortfolioCost");
                    csv.WriteField("PortfolioProfit");
                    csv.WriteField("Cash");

                    var moneyTypes = statistic.SelectMany(s => s.Sum).Select(m => m.Key).Distinct().ToList();
                    foreach (var moneyType in moneyTypes)
                    {
                        csv.WriteField(moneyType);
                    }
                    csv.NextRecord();

                    // records
                    foreach (var s in statistic)
                    {
                        csv.WriteField(s.Date.ToShortDateString());

                        foreach (var secId in secIds)
                        {
                            var p = s.Papers.SingleOrDefault(x => x.Item1 == secId);
                            if (null == p)
                            {
                                csv.WriteField(string.Empty);
                                csv.WriteField(string.Empty);
                                csv.WriteField(string.Empty);
                                csv.WriteField(string.Empty);
                                csv.WriteField(string.Empty);
                            }
                            else
                            {
                                csv.WriteField(p.Item2);
                                csv.WriteField(Math.Round(p.Item3, 2));
                                csv.WriteField(Math.Round(p.Item4, 2));
                                csv.WriteField(Math.Round(p.Item4 - p.Item3, 2));
                                csv.WriteField(Math.Round(p.Item4 * p.Item2, 2));
                            }
                        }

                        // учитываем только внесенные деньги
                        var incomeSum = 0m;
                        if (s.Sum.ContainsKey(MoneyActionType.IncomeExternal))
                        {
                            incomeSum = s.Sum[MoneyActionType.IncomeExternal];
                        }

                        csv.WriteField(Math.Round(s.PortfolioCost, 2));
                        csv.WriteField(0 == incomeSum
                            ? string.Empty
                            : Math.Round((s.Cash + s.PortfolioCost) / incomeSum * 100 - 100, 2).ToString());
                        csv.WriteField(Math.Round(s.Cash, 2));

                        foreach (var moneyType in moneyTypes)
                        {
                            if (!s.Sum.ContainsKey(moneyType))
                            {
                                csv.WriteField(string.Empty);
                            }
                            else
                            {
                                csv.WriteField(Math.Round(s.Sum[moneyType], 2));
                            }
                        }
                        csv.NextRecord();
                    }
                }
            }

            var actions = new List<PortfolioAction>();
            actions.AddRange(_portfolioActions.MoneyActions(portfolioId));
            actions.AddRange(_portfolioActions.PaperActions(portfolioId));
            actions = actions.OrderBy(a => a.When).ToList();
            filename = $"{date:yyyy_MM_dd}__{date:HH_mm_ss}_actions.csv";
            _logger.Info($"Export actions ({actions.Count} record(s)) to {filename} file");

            using (var writer = new StreamWriter(filename, false, enc1251))
            {
                using (var csv = new CsvWriter(writer, csvConfig))
                {
                    // headers
                    csv.WriteField("date");
                    csv.WriteField("time");
                    csv.WriteField("type");
                    csv.WriteField("secId");
                    csv.WriteField("action");
                    csv.WriteField("sum");
                    csv.WriteField("count");
                    csv.WriteField("value");
                    csv.WriteField("comment");
                    csv.NextRecord();

                    // actions
                    foreach (var action in actions)
                    {
                        var ma = action as PortfolioMoneyAction;
                        var pa = action as PortfolioPaperAction;

                        csv.WriteField(action.When.ToShortDateString());
                        csv.WriteField(action.When.ToShortTimeString());
                        csv.WriteField(ma != null ? "M" : "P");
                        csv.WriteField(action.SecId);
                        csv.WriteField(ma != null ? ma.MoneyAction.ToString() : pa.PaperAction.ToString());
                        csv.WriteField(Math.Round(action.Sum, 2));
                        csv.WriteField(pa != null ? pa.Count.ToString() : string.Empty);
                        csv.WriteField(pa != null ? Math.Round(pa.Value, 2).ToString() : string.Empty);
                        csv.WriteField(action.Comment);
                        csv.NextRecord();
                    }
                }
            }
        }
    }
}