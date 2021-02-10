using System.Collections.Generic;
using System.Linq;

namespace RfBondManagement.Engine
{
    public abstract class BaseStockPaper
    {
        public string Name { get; set; }
        public string ISIN { get; set; }
        public string Code { get; set; }

        public List<PriceOnDate> Price { get; set; }

        public PriceOnDate LastPrice => Price?.OrderByDescending(p => p.Date).FirstOrDefault();
    }
}
