using System;
using System.Collections.Generic;
using RfBondManagement.Engine.Common;

namespace BackTesting.Interfaces
{
    public interface IStrategy
    {
        IEnumerable<string> Papers { get; }

        string Description { get; }

        void Init(Portfolio portfolio, DateTime date);

        bool Process(DateTime date);
    }
}