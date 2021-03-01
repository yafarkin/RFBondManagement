using System;
using LiteDB;

namespace RfBondManagement.Engine.Common
{
    public class CurrencyCourse
    {
        public Guid Id { get; set; }

        public string Currency { get; set; }
        public DateTimeOffset Date { get; set; }
        public decimal Course { get; set; }

        public string IndexCode => $"{Currency}{Date}";
    }
}