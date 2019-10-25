using OSPSuite.Core;
using OSPSuite.Infrastructure.Serialization.Journal;
using OSPSuite.Infrastructure.Serialization.Journal.Commands;
using OSPSuite.Infrastructure.Serialization.Journal.Queries;
using OSPSuite.Infrastructure.Serialization.ORM.MetaData;
using OSPSuite.Infrastructure.Serialization.Services;
using OSPSuite.Utility.Container;

namespace OSPSuite.Infrastructure.Serialization
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