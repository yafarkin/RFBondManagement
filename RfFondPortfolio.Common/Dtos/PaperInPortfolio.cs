using System.Collections.Generic;
using System.Linq;

namespace RfFondPortfolio.Common.Dtos
{
    public class PaperInPortfolio<T> where T : AbstractPaper
    {
        public T Paper { get; set; }

        public IList<PortfolioPaperAction> Actions { get; set; }

        public long Count => Actions.Sum(a => a.Value >= 0 ? a.Count : -a.Count);

        // TODO: define & implement
        public decimal AveragePrice => Count Actions.Average()
    }
}