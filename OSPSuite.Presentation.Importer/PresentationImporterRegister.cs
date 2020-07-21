using OSPSuite.Core;
using OSPSuite.Utility.Container;

namespace OSPSuite.Presentation.Importer
{
   public class PresentationImporterRegister : Register
   {
      public override void RegisterInContainer(IContainer container)
      {
         container.AddScanner(scan =>
         {
            scan.AssemblyContainingType<PresentationImporterRegister>();

            scan.WithConvention(new OSPSuiteRegistrationConvention(registerConcreteType: true));
         });
      }
   }
}