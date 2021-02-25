using System;
using System.Collections.Generic;
using RfBondManagement.Engine.Common;

namespace RfBondManagement.Engine.Interfaces
{
    public interface IHistoryDatabaseLayer
    {
        IList<DividendInfo> GetDividendInfo();
        IList<SplitInfo> GetSplitInfo();
        IList<CurrencyCourse> GetUsdRubCourses();
        IList<BaseStockPaper> GetHistoryPapers();
        IList<HistoryPrice> GetHistoryPrice(string paperCode);
        HistoryPrice GetHistoryPriceOnDate(string paperCode, DateTime date);
        HistoryPrice GetNearHistoryPriceOnDate(string paperCode, DateTime date);
        CurrencyCourse GetNearUsdRubCourse(DateTime date);

        DividendInfo AddDividendInfo(string code, DateTime t2Date, DateTime cutoffDate, decimal dividend);
        SplitInfo AddSplitInfo(DateTime date, string code, decimal multiplier);
        CurrencyCourse AddUsdRub(DateTime date, decimal course);
        void AddPaper(BaseStockPaper paper);
        bool AddHistoryPrice(HistoryPrice historyPrice);

    }
}