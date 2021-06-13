using RfBondManagement.Engine.Interfaces;
using RfFondPortfolio.Common.Dtos;
using RfFondPortfolio.Common.Interfaces;

namespace RfBondManagement.Engine.Database
{
    public class PortfolioRepository : BaseRepository<Portfolio>, IPortfolioRepository
    {
        protected override string _collectionName => "portfolios";

        public PortfolioRepository(IDatabaseLayer db)
            : base(db)
        {
        }
    }
}