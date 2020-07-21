using OSPSuite.Core;
using OSPSuite.Utility.Container;

namespace OSPSuite.Presentation.DeprecatedImporter
{
   public class PresentationDeprecatedImporterRegister : Register
   {
      public override void RegisterInContainer(IContainer container)
      {
         container.AddScanner(scan =>
         {
            scan.AssemblyContainingType<PresentationDeprecatedImporterRegister>();

            scan.WithConvention(new OSPSuiteRegistrationConvention(registerConcreteType: true));
         });
      }
   }
}