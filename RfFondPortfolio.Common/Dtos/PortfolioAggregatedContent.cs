using System.Collections.Generic;

namespace RfFondPortfolio.Common.Dtos
{
    public class PortfolioAggregatedContent
    {
        public IReadOnlyDictionary<MoneyActionType, decimal> Sums { get; internal set; }

        public IReadOnlyList<PaperInPortfolio<AbstractPaper>> Papers { get; internal set; }
    }
}