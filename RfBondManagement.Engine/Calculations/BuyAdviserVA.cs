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
    public class BuyAdviserVA : BaseAdviser
    {
        public BuyAdviserVA(ILogger logger, IPortfolioBuilder portfolioBuilder,
            IPortfolioCalculator portfolioCalculator, IPortfolioService portfolioService, IPaperRepository paperRepository)
            : base(logger, portfolioBuilder, portfolioCalculator, portfolioService, paperRepository)
        {
        }

        public override async Task<IEnumerable<PortfolioAction>> Advise(Portfolio portfolio, ExternalImportType importType, IDictionary<string, string> p)
        {
            var allowSell = GetAsBool(p, Constants.Adviser.P_AllowSell, false);
            var onDate = GetAsDateTime(p, Constants.Adviser.P_OnDate);
            var expectedVolume = GetAsDecimal(p, Constants.Adviser.BuyAndHoldWithVA.P_ExpectedVolume) ?? 0m;

            await Prepare(portfolio, importType, onDate);

            if (0 == expectedVolume)
            {
                return new List<PortfolioAction>();
            }

            while (true)
            {
                string secId = null;
                long countToChange = 0;

                var totalVolume = CalcTotalVolume();
                var balance = CalcBalance(expectedVolume);

                if (totalVolume < expectedVolume)
                {
                    // можем докупать, имеющийся объём меньше указанного
                    var availSum = expectedVolume - totalVolume;
                    var buyPaper = balance
                        .Where(x => x.Value.Difference > 0 && x.Value.Price <= availSum)
                        .OrderByDescending(x => x.Value.Difference)
                        .FirstOrDefault();
                    if (buyPaper.Key != null)
                    {
                        countToChange = 1;
                        secId = buyPaper.Key;
                    }
                }
                else if (totalVolume > expectedVolume && allowSell == true)
                {
                    // объём портфеля больше указанного, можно что то продать
                    var availSum = totalVolume - expectedVolume;
                    var sellPaper = balance
                        .Where(x => x.Value.Price <= availSum)
                        .OrderBy(x => x.Value.Difference)
                        .FirstOrDefault();
                    if (sellPaper.Key != null)
                    {
                        var diffSumm = Math.Abs(sellPaper.Value.Difference) * expectedVolume;

                        var summToSell = availSum > diffSumm
                            ? diffSumm
                            : availSum;

                        var countInPortfolio = _portfolioPapers[sellPaper.Key].Count;
                        countToChange = Convert.ToInt64(summToSell / sellPaper.Value.Price);
                        if (countToChange > countInPortfolio)
                        {
                            countToChange = countInPortfolio;
                        }

                        secId = sellPaper.Key;
                        countToChange = -countToChange;
                    }
                }

                if (0 == countToChange || null == secId)
                {
                    // не смогли ничего купить или продать
                    break;
                }

                ChangeCount(secId, countToChange, onDate);
            }

            return Finish(onDate);
        }
    }
}