using System;

namespace RfFondPortfolio.Common.Dtos
{
    public class PaperSplit : BaseSecurityEntity
    {
        public DateTime Date { get; set; }

        public decimal Multiplier { get; set; }
    }
}