using System;
using System.Collections.Generic;
using RfBondManagement.Engine.Common;
using RfBondManagement.Engine.Interfaces;

namespace BackTesting.Interfaces
{
    public interface IBacktestEngine
    {
        void Run(Portfolio portfolio, IStrategy strategy, DateTime fromDate, ref DateTime toDate);

        Statistic FillStatistic(Portfolio portfolio, DateTime date);
        DateTime FindNearestDateWithPrices(IList<string> codes, DateTime date);

        void BuyPaper(Portfolio portfolio, DateTime date, BaseStockPaper paper, long count, IBondCalculator bondCalculator);
        void SellPaper(Portfolio portfolio, DateTime date, BaseStockPaper paper, long count, IBondCalculator bondCalculator);
    }
}