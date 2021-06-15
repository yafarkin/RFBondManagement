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
        /// Link to buy action, Link to sell action, Сount of available to sell
        /// </summary>
        IReadOnlyCollection<Tuple<PortfolioPaperAction, PortfolioPaperAction, long>> FifoActions { get; set; }

        public long Count { get; set; }

        decimal AveragePrice { get; set; }

        decimal MarketPrice { get; set; }

        decimal Profit { get; }
    }
}