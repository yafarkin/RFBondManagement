using System;
using System.Collections.Generic;
using RfBondManagement.Engine.Calculations;
using RfFondPortfolio.Common.Dtos;

namespace BackTesting.Interfaces
{
    public interface IBacktestEngine
    {
        PortfolioEngine PortfolioEngine { get; }

        void Run(IStrategy strategy, DateTime fromDate, ref DateTime toDate);

        Statistic FillStatistic(DateTime date);
        DateTime FindNearestDateWithPrices(IList<string> codes, DateTime date);

        void BuyPaper(DateTime date, AbstractPaper paper, long count);
        void SellPaper(DateTime date, AbstractPaper paper, long count);
    }
}