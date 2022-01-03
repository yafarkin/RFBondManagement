using System;
using System.Collections.Generic;
using RfFondPortfolio.Common.Dtos;

namespace RfBondManagement.Engine.Interfaces
{
    public interface IPortfolioCalculator
    {
        IEnumerable<PortfolioAction> PayTaxByDraftProfit(Portfolio portfolio, decimal draftSum, string comment = null, DateTime when = default(DateTime));

        IEnumerable<PortfolioAction> MoveMoney(Portfolio portfolio, decimal sum, MoneyActionType moneyActionType, string comment, string secId = null, DateTime when = default(DateTime));

        IEnumerable<PortfolioAction> BuyPaper(Portfolio portfolio, AbstractPaper paper, long count, decimal price, DateTime when = default(DateTime));

        IEnumerable<PortfolioAction> SellPaper(Portfolio portfolio, AbstractPaper paper, long count, decimal price, DateTime when = default(DateTime));

        IEnumerable<PortfolioAction> Automate(Portfolio portfolio, DateTime onDate);
    }
}