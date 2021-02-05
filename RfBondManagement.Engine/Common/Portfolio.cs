using System.Collections.Generic;

namespace RfBondManagement.Engine
{
    public class Portfolio
    {
        public string AccountNumber { get; set; }

        public List<BaseStockPaper> Papers { get; set; }

        public List<BaseAction> Actions { get; set; }
    }
}