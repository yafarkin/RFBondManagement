using System;
using System.Collections.Generic;
using System.Linq;
using RfFondPortfolio.Common.Dtos;

namespace BackTesting
{
    /// <summary>
    /// Статистика бэктестинга
    /// </summary>
    public class Statistic
    {
        /// <summary>
        /// Дата
        /// </summary>
        public DateTime Date;

        /// <summary>
        /// Суммы
        /// </summary>
        /// <remarks>Тип суммы, сумма</remarks>
        public IReadOnlyDictionary<MoneyActionType, decimal> Sum;

        /// <summary>
        /// Значение курса USD к рублю на дату
        /// </summary>
        public decimal UsdRubValue;

        /// <summary>
        /// Стоимость портфеля в долларах
        /// </summary>
        public decimal UsdPortfolioCost => Math.Round(0 == UsdRubValue ? 0 : PortfolioCost / UsdRubValue, 2);

        /// <summary>
        /// Стоимость портфеля
        /// </summary>
        public decimal PortfolioCost => Papers?.Sum(p => p.Item2 * p.Item4) ?? 0;

        /// <summary>
        /// Незадействованная сумма
        /// </summary>
        public decimal Cash;

        /// <summary>
        /// Список бумаг в портфеле
        /// </summary>
        /// <remarks>SecId, количество, средняя цена покупки, рыночная цена</remarks>
        public IReadOnlyList<Tuple<string, long, decimal, decimal>> Papers;
    }
}