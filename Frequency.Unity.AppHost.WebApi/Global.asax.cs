using Frequency.Framework.Configuration.Configurers;
using Frequency.Framework.Web;

namespace Frequency.Unity.AppHost.WebApi
{
    public class Global : WebApiServiceClientApplication
    {
        protected override ServiceClientConfiguration ServiceClientConfiguration(ServiceClientConfigurer configure)
        {
			return configure.Localhost(53325);

        }

        protected override int MaxResultLengthInBytes { get { return 80 * 1024 * 1024; } }
    }
}