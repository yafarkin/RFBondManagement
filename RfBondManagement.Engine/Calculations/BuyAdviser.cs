using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog;
using RfBondManagement.Engine.Common;
using RfBondManagement.Engine.Interfaces;
using RfFondPortfolio.Common.Dtos;
using RfFondPortfolio.Common.Interfaces;

namespace RfBondManagement.Engine.Calculations
{
    public class BuyAdviser : BaseAdviser
    {
        public BuyAdviser(ILogger logger, IPortfolioBuilder portfolioBuilder, IPortfolioCalculator portfolioCalculator, IPortfolioService portfolioService, IPaperRepository paperRepository)
            : base(logger, portfolioBuilder, portfolioCalculator, portfolioService, paperRepository)
        {
        }

        public override async Task<IEnumerable<PortfolioAction>> Advise(Portfolio portfolio, ExternalImportType importType, IDictionary<string, string> p)
        {
            var availSum = GetAsDecimal(p, Constants.Adviser.BuyAndHold.P_AvailSum) ?? 0;
            var allowSell = GetAsBool(p, Constants.Adviser.P_AllowSell, false);
            var onDate = GetAsDateTime(p, Constants.Adviser.P_OnDate);

            if (0 == availSum)
            {
                return new List<PortfolioAction>();
            }

            await Prepare(portfolio, importType, onDate);

            while (availSum > 0)
            {
                var totalVolume = availSum + CalcTotalVolume();
                var balance = CalcBalance(totalVolume);

                if (allowSell == true)
                {
                    // выбираем бумаги, от которых надо избавится, и сумма больше чем цена за одну бумагу
                    var sellPapers = balance.Where(x => x.Value.Difference < 0);
                    foreach (var kv in sellPapers)
                    {
                        var countToSell = Convert.ToInt64(kv.Value.Difference * totalVolume / kv.Value.Price);
                        var foundPaper = _flattenPapers.ContainsKey(kv.Key) ? _flattenPapers[kv.Key].SecId : _portfolioPapers[kv.Key].Paper.SecId;
                        var sellActions = ChangeCount(foundPaper, countToSell, onDate);
                        foreach (var action in sellActions)
                        {
                            availSum += action.Sum;
                        }
                    }
                }

                // берем саму разбаланисированную бумагу и пытаемся купить
                var buyPaper = balance
                    .Where(x => x.Value.Difference > 0 && x.Value.Price <= availSum)
                    .OrderByDescending(x => x.Value.Difference)
                    .FirstOrDefault();

                if (null == buyPaper.Key)
                {
                    // мы ничего уже не можем купить
                    break;
                }

                var countToBuy = 1;
                var buyActions = ChangeCount(_flattenPapers[buyPaper.Key].SecId, countToBuy, onDate);
                foreach(var action in buyActions)
                {
                    availSum -= action.Sum;
                }
            }

            return Finish(onDate);
        }
    }
}