using OSPSuite.Core;
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
         });

         registerThirdPartyComponents(container);
      }

      private static void registerThirdPartyComponents(IContainer container)
      {
         container.Register<ICompression, SharpLibCompression>();
         container.Register<IStringCompression, StringCompression>();
      }
   }
}