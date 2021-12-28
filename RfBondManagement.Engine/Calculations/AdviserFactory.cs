using System.Collections.Generic;
using RfBondManagement.Engine.Interfaces;
using Unity;
using Unity.Resolution;

namespace RfBondManagement.Engine.Calculations
{
    public static class AdviserFactory
    {
        public static IAdviser GetAdviser(IUnityContainer container, string adviserType, IDictionary<string, string> p)
        {
            return container.Resolve<IAdviser>(adviserType, new ParameterOverride("p", p));
        }
    }
}