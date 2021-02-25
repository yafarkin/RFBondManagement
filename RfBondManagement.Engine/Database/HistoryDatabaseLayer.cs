using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using LiteDB;
using RfBondManagement.Engine.Common;
using RfBondManagement.Engine.Interfaces;

namespace RfBondManagement.Engine.Database
{
    public class HistoryDatabaseLayer : IHistoryDatabaseLayer
    {
        protected ILiteDatabase _database;

        protected ILiteCollection<BaseStockPaper> _papers;
        protected ILiteCollection<HistoryPrice> _prices;
        protected ILiteCollection<CurrencyCourse> _usdRubCourse;
        protected ILiteCollection<SplitInfo> _splits;
        protected ILiteCollection<DividendInfo> _dividends;

        protected IDictionary<DateTime, CurrencyCourse> _usdRubCourseCache;

        public HistoryDatabaseLayer()
        {
            _database = new LiteDatabase("history.db");
            _papers = _database.GetCollection<BaseStockPaper>("papers");
            _prices = _database.GetCollection<HistoryPrice>("prices");
            _usdRubCourse = _database.GetCollection<CurrencyCourse>("usdrub_course");
            _splits = _database.GetCollection<SplitInfo>("splits");
            _dividends = _database.GetCollection<DividendInfo>("dividends");

            _papers.EnsureIndex(p => p.Code);
            _prices.EnsureIndex(p => p.IndexCode);
            _usdRubCourse.EnsureIndex(p => p.Date);
            _splits.EnsureIndex(p => p.Date);
            _dividends.EnsureIndex(p => p.CutOffDate);

            _usdRubCourseCache = GetUsdRubCourses().ToDictionary(x => x.Date);
        }

        public IList<DividendInfo> GetDividendInfo()
        {
            return _dividends.FindAll().ToList();
        }

        public IList<SplitInfo> GetSplitInfo()
        {
            return _splits.FindAll().ToList();
        }

        public IList<CurrencyCourse> GetUsdRubCourses()
        {
            return _usdRubCourse.FindAll().ToList();
        }

        public IList<BaseStockPaper> GetHistoryPapers()
        {
            return _papers.FindAll().ToList();
        }

        public IList<HistoryPrice> GetHistoryPrice(string paperCode)
        {

            return _prices.Find(p => p.PaperCode == paperCode).ToList();
        }

        public HistoryPrice GetHistoryPriceOnDate(string paperCode, DateTime date)
        {
            var indexCode = $"{paperCode}{date:yyyyMMdd}";
            return _prices.FindOne(p => p.IndexCode == indexCode);
        }

        public HistoryPrice GetNearHistoryPriceOnDate(string paperCode, DateTime date)
        {
            //return _prices.FindOne(p => p.PaperCode == paperCode && p.Date >= date);

            var maxDays = 60;
            while (maxDays > 0)
            {
                var indexCode = $"{paperCode}{date:yyyyMMdd}";
                var result = _prices.FindOne(p => p.IndexCode == indexCode);
                if(null == result)
                {
                    date = date.AddDays(+1);
                    maxDays--;
                }
                else
                {
                    return result;
                }
            }

            return null;
        }

        public CurrencyCourse GetNearUsdRubCourse(DateTime date)
        {
            if (_usdRubCourseCache.ContainsKey(date))
            {
                return _usdRubCourseCache[date];
            }

            var nearDate = _usdRubCourseCache
                .Where(x => x.Key <= date)
                .Select(x => x.Key)
                .OrderByDescending(x => x)
                .FirstOrDefault();

            if (nearDate != default(DateTime))
            {
                return _usdRubCourseCache[nearDate];
            }

            return null;
        }

        public DividendInfo AddDividendInfo(string code, DateTime t2Date, DateTime cutoffDate, decimal dividend)
        {
            var d = _dividends.FindOne(p => p.CutOffDate == cutoffDate && p.Code == code);
            if (null == d)
            {
                d = new DividendInfo
                {
                    DividendInfoId = ObjectId.NewObjectId(),
                    Code = code,
                    T2Date = t2Date,
                    CutOffDate = cutoffDate,
                    Dividend = dividend,
                };

                _dividends.Insert(d);
            }
            else
            {
                d.T2Date = t2Date;
                d.Dividend = dividend;
                _dividends.Update(d);
            }

            return d;
        }

        public SplitInfo AddSplitInfo(DateTime date, string code, decimal multiplier)
        {
            var s = _splits.FindOne(p => p.Date == date && p.Code == code);
            if (null == s)
            {
                s = new SplitInfo
                {
                    SplitInfoId = ObjectId.NewObjectId(),
                    Date = date,
                    Code = code,
                    Multiplier = multiplier
                };

                _splits.Insert(s);
            }
            else
            {
                s.Multiplier = multiplier;
                _splits.Update(s);
            }

            return s;
        }

        public CurrencyCourse AddUsdRub(DateTime date, decimal course)
        {
            var c = _usdRubCourse.FindOne(p => p.Date == date);
            if (null == c)
            {
                c = new CurrencyCourse
                {
                    UsdRubCourseId = ObjectId.NewObjectId(),
                    Date = date,
                    Course = course
                };

                _usdRubCourse.Insert(c);
            }
            else
            {
                c.Course = course;
                _usdRubCourse.Update(c);
            }

            return c;
        }

        public void AddPaper(BaseStockPaper paper)
        {
            if (null == _papers.FindOne(p => p.Code == paper.Code))
            {
                paper.BaseStockPaperId = ObjectId.NewObjectId();
                _papers.Insert(paper);
            }
        }

        public bool AddHistoryPrice(HistoryPrice historyPrice)
        {
            var hp = _prices.FindOne(p => p.IndexCode == historyPrice.IndexCode);
            if (null == hp)
            {
                historyPrice.HistoryPriceId = ObjectId.NewObjectId();
                _prices.Insert(historyPrice);
                return true;
            }
            else
            {
                hp.ClosePrice = historyPrice.ClosePrice;
                hp.HighPrice = historyPrice.HighPrice;
                hp.LowPrice = historyPrice.LowPrice;
                hp.OpenPrice = historyPrice.OpenPrice;
                hp.Volume = historyPrice.Volume;
                hp.Price = historyPrice.Price;
                _prices.Update(hp);
                return false;
            }
        }
    }
}