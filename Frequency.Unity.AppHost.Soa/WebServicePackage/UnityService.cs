using Castle.Windsor;
using Frequency.Framework.Configuration;
using Frequency.Framework.Configuration.Overriders;
using Frequency.Unity.Common.Module.Security.Service;
using Routine;
using Routine.Engine.Configuration.ConventionBased;

namespace Frequency.Unity.AppHost.Soa.WebServicePackage
{
    public class UnityService : IServiceConfigurationOverrider
    {
        public ConventionBasedCodingStyle MakeOverride(ConventionBasedCodingStyle codingStyle, IWindsorContainer container)
        {
            return codingStyle
                .AddTypes(
                    v => v.WebService("Unity", s => s
                    .Methods.Add(o => o.Proxy<IAccountManagerService>("CreateAccount").TargetBySingleton(container))
                    )
               );
        }
    }
}