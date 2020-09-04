using OSPSuite.Core;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Services;
using OSPSuite.Utility.Compression;
using OSPSuite.Utility.Container;

namespace OSPSuite.Infrastructure
{
   public class InfrastructureRegister : Register
   {
      public override void RegisterInContainer(IContainer container)
      {
         container.AddScanner(scan =>
         {
            scan.AssemblyContainingType<InfrastructureRegister>();
            scan.WithConvention(new OSPSuiteRegistrationConvention(registerConcreteType: true));
            scan.ExcludeType<OSPLogger>();
            scan.ExcludeType<LoggerCreator>(); ;
         });

         registerThirdPartyComponents(container);
         registerLogging(container);
      }

      private static void registerLogging(IContainer container)
      {
         var loggerCreator = new LoggerCreator();
         container.RegisterImplementationOf((ILoggerCreator)loggerCreator);
         container.Register<IOSPLogger, OSPLogger>(LifeStyle.Singleton);
      }

      private static void registerThirdPartyComponents(IContainer container)
      {
         container.Register<ICompression, SharpLibCompression>();
         container.Register<IStringCompression, StringCompression>();
      }
   }
}