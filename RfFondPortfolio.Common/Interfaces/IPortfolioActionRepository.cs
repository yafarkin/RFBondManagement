using System;
using RfFondPortfolio.Common.Dtos;

namespace RfFondPortfolio.Common.Interfaces
{
    public interface IPortfolioActionRepository<T> : IGenericSecRepository<T> where T : BaseSecurityEntity
    {
        void Setup(Guid portfolioId);
    }
}