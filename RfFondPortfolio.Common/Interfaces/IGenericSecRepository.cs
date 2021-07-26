using RfFondPortfolio.Common.Dtos;

namespace RfFondPortfolio.Common.Interfaces
{
    public interface IGenericSecRepository<T> : IGenericRepository<T> where T : BaseSecurityEntity
    {
        T Get(string secId);
    }
}