using OSPSuite.Utility.Container;

namespace OSPSuite.UI
{
   public class UIImporterRegister : Register
   {
      public override void RegisterInContainer(IContainer container)
      {
         container.AddScanner(scan =>
         {
            scan.AssemblyContainingType<UIImporterRegister>();
            scan.WithDefaultConvention();
         });
      }
   }
}