using System;
using LiteDB;

namespace RfBondManagement.Engine.Common
{
    public class DividendInfo
    {
        public ObjectId DividendInfoId { get; set; }

        public string Code { get; set; }

        public DateTime T2Date { get; set; }
        public DateTime CutOffDate { get; set; }

        public decimal Dividend { get; set; }
    }
}