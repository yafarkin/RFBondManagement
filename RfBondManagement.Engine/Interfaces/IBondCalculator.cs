using System;
using RfBondManagement.Engine.Common;
using RfFondPortfolio.Common.Dtos;

namespace RfBondManagement.Engine.Interfaces
{
    public interface IBondCalculator
    {
        int CalculateDuration(BondPaper paper, DateTime? fromDate = null, DateTime? toDate = null);

        void CalculateIncome(BondIncomeInfo bondIncomeInfo, Portfolio portfolio, DateTime toDate);

        decimal CalculateAci(BondPaper paper, DateTime toDate);
    }
}