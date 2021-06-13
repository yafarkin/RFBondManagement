using System.Collections.Generic;
using LiteDB;
using RfBondManagement.Engine.Interfaces;
using RfFondPortfolio.Common.Dtos;
using RfFondPortfolio.Common.Interfaces;

namespace RfBondManagement.Engine.Database
{
    public abstract class BaseRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected IDatabaseLayer _db;
        protected ILiteCollection<T> _entities;

        protected abstract string _collectionName { get; }

        protected BaseRepository(IDatabaseLayer db)
        {
            _db = db;
            _entities = _db.Database.GetCollection<T>(_collectionName);
            _entities.EnsureIndex(p => p.Id);
        }

        public IEnumerable<T> Get()
        {
            return _entities.FindAll();
        }

        public T Insert(T entity)
        {
            _entities.Insert(entity);

            return entity;
        }

        public void Update(T entity)
        {
            _entities.Update(entity);
        }

        public void Delete(T entity)
        {
            _entities.DeleteMany(e => e.Id == entity.Id);
        }
    }
}