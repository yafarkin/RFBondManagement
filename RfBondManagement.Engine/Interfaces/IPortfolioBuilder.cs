using System;
using System.Collections.Generic;
using RfFondPortfolio.Common.Dtos;
using RfFondPortfolio.Common.Interfaces;

namespace RfBondManagement.Engine.Interfaces
{
    public interface IPortfolioBuilder
    {
        IPaperInPortfolio<AbstractPaper> BuildPaperInPortfolio(AbstractPaper paper, IEnumerable<PortfolioPaperAction> allPaperActions, DateTime? onDate = null);

        PortfolioAggregatedContent Build(Guid portfolioId, DateTime? onDate = null);
    }
}