using System;
using LiteDB;

namespace RfBondManagement.Engine.Common
{
    public class DividendInfo
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public DateTime T2Date { get; set; }
        public DateTime CutOffDate { get; set; }

        public decimal Dividend { get; set; }
    }
}