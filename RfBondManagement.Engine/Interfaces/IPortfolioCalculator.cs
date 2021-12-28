using System;
using System.Collections.Generic;
using RfFondPortfolio.Common.Dtos;

namespace RfBondManagement.Engine.Interfaces
{
    public interface IPortfolioCalculator
    {
        Portfolio Portfolio { get; }

        void Configure(Portfolio portfolio);

        IEnumerable<PortfolioAction> PayTaxByDraftProfit(decimal draftSum, string comment = null, DateTime when = default(DateTime));

        IEnumerable<PortfolioAction> MoveMoney(decimal sum, MoneyActionType moneyActionType, string comment, string secId = null, DateTime when = default(DateTime));

        IEnumerable<PortfolioAction> BuyPaper(AbstractPaper paper, long count, decimal price, DateTime when = default(DateTime));

        IEnumerable<PortfolioAction> SellPaper(AbstractPaper paper, long count, decimal price, DateTime when = default(DateTime));

        IEnumerable<PortfolioAction> Automate(DateTime onDate);
    }
}