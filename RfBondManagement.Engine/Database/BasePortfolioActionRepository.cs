using System;
using System.Collections.Generic;
using System.Linq;
using RfBondManagement.Engine.Interfaces;
using RfFondPortfolio.Common.Dtos;

namespace RfBondManagement.Engine.Database
{
    public abstract class BasePortfolioActionRepository<T> : BaseSecRepository<T> where T : PortfolioAction
    {
        protected override string _collectionName => "actions";

        protected Guid _portfolioId;

        protected BasePortfolioActionRepository(IDatabaseLayer db)
            : base(db)
        {
        }

        public IEnumerable<T> Get()
        {
            if (_portfolioId == Guid.Empty)
            {
                throw new InvalidOperationException("Не задан идентификатор портфеля");
            }

            return _entities.FindAll().Where(x => x.PortfolioId == _portfolioId).OfType<T>();
        }

        public virtual void Setup(Guid portfolioId)
        {
            _portfolioId = portfolioId;
        }
    }
}