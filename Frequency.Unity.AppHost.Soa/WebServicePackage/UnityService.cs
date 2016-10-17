using Castle.MicroKernel;
using Frequency.Framework.Configuration;
using Frequency.Unity.Common.Module.Security.Service;
using Routine;
using Routine.Engine.Configuration.ConventionBased;

namespace Frequency.Unity.AppHost.Soa.WebServicePackage
{
    public class UnityService : ICodingStyleConfiguration
    {
        public void Configure(ConventionBasedCodingStyle codingStyle, IKernel kernel)
        {
            codingStyle
                 .AddTypes(
                     v => v.WebService("Unity", s => s
                     .Methods.Add(o => o.Proxy<IAccountManagerService>("CreateAccount").TargetBySingleton(kernel))
                     )
                );
        }
    }
}