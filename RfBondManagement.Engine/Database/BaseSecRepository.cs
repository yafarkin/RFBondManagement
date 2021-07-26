using RfBondManagement.Engine.Interfaces;
using RfFondPortfolio.Common.Dtos;

namespace RfBondManagement.Engine.Database
{
    public abstract class BaseSecRepository<T> : BaseRepository<T> where T : BaseSecurityEntity
    {
        protected BaseSecRepository(IDatabaseLayer db)
            : base(db)
        {
            _entities.EnsureIndex(p => p.SecId);
        }

        public T Get(string secId)
        {
            return _entities.FindOne(e => e.SecId == secId);
        }
    }
}