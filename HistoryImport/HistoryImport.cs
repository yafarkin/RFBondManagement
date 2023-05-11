using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using NLog;
using RfBondManagement.Engine.Common;
using RfBondManagement.Engine.Interfaces;
using RfFondPortfolio.Common.Dtos;
using Unity.Injection;

namespace HistoryImport
{
    public class HistoryImport
    {
        protected const string HISTORY_FOLDER = "Historical";
        protected const string USDRUB_FILE = "usdrub.csv";
        protected const string SPLIT_FILE = "split.csv";
        protected const string DIVIDENDS_FILE = "dividends.csv";

        public static readonly DateTime MinDate = new DateTime(2000, 1, 1);

        protected ILogger _logger;
        protected IHistoryDatabaseLayer _history;

        protected bool? _allShare;

        public HistoryImport(ILogger logger, IHistoryDatabaseLayer history)
        {
            _logger = logger;
            _history = history;
        }

        public void RunOnline()
        {
            ImportCourseCbrf();
        }

        private void ImportCourseCbrf()
        {
            _logger.Info("Import from CbRf");

            var usdCourse = _history.GetCourses("usd").ToDictionary(x => x.Date);

            var imported = 0;

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var serializer = new XmlSerializer(typeof(ValCurs));

            var dateReq1 = MinDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            var dateReq2 = DateTime.Today.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            var url = $"http://www.cbr.ru/scripts/XML_dynamic.asp?date_req1={dateReq1}&date_req2={dateReq2}&VAL_NM_RQ=R01235";
            var xdoc = XDocument.Load(url);
            using (var reader = xdoc.CreateReader())
            {
                var course = serializer.Deserialize(reader) as ValCurs;
                if (null == course?.Record || 0 == course.Record.Length)
                {
                    _logger.Error("Fail to retrieve course");
                }
                else
                {
                    foreach (var valCursRecord in course.Record)
                    {
                        var importDate = DateTimeOffset.Parse(valCursRecord.Date);
                        importDate = importDate.ToUniversalTime().ToLocalTime();
                        var courseValue = decimal.Parse(valCursRecord.Value) / valCursRecord.Nominal;

                        _history.AddCurrencyCourse("usd", importDate, courseValue);
                        imported++;
                    }
                }
            }

            _logger.Info($"Imported {imported} record(s)");

            var usdCourse2 = _history.GetCourses("usd").ToDictionary(x => x.Date);
        }

        public void RunLocally()
        {
            if (!Directory.Exists(HISTORY_FOLDER))
            {
                return;
            }

            _allShare = null;
            ImportSplit();
            ImportHistoricalPrice();
            ImportUsdRubCourse();
            ImportDividends();
        }

        private void ImportDividends()
        {
            var file = Path.Combine(HISTORY_FOLDER, DIVIDENDS_FILE);

            if (!File.Exists(file))
            {
                return;
            }

            _logger.Info("Import DIVIDENDS info");

            var dividends = _history.GetDividendInfo().ToDictionary(x => x.CutOffDate + x.Code);
            using (var f = new StreamReader(file))
            {
                var lineCount = 0;
                var added = 0;

                while (true)
                {
                    lineCount++;
                    var l = f.ReadLine();
                    if (null == l)
                    {
                        break;
                    }

                    if (lineCount == 1)
                    {
                        continue;
                    }

                    var dataArr = l.Split(';', StringSplitOptions.RemoveEmptyEntries);
                    if (dataArr.Length != 4)
                    {
                        _logger.Warn($"Skip line {lineCount}, expected 4 rows, but got {dataArr.Length}");
                        continue;
                    }

                    var data = new
                    {
                        code = dataArr[0],
                        dateT2 = DateTime.ParseExact(dataArr[1], "yyyyMMdd", null),
                        dateCutOff = DateTime.ParseExact(dataArr[2], "yyyyMMdd", null),
                        dividend = Convert.ToDecimal(dataArr[3], CultureInfo.InvariantCulture)
                    };

                    if (!dividends.ContainsKey(data.dateCutOff + data.code))
                    {
                        var d = _history.AddDividendInfo(data.code, data.dateT2, data.dateCutOff, data.dividend);
                        dividends.Add(data.dateCutOff + data.code, d);
                        added++;
                    }
                }

                _logger.Info($"Lines parsed: {lineCount}, added entries: {added}");
            }

            File.Move(file, file + ".passed");
        }

        protected void ImportSplit()
        {
            var file = Path.Combine(HISTORY_FOLDER, SPLIT_FILE);

            if (!File.Exists(file))
            {
                return;
            }

            _logger.Info("Import SPLIT info");

            var splits = _history.GetSplitInfo().ToDictionary(x => x.Date + x.Code);
            using (var f = new StreamReader(file))
            {
                var lineCount = 0;
                var added = 0;

                while (true)
                {
                    lineCount++;
                    var l = f.ReadLine();
                    if (null == l)
                    {
                        break;
                    }

                    if (lineCount == 1)
                    {
                        continue;
                    }

                    var dataArr = l.Split(';', StringSplitOptions.RemoveEmptyEntries);
                    if (dataArr.Length != 3)
                    {
                        _logger.Warn($"Skip line {lineCount}, expected 3 rows, but got {dataArr.Length}");
                        continue;
                    }

                    var data = new
                    {
                        date = DateTime.ParseExact(dataArr[0], "yyyyMMdd", null),
                        code = dataArr[1],
                        multiplier = Convert.ToDecimal(dataArr[2], CultureInfo.InvariantCulture)
                    };

                    if (!splits.ContainsKey(data.date.ToString()+data.code))
                    {
                        var s = _history.AddSplitInfo(data.date, data.code, data.multiplier);
                        splits.Add(data.date.ToString()+data.code, s);
                        added++;
                    }
                }

                _logger.Info($"Lines parsed: {lineCount}, added entries: {added}");
            }

            File.Move(file, file + ".passed");
        }

