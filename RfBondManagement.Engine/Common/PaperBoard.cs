using System;

namespace RfBondManagement.Engine.Common
{
    public class PaperBoard
    {
        public string BoardId { get; set; }

        public string Title { get; set; }

        public string Market { get; set; }

        public string Engine { get; set; }

        public bool IsTraded { get; set; }

        public DateTime HistoryFrom { get; set; }

        public DateTime HistoryTill { get; set; }

        public bool IsPrimary { get; set; }

        public string Currency { get; set; }
    }
}