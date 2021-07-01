using System;
using RfBondManagement.Engine.Interfaces;
using RfFondPortfolio.Common.Dtos;
using RfFondPortfolio.Common.Interfaces;

namespace RfBondManagement.Engine.Database
{
    public class HistoryRepository : BaseSecRepository<HistoryPrice>, IHistoryRepository
    {
        protected override string _collectionName => "history";

        public HistoryRepository(IDatabaseLayer db)
            : base(db)
        {
            _entities.EnsureIndex(p => p.Index);
        }

        public HistoryPrice GetNearHistoryPriceOnDate(string secId, DateTime date)
        {
            var maxDays = 60;
            while (maxDays > 0)
            {
                var result = GetHistoryPriceOnDate(secId, date);
                if (null == result)
                {
                    date = date.AddDays(+1);
                    maxDays--;
                }
                else
                {
                    return result;
                }
            }

            return null;
        }

        public HistoryPrice GetHistoryPriceOnDate(string secId, DateTime date)
        {
            var index = $"{secId},{date:yyyyMMdd}";
            return _entities.FindOne(p => p.Index == index);
        }
    }
}