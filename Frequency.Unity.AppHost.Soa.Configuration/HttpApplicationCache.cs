using System;
using System.Web;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Frequency.Framework.Configuration.Overriders;
using Frequency.Unity.Common.Module.SharedData;

namespace Frequency.Unity.AppHost.Soa.Configuration
{
	public class HttpApplicationCache : IApplicationCache, IIoCConfigurationOverrider
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

		public IWindsorContainer MakeOverride(IWindsorContainer container)
		{
			return container.Register(Component.For<IApplicationCache>().ImplementedBy<HttpApplicationCache>().LifestyleSingleton());
		}
	}
}