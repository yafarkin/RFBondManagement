using System;
using System.Collections.Generic;
using RfBondManagement.Engine.Common;

namespace RfBondManagement.Engine.Interfaces
{
    public interface IHistoryDatabaseLayer
    {
        IList<BaseStockPaper> GetHistoryPapers();
        IList<HistoryPrice> GetHistoryPrice(string paperCode);
        HistoryPrice GetHistoryPriceOnDate(string paperCode, DateTime date);

        void AddPaper(BaseStockPaper paper);
        bool AddHistoryPrice(HistoryPrice historyPrice);

    }
}