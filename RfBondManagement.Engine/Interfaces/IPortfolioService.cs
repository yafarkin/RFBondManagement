using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RfBondManagement.Engine.Common;
using RfFondPortfolio.Common.Dtos;

namespace RfBondManagement.Engine.Interfaces
{
    public interface IPortfolioService
    {
        Portfolio Portfolio { get; }

        void Configure(Portfolio portfolio, ExternalImportType importType);

        void ApplyActions(IEnumerable<PortfolioAction> actions);

        Task GetPrice(PortfolioAggregatedContent portfolioAggregatedContent, DateTime? onDate = null);
        
        Task<decimal> GetPrice(AbstractPaper paper, DateTime? onDate = null);
    }
}