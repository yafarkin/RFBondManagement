using System;
using System.Security;
using NLog;
using Unity.Builder;
using Unity.Extension;
using Unity.Policy;
using Unity.Resolution;

namespace RfBondManagement.Engine
{
    /// <summary>
    /// Source from: https://github.com/unitycontainer/NLog/blob/master/src/NLogExtension.cs
    /// </summary>
    [SecuritySafeCritical]
    public class NLogExtension : UnityContainerExtension
    {
        private static readonly Func<Type, string> _defaultGetName = (t) => t?.FullName ?? "default";

        public Func<Type, string> GetName { get; set; }

        protected override void Initialize()
        {
            Context.Policies.Set(typeof(ILogger), null, typeof(ResolveDelegateFactory), (ResolveDelegateFactory)GetResolver);
        }

        public ResolveDelegate<BuilderContext> GetResolver(ref BuilderContext context)
        {
            var method = GetName ?? _defaultGetName;
            Type declaringType = context.DeclaringType;

            return (ref BuilderContext c) => LogManager.GetLogger(method(declaringType));
        }
    }
}