using System;

namespace RfBondManagement.Engine.Common
{
    public abstract class BaseAction
    {
        public DateTime Date { get; set; }
        public BaseStockPaper Paper { get; set; }
        public long Count { get; set; }
        public decimal Price { get; set; }
        public virtual bool IsBuy { get; set; }
        public string Comment { get; set; }
    }
}