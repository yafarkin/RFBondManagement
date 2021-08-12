using System.Collections.Generic;
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

        public override IEnumerable<Portfolio> Get()
        {
            var portfolios = base.Get();
            foreach (var portfolio in portfolios)
            {
                yield return RestoreParent(portfolio);
            }
        }

        protected Portfolio RestoreParent(Portfolio portfolio)
        {
            var result = portfolio;

            if (portfolio.RootLeaf != null)
            {
                foreach (var child in portfolio.RootLeaf.Children)
                {
                    RestoreParentInChild(child, portfolio.RootLeaf);
                }
            }

            return result;
        }

        protected void RestoreParentInChild(PortfolioStructureLeaf child, PortfolioStructureLeaf parent)
        {
            child.Parent = parent;
            if (child.Children != null)
            {
                foreach (var child2 in child.Children)
                {
                    RestoreParentInChild(child2, child);
                }
            }
        }
    }
}