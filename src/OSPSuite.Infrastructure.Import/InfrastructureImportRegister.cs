using OSPSuite.Utility.Container;

namespace OSPSuite.Infrastructure.Import
{
   public class InfrastructureImportRegister : Register
   {
      public override void RegisterInContainer(IContainer container)
      {
         container.AddScanner(x =>
         {
            x.AssemblyContainingType<InfrastructureImportRegister>();
            x.WithDefaultConvention();
         });
      }
   }
}