using OSPSuite.Core;
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
      }
   }
}