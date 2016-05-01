using Frequency.Framework.Configuration.Configurers;
using Frequency.Framework.IO;
using Frequency.Framework.Logging;
using Frequency.Framework.Web;

namespace Frequency.Unity.AppHost.Soa
{
    public class Global : ServiceApplication
    {
        protected override ServiceClientConfiguration ServiceClientConfiguration(ServiceClientConfigurer configure)
        {
            return configure.Localhost(53325);
        }

        protected override DatabaseConfiguration DatabaseConfiguration(DatabaseConfigurer configure)
        {
            return configure.SqlServer2008("Oracle", "UnityDb",false);
        }

        protected override IFileSystem FileSystemConfiguration(FileSystemConfigurer configure)
        {
            return configure.LocalFileSystem(@"C:\SavedFromApplication", true);
        }

        protected override SchedulerConfiguration SchedulerConfiguration(SchedulerConfigurer configure)
        {
            return configure.NoScheduling();
        }

        protected override MessageQueueConfiguration MessageQueueConfiguration(MessageQueueConfigurer configure)
        {
            return configure.NoMessageQueue();
        }

        protected override LoggerConfiguration LoggerConfiguration(LoggerConfigurer configure)
        {
            return configure
                .Level("MNF.Cache", LogLevel.Info)
                .Level("MNF.DataAccess", LogLevel.Info)
                .Debug();
        }

        protected override bool IncludeModuleGroupInModuleNames { get { return true; } }

        protected override int ReportQueryTimeOutInSeconds { get { return 120; } }

        protected override int MaxResultLengthInBytes { get { return 80 * 1024 * 1024; } }

        protected override int ServiceRequestTimeoutInMinutes { get { return 45; } }
    }
}