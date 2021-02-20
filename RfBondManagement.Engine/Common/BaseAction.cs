using System;

namespace RfBondManagement.Engine.Common
{
    public abstract class BaseAction<T> where T : BaseStockPaper
    {
        public DateTime Date { get; set; }
        public T Paper { get; set; }
        public long Count { get; set; }
        public decimal Price { get; set; }
        public virtual bool IsBuy { get; set; }
        public string Comment { get; set; }
    }
}