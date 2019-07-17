using OSPSuite.Core;
using OSPSuite.Utility.Container;

namespace OSPSuite.Infrastructure.Export
{
   public class InfrastructureExportRegister : Register
   {
      public override void RegisterInContainer(IContainer container)
      {
         container.AddScanner(scan =>
         {
            scan.AssemblyContainingType<InfrastructureExportRegister>();

            scan.WithConvention(new OSPSuiteRegistrationConvention(registerConcreteType: true));
         });
      }
   }
}