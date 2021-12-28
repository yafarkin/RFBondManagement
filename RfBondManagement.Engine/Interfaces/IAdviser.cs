using System.Collections.Generic;
using System.Threading.Tasks;
using RfFondPortfolio.Common.Dtos;

namespace RfBondManagement.Engine.Interfaces
{
    public interface IAdviser
    {
        Task<IEnumerable<PortfolioAction>> Advise(Portfolio portfolio);
    }
}