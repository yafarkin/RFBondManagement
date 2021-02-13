using System;
using RfBondManagement.Engine.Common;

namespace RfBondManagement.Engine.Interfaces
{
    public interface IBondCalculator
    {
        void CalculateIncome(BondIncomeInfo bondIncomeInfo, Settings settings, DateTime toDate);
    }
}