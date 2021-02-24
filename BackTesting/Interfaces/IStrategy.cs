using System;
using RfBondManagement.Engine.Common;

namespace BackTesting.Interfaces
{
    public interface IStrategy
    {
        string Description { get; }

        void Init(Portfolio portfolio, DateTime date);

        bool Process(DateTime date);
    }
}