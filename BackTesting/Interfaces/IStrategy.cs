using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RfFondPortfolio.Common.Dtos;

namespace BackTesting.Interfaces
{
    public interface IStrategy
    {
        IEnumerable<string> Papers { get; }

        string Description { get; }

        void Init(Portfolio portfolio, DateTime date);

        Task<bool> Process(DateTime date);
    }
}