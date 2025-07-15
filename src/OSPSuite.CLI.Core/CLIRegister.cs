using OSPSuite.CLI.Core.MinimalImplementations;
using OSPSuite.Core;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
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
            scan.ExcludeType<GroupRepository>();
            scan.WithConvention<OSPSuiteRegistrationConvention>();
         });

         container.Register<IDiagramModel, CLIDiagramModel>();

         //Singletons
         container.Register<IGroupRepository, GroupRepository>(LifeStyle.Singleton);
         container.Register<IOSPSuiteExecutionContext, ExecutionContext>(LifeStyle.Singleton);
      }
   }
}