using System;
using System.Collections.Generic;
using RfFondPortfolio.Common.Interfaces;

namespace RfFondPortfolio.Common.Dtos
{
    /// <summary>
    /// Данные по бумаге в портфеле
    /// </summary>
    public abstract class PaperInPortfolio<T> : IPaperInPortfolio<T> where T : AbstractPaper
    {
        /// <summary>
        /// На какую дату сформирована информация
        /// </summary>
        public DateTime? OnDate { get; set; }

        /// <summary>
        /// Бумага
        /// </summary>
        public T Paper { get; set; }

        /// <summary>
        /// Список действий по бумаге
        /// </summary>
        public IReadOnlyCollection<PortfolioPaperAction> Actions { get; set; }

        /// <summary>
        /// Список действий купли/продажи в FIFO порядке
        /// </summary>
        public IReadOnlyCollection<Tuple<PortfolioPaperAction, PortfolioPaperAction, long>> FifoActions { get; set; }

        /// <summary>
        /// Количество бумаг
        /// </summary>
        public long Count { get; set; }

        /// <summary>
        /// Средняя цена
        /// </summary>
        public decimal AveragePrice { get; set; }

        /// <summary>
        /// Рыночная цена (либо последняя цена, либо на определенную дату)
        /// </summary>
        public decimal MarketPrice { get; set; }

        /// <summary>
        /// Профит
        /// </summary>
        public decimal Profit => MarketPrice - AveragePrice;
    }
}