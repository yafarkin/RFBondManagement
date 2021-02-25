using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using BackTesting.Interfaces;
using NLog;
using RfBondManagement.Engine.Common;
using RfBondManagement.Engine.Interfaces;

namespace BackTesting
{
    public abstract class BaseEmptyStrategy : IStrategy
    {
        protected readonly ILogger _logger;
        protected readonly IHistoryDatabaseLayer _history;
        protected readonly IBondCalculator _bondCalculator;
        protected readonly IBacktestEngine _backtestEngine;

        protected Portfolio _portfolio { get; set; }

        public abstract IEnumerable<string> Papers { get; }
        public abstract string Description { get; }
        public abstract void Init(Portfolio portfolio, DateTime date);
        public abstract bool Process(DateTime date);

        protected BaseEmptyStrategy(ILogger logger, IHistoryDatabaseLayer history, IBondCalculator bondCalculator, IBacktestEngine backtestEngine)
        {
            _logger = logger;
            _history = history;
            _bondCalculator = bondCalculator;
            _backtestEngine = backtestEngine;
        }

    }
}