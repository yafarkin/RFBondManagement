using System.Collections.Generic;
using RfFondPortfolio.Common.Dtos;

namespace RfFondPortfolio.Common.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        IEnumerable<T> Get();

        T Insert(T entity);

        void Update(T entity);

        void Delete(T entity);
    }
}