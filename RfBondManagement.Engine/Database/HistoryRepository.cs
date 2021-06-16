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
    }
}