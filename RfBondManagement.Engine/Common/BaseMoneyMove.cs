using System;

namespace RfBondManagement.Engine.Common
{
    public class BaseMoneyMove
    {
        public MoneyMoveType MoneyMoveType { get; set; }
        public DateTime Date { get; set; }
        public decimal Sum { get; set; }
        public string Comment { get; set; }
    }
}