using OSPSuite.Core;
using OSPSuite.Infrastructure.Journal;
using OSPSuite.Infrastructure.Journal.Commands;
using OSPSuite.Infrastructure.Journal.Queries;
using OSPSuite.Infrastructure.ORM.MetaData;
using OSPSuite.Infrastructure.Services;
using OSPSuite.Utility.Container;

namespace OSPSuite.Infrastructure
{
   public class InfrastructureSerializationRegister : Register
   {
      public override void RegisterInContainer(IContainer container)
      {
         container.AddScanner(scan =>
         {
            scan.AssemblyContainingType<InfrastructureSerializationRegister>();
            scan.ExcludeType<SessionManager>();
            scan.ExcludeType<CommandMetaDataRepository>();
            scan.ExcludeType<JournalSession>();

            //Will be registered using specific convention
            scan.ExcludeNamespaceContainingType<AllKnownTags>();
            scan.ExcludeNamespaceContainingType<JournalPagePayload>();

            scan.WithConvention(new OSPSuiteRegistrationConvention(registerConcreteType: true));
         });

         container.Register<IJournalSession, JournalSession>(LifeStyle.Singleton);
         container.Register<ISessionManager, SessionManager>(LifeStyle.Singleton);
         container.Register<ICommandMetaDataRepository, CommandMetaDataRepository>(LifeStyle.Singleton);

         container.Register<SQLiteProjectCommandExecuter, SQLiteProjectCommandExecuter>();

         //Register working journal query and commands
         container.AddScanner(scan =>
         {
            scan.AssemblyContainingType<InfrastructureSerializationRegister>();
            scan.IncludeNamespaceContainingType<AllKnownTags>();
            scan.IncludeNamespaceContainingType<JournalPagePayload>();
            scan.WithConvention<JournalDatabaseCommandAndQueryConvention>();
         });
      }
   }
}