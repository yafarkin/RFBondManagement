using System;
using System.Collections.Generic;
using RfFondPortfolio.Common.Dtos;

namespace RfFondPortfolio.Common.Interfaces
{
    public interface IPaperInPortfolio<out T> where T : AbstractPaper
    {
        DateTime? OnDate { get; set; }

        T Paper { get; }

        IReadOnlyCollection<PortfolioPaperAction> Actions { get; set; }

        /// <summary>
        /// Ссылка на действие покупки, ссылка на действие продажи, количество доступных бумаг для продажи
        /// </summary>
        IReadOnlyCollection<FifoAction> FifoActions { get; set; }

        public long Count { get; set; }

        decimal AveragePrice { get; set; }

        decimal MarketPrice { get; set; }

        decimal Profit { get; }
    }
}