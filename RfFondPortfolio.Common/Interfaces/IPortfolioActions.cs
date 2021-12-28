using System;
using System.Collections.Generic;
using RfFondPortfolio.Common.Dtos;

namespace RfFondPortfolio.Common.Interfaces
{
    public interface IPortfolioActions
    {
        IEnumerable<PortfolioMoneyAction> MoneyActions(Guid portfolioId);

        IEnumerable<PortfolioPaperAction> PaperActions(Guid portfolioId);
    }
}