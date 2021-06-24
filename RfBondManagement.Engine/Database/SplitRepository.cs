using RfBondManagement.Engine.Interfaces;
using RfFondPortfolio.Common.Dtos;
using RfFondPortfolio.Common.Interfaces;

namespace RfBondManagement.Engine.Database
{
    public class SplitRepository : BaseSecRepository<PaperSplit>, ISplitRepository
    {
        protected override string _collectionName => "splits";

        public SplitRepository(IDatabaseLayer db)
            : base(db)
        {
        }
    }
}