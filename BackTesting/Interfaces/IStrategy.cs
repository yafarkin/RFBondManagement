using System;
using System.Collections.Generic;
using RfFondPortfolio.Common.Dtos;

namespace BackTesting.Interfaces
{
    public interface IStrategy
    {
        IEnumerable<string> Papers { get; }

        string Description { get; }

        void Init(IBacktestEngine backtestEngine, Portfolio portfolio, DateTime date);

        bool Process(DateTime date);
    }
}