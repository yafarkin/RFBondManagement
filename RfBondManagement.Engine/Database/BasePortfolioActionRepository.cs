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

        public override IEnumerable<T> Get()
        {
            if (_portfolioId == Guid.Empty)
            {
                throw new InvalidOperationException("Не задан идентификатор портфеля");
            }

            return _entities.FindAll().Where(x => x.PortfolioId == _portfolioId).OfType<T>();
        }

        public override T Insert(T entity)
        {
            if (_portfolioId == Guid.Empty)
            {
                throw new InvalidOperationException("Не задан идентификатор портфеля");
            }

            if (entity.PortfolioId != _portfolioId)
            {
                throw new InvalidOperationException($"Идентификатор портфеля в сущности {entity.PortfolioId} не совпадает с портфелем {_portfolioId}");
            }

            return base.Insert(entity);
        }

        public override void Update(T entity)
        {
            if (_portfolioId == Guid.Empty)
            {
                throw new InvalidOperationException("Не задан идентификатор портфеля");
            }

            if (entity.PortfolioId != _portfolioId)
            {
                throw new InvalidOperationException($"Идентификатор портфеля в сущности {entity.PortfolioId} не совпадает с портфелем {_portfolioId}");
            }

            base.Update(entity);
        }

        public override void Delete(T entity)
        {
            if (_portfolioId == Guid.Empty)
            {
                throw new InvalidOperationException("Не задан идентификатор портфеля");
            }

            if (entity.PortfolioId != _portfolioId)
            {
                throw new InvalidOperationException($"Идентификатор портфеля в сущности {entity.PortfolioId} не совпадает с портфелем {_portfolioId}");
            }

            base.Delete(entity);
        }

        public virtual void Setup(Guid portfolioId)
        {
            _portfolioId = portfolioId;
        }
    }
}