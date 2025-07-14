using OSPSuite.CLI.Core.MinimalImplementations;
using OSPSuite.Core;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Container;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.CLI.Core
{
   public class CLIRegister : Register
   {
      public override void RegisterInContainer(IContainer container)
      {
         container.AddScanner(scan =>
         {
            scan.AssemblyContainingType<CLIRegister>();

            //Register Minimal implementations
            scan.IncludeNamespaceContainingType<DisplayUnitRetriever>();


            // This will be registered as singleton
            scan.ExcludeType<RGroupRepository>();
            scan.WithConvention<OSPSuiteRegistrationConvention>();
         });

         //Add specific implementations that are not registered automatically
         container.Register<IObjectTypeResolver, RObjectTypeResolver>();
         container.Register<IDiagramModel, CLIDiagramModel>();

         //Singletons
         container.Register<IGroupRepository, RGroupRepository>(LifeStyle.Singleton);
         container.Register<IOSPSuiteExecutionContext, RExecutionContext>(LifeStyle.Singleton);
      }
   }
}