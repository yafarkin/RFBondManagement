﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RfBondManagement.Engine.Interfaces;

namespace BackTesting.Interfaces
{
    public interface IStrategy
    {
        IEnumerable<string> Papers { get; }

        string Description { get; }

        void Init(IPortfolioService portfolioService, DateTime date);

        Task<bool> Process(DateTime date);
    }
}