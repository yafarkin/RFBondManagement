using System.Collections.Generic;
using System.Linq;
using RfFondPortfolio.Common.Interfaces;

namespace RfFondPortfolio.Common.Dtos
{
    public class PortfolioAggregatedContent
    {
        /// <summary>
        /// Список сумм по группам
        /// </summary>
        public IReadOnlyDictionary<MoneyActionType, decimal> Sums { get; set; }

        /// <summary>
        /// Список данных по бумагам
        /// </summary>
        public IReadOnlyList<IPaperInPortfolio<AbstractPaper>> Papers { get; set; }

        /// <summary>
        /// Доступный кэш
        /// </summary>
        public decimal AvailSum => Sums.Where(s => MoneyActionTypeHelper.IncomeTypes.Contains(s.Key) || MoneyActionTypeHelper.OutcomeTypes.Contains(s.Key)).Sum(s => s.Value);

        /// <summary>
        /// Разница между покупкой и текущей ценой портфеля
        /// </summary>
        public decimal Profit => Papers.Sum(p => p.Profit);
    }
}