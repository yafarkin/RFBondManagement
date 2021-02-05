using System;

namespace RfBondManagement.Engine
{
    public abstract class BaseAction
    {
        public DateTime Date { get; set; }
        public BaseStockPaper Paper { get; set; }
        public long Count { get; set; }
        public decimal Price { get; set; }
        public bool IsBuy { get; set; }
        public string Comment { get; set; }
    }
}