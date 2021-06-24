using System;

namespace RfFondPortfolio.Common.Dtos
{
    public class FifoAction : Tuple<PortfolioPaperAction, PortfolioPaperAction, long>
    {
        public PortfolioPaperAction BuyAction => Item1;
        public PortfolioPaperAction SellAction => Item2;
        public long Count => Item3;

        public FifoAction(PortfolioPaperAction item1, PortfolioPaperAction item2, long item3)
            : base(item1, item2, item3)
        {
        }
    }
}