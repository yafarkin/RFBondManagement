using RfBondManagement.Engine.Common;
using RfFondPortfolio.Common.Interfaces;

namespace RfBondManagement.Engine.Interfaces
{
    public interface IExternalImportFactory
    {
        IExternalImport GetImpl(ExternalImportType externalImport);

        IExternalImport GetDefaultImpl();
    }
}