using System;
using System.Collections.Generic;
using BackTesting.Interfaces;
using NLog;
using RfBondManagement.Engine.Interfaces;
using RfFondPortfolio.Common.Dtos;
using RfFondPortfolio.Common.Interfaces;

namespace BackTesting
{
    public abstract class BaseEmptyStrategy : IStrategy
    {
        protected readonly ILogger _logger;
        protected readonly IHistoryRepository _historyRepository;
        protected readonly IBondCalculator _bondCalculator;

        protected Portfolio _portfolio { get; set; }

        public abstract IEnumerable<string> Papers { get; }
        public abstract string Description { get; }
        public abstract void Init(IBacktestEngine backtestEngine, Portfolio portfolio, DateTime date);
        public abstract bool Process(DateTime date);

        protected BaseEmptyStrategy(ILogger logger, IHistoryRepository historyRepository, IBondCalculator bondCalculator)
        {
            _logger = logger;
            _historyRepository = historyRepository;
            _bondCalculator = bondCalculator;
        }
    }
}