using System;
using RfFondPortfolio.Common.Dtos;

namespace RfFondPortfolio.Common.Interfaces
{
    public interface IHistoryRepository : IGenericSecRepository<HistoryPrice>
    {
        HistoryPrice GetNearHistoryPriceOnDate(string secId, DateTime date);

        HistoryPrice GetHistoryPriceOnDate(string secId, DateTime date);
    }
}