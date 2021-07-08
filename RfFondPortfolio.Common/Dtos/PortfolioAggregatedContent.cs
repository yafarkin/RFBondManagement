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

        public decimal TotalIncome => Sums.Where(s => MoneyActionTypeHelper.IncomeTypes.Contains(s.Key)).Sum(s => s.Value);

        public decimal TotalOutcome => Sums.Where(s => MoneyActionTypeHelper.OutcomeTypes.Contains(s.Key)).Sum(s => s.Value);

        /// <summary>
        /// Доступный кэш
        /// </summary>
        public decimal AvailSum => TotalIncome - TotalOutcome;

        /// <summary>
        /// Разница между покупкой и текущей ценой портфеля
        /// </summary>
        public decimal Profit => Papers.Sum(p => p.Profit);
    }
}