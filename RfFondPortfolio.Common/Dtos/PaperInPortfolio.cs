using System.Collections.Generic;

namespace RfFondPortfolio.Common.Dtos
{
    public class PaperInPortfolio<T> where T : AbstractPaper
    {
        public T Paper { get; internal set; }

        public IReadOnlyCollection<PortfolioPaperAction> Actions { get; internal set; }

        public long Count { get; internal set; }

        public decimal AveragePrice { get; internal set; }
    }
}