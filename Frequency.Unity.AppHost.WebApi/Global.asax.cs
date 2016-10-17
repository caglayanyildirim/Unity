using Frequency.Framework.Configuration.Configurers;
using Frequency.Framework.Logging;
using Frequency.Framework.Web;

namespace Frequency.Unity.AppHost.WebApi
{
    public class Global : WebServiceServiceClientApplication
    {
        protected override ServiceClientConfiguration ServiceClient(ServiceClientConfigurer configure)
        {
            return configure.Localhost(53325).Routine();
        }

        protected override WebServiceConfiguration WebService(WebServiceConfigurer configure)
        {
            return configure.WebApi();
        }

        protected override LoggingFeature Logging(LoggingConfigurer configure)
        {
            return configure.Log4Net(LogLevel.Debug);
        }
    }
}