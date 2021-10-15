using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using BackTesting.Interfaces;
using CsvHelper;
using CsvHelper.Configuration;
using NLog;
using RfBondManagement.Engine.Calculations;
using RfBondManagement.Engine.Common;
using RfFondPortfolio.Common.Dtos;
using RfFondPortfolio.Common.Interfaces;

namespace BackTesting
{
    public class BacktestEngine : IBacktestEngine
    {
        protected ILogger _logger;

        protected PortfolioEngine _portfolioEngine;

        protected IHistoryRepository _historyRepository;
        protected IPortfolioPaperActionRepository _paperActionRepository;
        protected IPortfolioMoneyActionRepository _moneyActionRepository;

        public BacktestEngine(
            ILogger logger,
            PortfolioEngine portfolioEngine,
            Portfolio portfolio,
            ExternalImportType importType,
            IHistoryRepository historyRepository,
            IPortfolioPaperActionRepository paperActionRepository,
            IPortfolioMoneyActionRepository moneyActionRepository)
        {
            _logger = logger;
            _portfolioEngine = portfolioEngine;
            _historyRepository = historyRepository;

            _portfolioEngine.Configure(portfolio, importType);

            _paperActionRepository = paperActionRepository;
            _moneyActionRepository = moneyActionRepository;

            _paperActionRepository.Setup(portfolioEngine.Portfolio.Id);
            _moneyActionRepository.Setup(portfolioEngine.Portfolio.Id);
        }

        public Statistic FillStatistic(DateTime date)
        {
            //var usdRubCourse = _historyRepository.GetNearUsdRubCourse(date);

            var statistic = new Statistic {Date = date};

            var content = _portfolioEngine.Build();

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

        public PortfolioEngine PortfolioEngine => _portfolioEngine;

        public void ExportToCsv(IList<Statistic> statistic)
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
            actions.AddRange(_moneyActionRepository.Get());
            actions.AddRange(_paperActionRepository.Get());
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

        public void Run(IStrategy strategy, DateTime fromDate, ref DateTime toDate)
        {
            var statistics = new List<Statistic>();

            _logger.Info($"Start backtesting strategy '{strategy.Description}' from {fromDate} to {toDate}");

            var date = FindNearestDateWithPrices(strategy.Papers.ToList(), fromDate);
            if (date != fromDate)
            {
                _logger.Warn($"Shifted start date {fromDate} to nearest date with prices {date}");
            }

            strategy.Init(this, _portfolioEngine.Portfolio, date);

            if (date != fromDate)
            {
                _logger.Info($"Start date shifted from {fromDate} to {date}");
            }

            statistics.Add(FillStatistic(date));

            while (date <= toDate)
            {
                if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                {
                    date = date.AddDays(1);
                    continue;
                }

                var nextProcessDate = FindNearestDateWithPrices(strategy.Papers.ToList(), date);
                if (nextProcessDate != date)
                {
                    _logger.Warn($"Shifted current process date {date} to nearest date with prices {nextProcessDate}");
                }

                do
                {
                    var actions = _portfolioEngine.Automate(date);
                    foreach (var action in actions)
                    {
                        if (action is PortfolioPaperAction p)
                        {
                            _paperActionRepository.Insert(p);
                        }
                        else if (action is PortfolioMoneyAction m)
                        {
                            _moneyActionRepository.Insert(m);
                        }
                        else
                        {
                            throw new ApplicationException($"Неизвестный тип действия: {action.GetType()}");
                        }
                    }

                    if (date != nextProcessDate)
                    {
                        date = date.AddDays(1);
                    }
                    else
                    {
                        break;
                    }
                } while (true);

                var result = strategy.Process(date);

                statistics.Add(FillStatistic(date));

                if (!result)
                {
                    break;
                }

                date = date.AddDays(1);
            }

            _logger.Info($"Complete at {date}");

            ExportToCsv(statistics);
        }

        public void BuyPaper(DateTime date, AbstractPaper paper, long count)
        {
            var priceEntity = _historyRepository.GetHistoryPriceOnDate(paper.SecId, date);
            var price = priceEntity?.ClosePrice ?? 0;
            if (0 == price)
            {
                throw new ApplicationException($"Нет цены для {paper.SecId} на {date}");
            }

            _portfolioEngine.BuyPaper(paper, count, price, date);
        }

        public void SellPaper(DateTime date, AbstractPaper paper, long count)
        {
            var priceEntity = _historyRepository.GetHistoryPriceOnDate(paper.SecId, date);
            var price = priceEntity?.ClosePrice ?? 0;
            if (0 == price)
            {
                throw new ApplicationException($"Нет цены для {paper.SecId} на {date}");
            }

            _portfolioEngine.SellPaper(paper, count, price, date);
        }

        public virtual DateTime FindNearestDateWithPrices(IList<string> codes, DateTime date)
        {
            var nearDate = date;
            for (var i = 0; i < codes.Count; i++)
            {
                var code = codes[i];

                var paperPrice = _historyRepository.GetHistoryPriceOnDate(code, nearDate);
                if (null == paperPrice)
                {
                    paperPrice = _historyRepository.GetNearHistoryPriceOnDate(code, nearDate);
                    if (null == paperPrice)
                    {
                        throw new ApplicationException($"Can't find paper price '{code}' after {date}");
                    }
                }

                if (paperPrice.When > nearDate)
                {
                    _logger.Warn($"Paper {code} shift date from {nearDate} to {paperPrice.When}");
                    nearDate = paperPrice.When;
                    i = -1;
                }
            }

            return nearDate;
        }
    }
}