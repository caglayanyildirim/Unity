using System;
using System.Data;
using Castle.MicroKernel;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Frequency.Framework;
using Frequency.Framework.Configuration;
using Frequency.Framework.Configuration.Configurers;
using Frequency.Framework.DataAccess.NHibernate;
using Frequency.Framework.Logging;
using Frequency.Framework.Web;


namespace Frequency.Unity.AppHost.Soa
{
    public class BatchSizePatch : INHibernateConfiguration
    {
        public void Configure(FluentConfiguration fluentConfiguration, IKernel kernel)
        {
            fluentConfiguration.Database(
                MsSqlConfiguration.MsSql2008
                    .ConnectionString(c => c.Database("ApplicationDB").Server("ISTDBDEVSRV").TrustedConnection())
                    .MaxFetchDepth(Constants.DEFAULT_DATA_ACCESS_MAX_FETCH_DEPTH)
                    .ShowSql()
                    .AdoNetBatchSize(100)
                );
        }

        public void Configure(NHibernateInterceptor nhibernateInterceptor, IKernel kernel) { }
    }

    public class Global : ServiceApplication
    {
        protected override DatabaseConfiguration Database(DatabaseConfigurer configure)
        {
            return configure.SqlServer2005("ISTDBDEVSRV", "ApplicationDB");
            //return configure.SqlServer2005("ISTSQLTESTSRV", "ApplicationDB");
        }

        protected override NullParentsOption NullParents(NullParentsConfigurer configure)
        {
            return configure.UseZeroId();
        }

        protected override TransactionFeature Transaction(TransactionConfigurer configure)
        {
            return configure.NHibernate(IsolationLevel.ReadUncommitted);
        }

        protected override ReportEngineFeature ReportEngine(ReportEngineConfigurer configure)
        {
            return configure.NativeSql(TimeSpan.FromSeconds(120));
        }

        protected override ServiceConfiguration Service(ServiceConfigurer configure)
        {
            return configure
                .MaxResultLengthInBytes(80 * 1024 * 1024)
                .RequestTimeout(TimeSpan.FromMinutes(45))
                .Localhost(32805)
                .Routine();
        }

        protected override ModuleConfiguration Module(ModuleConfigurer configure)
        {
            return configure.Routine(true);
        }

        protected override AuthenticationFeature Authentication(AuthenticationConfigurer configure)
        {
            return configure.TokenBased(4);
        }

        protected override SchedulerFeature Scheduling(SchedulerConfigurer configure)
        {
            return configure.NoScheduling();
            //return configure.Quartz();
        }

        protected override MessageQueueFeature MessageQueue(MessageQueueConfigurer configure)
        {
            return configure.NoMessageQueue();
            //return configure.RabbitMq("test.mq.multinetlocal.com.tr", "EnterpriseServices/" + Environment.UserName);
        }

        protected override FileSystemFeature FileSystem(FileSystemConfigurer configure)
        {
            return configure.Local(@"C:\SavedFromApplication", true);
        }

        protected override LoggingFeature Logging(LoggingConfigurer configure)
        {
            return configure
                .Level("Frequency.Framework", LogLevel.Info)
                .Log4Net(LogLevel.Debug);
        }
    }
}