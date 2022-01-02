using System.Collections.Generic;
using System.Threading.Tasks;
using RfBondManagement.Engine.Common;
using RfFondPortfolio.Common.Dtos;

namespace RfBondManagement.Engine.Interfaces
{
    public interface IAdviser
    {
        Task<IEnumerable<PortfolioAction>> Advise(Portfolio portfolio, ExternalImportType importType, IDictionary<string, string> p);
    }
}