using System;
using System.Collections.Generic;
using RfBondManagement.Engine.Interfaces;
using RfFondPortfolio.Common.Interfaces;
using RfFondPortfolio.Integration.Moex;
using Unity;

namespace RfBondManagement.Engine.Common
{
    internal class ExternalImportFactory : IExternalImportFactory
    {
        protected readonly Dictionary<ExternalImportType, Type> _impl = new Dictionary<ExternalImportType, Type>();
        protected readonly IUnityContainer _container;

        public ExternalImportFactory(IUnityContainer container)
        {
            _container = container;

            _impl.Add(ExternalImportType.Moex, typeof(MoexImport));
        }

        public IExternalImport GetImpl(ExternalImportType externalImport)
        {
            var t = _impl[externalImport];
            var importImpl = _container.Resolve(t, externalImport.ToString()) as IExternalImport;

            return new ExternalImport(importImpl);
        }
    }
}