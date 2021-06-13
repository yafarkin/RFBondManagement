using System.Collections.Generic;
using RfFondPortfolio.Common.Dtos;

namespace RfFondPortfolio.Common.Interfaces
{
    public interface IPaperInPortfolio<out T> where T : AbstractPaper
    {
        T Paper { get; }

        IReadOnlyCollection<PortfolioPaperAction> Actions { get; set; }

        public long Count { get; set; }

        decimal AveragePrice { get; set; }

        decimal MarketPrice { get; set; }

        decimal Profit { get; }
    }
}