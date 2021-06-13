using System.Collections.Generic;
using System.Linq;
using RfBondManagement.Engine.Interfaces;
using RfFondPortfolio.Common.Dtos;

namespace RfBondManagement.Engine.Database
{
    public abstract class BasePortfolioActionRepository<T> : BaseSecRepository<T> where T : PortfolioAction
    {
        protected override string _collectionName => "actions";

        protected BasePortfolioActionRepository(IDatabaseLayer db)
            : base(db)
        {
        }

        public IEnumerable<T> Get()
        {
            return _entities.FindAll().OfType<T>();
        }
    }
}