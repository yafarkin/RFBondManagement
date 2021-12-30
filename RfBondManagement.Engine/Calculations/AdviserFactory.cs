using System.Collections.Generic;
using RfBondManagement.Engine.Common;
using RfBondManagement.Engine.Interfaces;
using Unity;
using Unity.Resolution;

namespace RfBondManagement.Engine.Calculations
{
    public class AdviserFactory : IAdviserFactory
    {
        protected readonly IUnityContainer _container;
        
        public AdviserFactory(IUnityContainer container)
        {
            _container = container;
        }
        
        public IAdviser GetAdviser(string adviserType)
        {
            return _container.Resolve<IAdviser>(adviserType);
        }
    }
}