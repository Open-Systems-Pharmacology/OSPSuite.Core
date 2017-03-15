using OSPSuite.Core;
using OSPSuite.Core.Reporting;
using OSPSuite.Utility.Container;
using OSPSuite.Infrastructure.Journal;
using OSPSuite.Infrastructure.Journal.Commands;
using OSPSuite.Infrastructure.Journal.Queries;
using OSPSuite.Infrastructure.Reporting;
using OSPSuite.Infrastructure.Serialization.ORM.MetaData;
using OSPSuite.Infrastructure.Services;

namespace OSPSuite.Infrastructure
{
   public class InfrastructureRegister : Register
   {
      public override void RegisterInContainer(IContainer container)
      {
         container.AddScanner(scan =>
         {
            scan.AssemblyContainingType<InfrastructureRegister>();
            scan.ExcludeType<SessionManager>();
            scan.ExcludeType<CommandMetaDataRepository>();
            scan.ExcludeType<JournalSession>();
            scan.ExcludeType<ReportTemplateRepository>();

            //Will be registered using specific convention
            scan.ExcludeNamespaceContainingType<AllKnownTags>();
            scan.ExcludeNamespaceContainingType<JournalPagePayload>();
            scan.ExcludeNamespaceContainingType<ReportingRegister>();

            scan.WithConvention(new OSPSuiteRegistrationConvention(registerConcreteType: true));
         });

         container.Register<IJournalSession, JournalSession>(LifeStyle.Singleton);
         container.Register<IReportTemplateRepository, ReportTemplateRepository>(LifeStyle.Singleton);
         container.Register<ISessionManager, SessionManager>(LifeStyle.Singleton);
         container.Register<ICommandMetaDataRepository, CommandMetaDataRepository>(LifeStyle.Singleton);

         //Register working journal query and commands
         container.AddScanner(scan =>
         {
            scan.AssemblyContainingType<InfrastructureRegister>();
            scan.IncludeNamespaceContainingType<AllKnownTags>();
            scan.IncludeNamespaceContainingType<JournalPagePayload>();
            scan.WithConvention<JournalDatabaseCommandAndQueryConvention>();
         });
      }
   }
}
