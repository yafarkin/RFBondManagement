using RfBondManagement.Engine.Interfaces;
using RfFondPortfolio.Common.Dtos;
using RfFondPortfolio.Common.Interfaces;

namespace RfBondManagement.Engine.Database
{
    public class PortfolioMoneyActionRepository : BasePortfolioActionRepository<PortfolioMoneyAction>, IPortfolioMoneyActionRepository
    {
        protected override string _collectionName => "moneyActions";

        public PortfolioMoneyActionRepository(IDatabaseLayer db)
            : base(db)
        {
        }
    }
}