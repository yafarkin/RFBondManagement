using System.Collections.Generic;
using System.Linq;
using RfBondManagement.Engine.Interfaces;
using RfFondPortfolio.Common.Dtos;
using RfFondPortfolio.Common.Interfaces;

namespace RfBondManagement.Engine.Database
{
    public class PortfolioPaperActionRepository : BasePortfolioActionRepository<PortfolioPaperAction>, IPortfolioPaperActionRepository
    {
        protected override string _collectionName => "paperActions";

        public PortfolioPaperActionRepository(IDatabaseLayer db)
            : base(db)
        {
        }
    }
}