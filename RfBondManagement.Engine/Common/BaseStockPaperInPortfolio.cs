﻿using System.Collections.Generic;
using System.Linq;

namespace RfBondManagement.Engine
{
    public class BaseStockPaperInPortfolio
    {
        public BaseStockPaper Paper { get; set; }

        public long Count => Actions.Sum(a => a.IsBuy ? a.Count : -a.Count);

        public decimal AvgBuySum => Actions.Where(a => a.IsBuy).Sum(a => a.Count * a.Price) /
                                    Actions.Where(a => a.IsBuy).Sum(a => a.Count);

        public List<BaseAction> Actions { get; set; }
    }
}