        protected void ImportUsdRubCourse()
        {
            var file = Path.Combine(HISTORY_FOLDER, USDRUB_FILE);

            if (!File.Exists(file))
            {
                return;
            }

            _logger.Info("Import USDRUB course");

            var courses = _history.GetCourses("usd").ToDictionary(x => x.Date);

            using (var f = new StreamReader(file))
            {
                var lineCount = 0;
                var added = 0;

                while (true)
                {
                    lineCount++;
                    var l = f.ReadLine();
                    if (null == l)
                    {
                        break;
                    }

                    if (lineCount == 1)
                    {
                        continue;
                    }

                    var dataArr = l.Split(';', StringSplitOptions.RemoveEmptyEntries);
                    if (dataArr.Length != 2)
                    {
                        _logger.Warn($"Skip line {lineCount}, expected 2 rows, but got {dataArr.Length}");
                        continue;
                    }

                    var data = new
                    {
                        date = DateTime.ParseExact(dataArr[0], "dd.MM.yyyy H:mm:ss", null),
                        course = Convert.ToDecimal(dataArr[1])
                    };

                    if (!courses.ContainsKey(data.date))
                    {
                        var c = _history.AddCurrencyCourse("usd", data.date, data.course);
                        courses.Add(data.date, c);
                        added++;
                    }
                }

                _logger.Info($"Lines parsed: {lineCount}, added entries: {added}");
            }

            File.Move(file, file + ".passed");
        }

        protected void ImportHistoricalPrice()
        {
            var files = Directory.GetFiles(HISTORY_FOLDER, "*.txt", SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                var lastParsedDate = DateTime.MinValue;
                BaseStockPaper lastPaper = null;

                var lineCount = 0;
                var added = 0;

                var papers = _history.GetHistoryPapers();

                _logger.Info($"Import history data from {file}");
                ImportFromHistoricalFile(file, ref lineCount, lastParsedDate, lastPaper, papers, ref added);

                _logger.Info($"Lines parsed: {lineCount}, added entries: {added}");
                File.Move(file, file + ".passed");
            }
        }

        protected void ImportFromHistoricalFile(string file, ref int lineCount, DateTime lastParsedDate, BaseStockPaper lastPaper, IList<BaseStockPaper> papers, ref int added)
        {
            var lastPrice = 0m;
            var splits = _history.GetSplitInfo().ToDictionary(s => s.Date + s.Code);

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

                    if (data.datetime.Year == lastParsedDate.Year &&
                        data.datetime.Month == lastParsedDate.Month &&
                        data.datetime.Day == lastParsedDate.Day)
                    {
                        continue;
                    }

                    lastParsedDate = data.datetime;

                    if (null == lastPaper || lastPaper.SecId != data.ticker)
                    {
                        var paper = papers.FirstOrDefault(p => p.SecId == data.ticker);
                        if (null == paper)
                        {
                            if (_allShare.HasValue)
                            {
                                if (_allShare == true)
                                {
                                    paper = new BaseStockPaper
                                    {
                                        SecId = data.ticker
                                    };
                                }
                                else
                                {
                                    paper = new BaseStockPaper
                                    {
                                        SecId = data.ticker
                                    };
                                }
                            }
                            else
                            {
                                while (true)
                                {
                                    Console.Write($"Is '{data.ticker}' SHARE (1) or BOND (2) or all is SHARE (3), or all is BOND (4)? ");
                                    var o = Console.ReadKey();
                                    Console.WriteLine();

                                    if (o.Key == ConsoleKey.D1 || o.Key == ConsoleKey.D3)
                                    {
                                        paper = new BaseStockPaper
                                        {
                                            SecId = data.ticker
                                        };

                                        if (o.Key == ConsoleKey.D3)
                                        {
                                            _allShare = true;
                                        }

                                        break;
                                    }
                                    else if (o.Key == ConsoleKey.D2 || o.Key == ConsoleKey.D4)
                                    {
                                        paper = new BaseStockPaper
                                        {
                                            SecId = data.ticker
                                        };

                                        if (o.Key == ConsoleKey.D3)
                                        {
                                            _allShare = false;
                                        }

                                        break;
                                    }
                                }
                            }

                            _history.AddPaper(paper);
                        }

                        lastPaper = paper;
                    }

                    var historyPrice = new HistoryPrice
                    {
                        PaperCode = lastPaper.SecId,
                        Date = data.datetime.Date,
                        ClosePrice = data.close,
                        HighPrice = data.high,
                        LowPrice = data.low,
                        OpenPrice = data.open,
                        Price = data.close,
                        Volume = data.vol
                    };

                    if (lastPrice > 0)
                    {
                        var priceChange = lastPrice / historyPrice.Price;
                        if (priceChange >= 8 || priceChange <= 0.2m)
                        {
                            if (!splits.ContainsKey(historyPrice.Date + historyPrice.PaperCode))
                            {
                                throw new ApplicationException($"Can't find split info for {historyPrice.PaperCode} at {historyPrice.Date}!");
                            }
                        }
                    }

                    lastPrice = historyPrice.Price;

                    var isAdded = _history.AddHistoryPrice(historyPrice);
                    if (isAdded)
                    {
                        added++;
                    }
                }
            }
        }
    }
}