using System.Collections.Generic;

namespace RfBondManagement.Engine
{
    public class Portfolio
    {
        public string AccountNumber { get; set; }

        public List<BaseStockPaperInPortfolio> Papers { get; set; }
    }
}