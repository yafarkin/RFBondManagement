using System;
using LiteDB;

namespace RfBondManagement.Engine.Common
{
    public class HistoryPrice : PriceOnDate
    {
        public Guid Id { get; set; }

        public string PaperCode { get; set; }

        public decimal OpenPrice { get; set; }
        public decimal HighPrice { get; set; }
        public decimal LowPrice { get; set; }
        public decimal ClosePrice { get; set; }
        public long Volume { get; set; }

        public string IndexCode => $"{PaperCode}{Date:yyyyMMdd}";
    }
}