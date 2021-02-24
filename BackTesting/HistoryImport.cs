using System;
using System.Globalization;
using System.IO;
using System.Linq;
using NLog;
using RfBondManagement.Engine.Common;
using RfBondManagement.Engine.Interfaces;
using Unity.Injection;

namespace BackTesting
{
    public class HistoryImport
    {
        protected const string HISTORY_FOLDER = "Historical";

        protected ILogger _logger;
        protected IHistoryDatabaseLayer _history;

        public HistoryImport(ILogger logger, IHistoryDatabaseLayer history)
        {
            _logger = logger;
            _history = history;
        }

        public void Run()
        {
            if (!Directory.Exists(HISTORY_FOLDER))
            {
                return;
            }

            var files = Directory.GetFiles(HISTORY_FOLDER, "*.txt", SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                var lastParsedDate = DateTime.MinValue;
                BaseStockPaper lastPaper = null;

                var lineCount = 0;
                var added = 0;

                var papers = _history.GetHistoryPapers();

                _logger.Info($"Import history data from {file}");
                using (var f = new StreamReader(file))
                {
                    while (true)
                    {
                        lineCount++;
                        var l = f.ReadLine();
                        if (null == l)
                        {
                            break;
                        }

                        if (0 == l.Length)
                        {
                            continue;
                        }

                        if (l[0] == '<')
                        {
                            continue;
                        }

                        var dataArr = l.Split(',', StringSplitOptions.RemoveEmptyEntries);

                        if (dataArr.Length != 9)
                        {
                            _logger.Warn($"Skip line {lineCount}, expected 9 rows, but got {dataArr.Length}");
                            continue;
                        }

                        var data = new
                        {
                            ticker = dataArr[0],
                            per = Convert.ToInt64(dataArr[1], CultureInfo.InvariantCulture),
                            open = Convert.ToDecimal(dataArr[4], CultureInfo.InvariantCulture),
                            high = Convert.ToDecimal(dataArr[5], CultureInfo.InvariantCulture),
                            low = Convert.ToDecimal(dataArr[6], CultureInfo.InvariantCulture),
                            close = Convert.ToDecimal(dataArr[7], CultureInfo.InvariantCulture),
                            vol = Convert.ToInt64(dataArr[8], CultureInfo.InvariantCulture),
                            datetime = DateTime.ParseExact(dataArr[2] + " " + dataArr[3], "yyyyMMdd HHmmss", null)
                        };

                        if (data.datetime.Year == lastParsedDate.Year && data.datetime.Month == lastParsedDate.Month &&
                            data.datetime.Day == lastParsedDate.Day)
                        {
                            continue;
                        }

                        lastParsedDate = data.datetime;

                        if (null == lastPaper || lastPaper.Code != data.ticker)
                        {
                            var paper = papers.FirstOrDefault(p => p.Code == data.ticker);
                            if (null == paper)
                            {
                                while (true)
                                {
                                    Console.Write($"Is '{data.ticker}' SHARE (1) or BOND (2)? ");
                                    var o = Console.ReadKey();
                                    Console.WriteLine();

                                    if (o.Key == ConsoleKey.D1)
                                    {
                                        paper = new BaseSharePaper
                                        {
                                            Code = data.ticker
                                        };
                                        break;
                                    }
                                    else if (o.Key == ConsoleKey.D2)
                                    {
                                        paper = new BaseBondPaper()
                                        {
                                            Code = data.ticker
                                        };
                                        break;
                                    }
                                }

                                _history.AddPaper(paper);
                            }

                            lastPaper = paper;
                        }

                        var historyPrice = new HistoryPrice
                        {
                            PaperCode =  lastPaper.Code,
                            Date = data.datetime,
                            ClosePrice = data.close,
                            HighPrice = data.high,
                            LowPrice = data.low,
                            OpenPrice = data.open,
                            Price = data.close,
                            Volume = data.vol
                        };

                        var isAdded = _history.AddHistoryPrice(historyPrice);
                        if (isAdded)
                        {
                            added++;
                        }
                    }
                }

                _logger.Info($"Lines parsed: {lineCount}, added entries: {added}");
                File.Move(file, file + ".passed");
            }
        }
    }
}