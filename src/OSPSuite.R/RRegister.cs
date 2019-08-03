using OSPSuite.Core;
using OSPSuite.R.Services;
using OSPSuite.Utility.Container;

namespace OSPSuite.R
{
   public class RRegister : Register
   {
      public override void RegisterInContainer(IContainer container)
      {
         container.AddScanner(scan =>
         {
            scan.AssemblyContainingType<RRegister>();

            //REGISTER Services
            scan.IncludeNamespaceContainingType<ISimulationRunner>();
            scan.WithConvention<OSPSuiteRegistrationConvention>();
         });
      }
   }
}