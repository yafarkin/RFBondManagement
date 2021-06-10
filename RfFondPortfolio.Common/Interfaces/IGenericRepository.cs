using System.Collections.Generic;

namespace RfFondPortfolio.Common.Interfaces
{
    public interface IGenericRepository<T>
    {
        IEnumerable<T> Get();

        T Insert(T entity);

        void Update(T entity);

        void Delete(T entity);
    }
}