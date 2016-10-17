using Frequency.Framework.Configuration.Configurers;
using Frequency.Framework.Web;

namespace Frequency.Unity.AppHost.Mvc
{
    public class Global : MvcServiceClientApplication
    {
        protected override AuthenticationFeature Authentication(AuthenticationConfigurer configure)
        {
            return configure.TokenBased();
        }

        protected override ServiceClientConfiguration ServiceClient(ServiceClientConfigurer configure)
        {
            return configure.Localhost(35880).Routine();
        }

        protected override ThemeConfiguration Theme(ThemeConfigurer configure)
        {
            return configure.PublicTheme();
        }
    }
}