using System;

namespace RfFondPortfolio.Common.Dtos
{
    /// <summary>
    /// Амортизация по облигации
    /// </summary>
    public class BondAmortization
    {
        public decimal IssueValue { get; set; }
        public DateTime AmortDate { get; set; }
        public decimal FaceValue { get; set; }
        public decimal InitialFaceValue { get; set; }
        public string FaceUnit { get; set; }
        public decimal ValuePercent { get; set; }
        public decimal Value { get; set; }
        public decimal ValueRub { get; set; }
        public string DataSource { get; set; }
    }
}