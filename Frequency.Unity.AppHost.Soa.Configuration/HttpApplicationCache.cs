using System;
using System.Web;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Frequency.Framework.Configuration;
using Frequency.Unity.Common.Module.SharedData;

namespace Frequency.Unity.AppHost.Soa.Configuration
{
    public class HttpApplicationCache : IIoCConfiguration
    {
        void IIoCConfiguration.Configure(IKernel kernel)
        {
            if (!kernel.HasComponent(typeof(IApplicationCache)))
            {
                kernel.Register(Component.For<IApplicationCache>().ImplementedBy<DotNet>().LifestyleSingleton());
            }
        }

        public class DotNet : IApplicationCache
        {
            public T Get<T>(string key, Func<T> getFunction)
            {
                var applicationState = HttpContext.Current.Application;
                var obj = (T)applicationState[key];

                if (obj != null)
                {
                    return obj;
                }

                applicationState.Lock();

                applicationState[key] = obj = getFunction();

                applicationState.UnLock();

                return obj;
            }

            public object Remove(string key)
            {
                var applicationState = HttpContext.Current.Application;
                var obj = applicationState[key];

                if (obj == null)
                {
                    return null;
                }
                applicationState.Lock();

                applicationState.Remove(key);

                applicationState.UnLock();

                return obj;
            }

            public void Clear()
            {
                var applicationState = HttpContext.Current.Application;
                applicationState.Lock();
                applicationState.Clear();
                applicationState.UnLock();
            }
        }
    }
}