using OSPSuite.Core;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain;
using OSPSuite.R.Mapper;
using OSPSuite.R.MinimalImplementations;
using OSPSuite.R.Services;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Events;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.R
{
   public class RRegister : Register
   {
      public override void RegisterInContainer(IContainer container)
      {
         container.AddScanner(scan =>
         {
            scan.AssemblyContainingType<RRegister>();

            //Register Services
            scan.IncludeNamespaceContainingType<ISimulationRunner>();

            //Register Minimal implementations
            scan.IncludeNamespaceContainingType<DisplayUnitRetriever>();

            //Register Mappers
            scan.IncludeNamespaceContainingType<ISensitivityAnalysisToCoreSensitivityAnalysisMapper>();

            // This will be registered as singleton
            scan.ExcludeType<GroupRepository>();
            scan.WithConvention<OSPSuiteRegistrationConvention>();
         });

         container.Register<IGroupRepository, GroupRepository>(LifeStyle.Singleton);

         container.Register<IOSPSuiteExecutionContext, RExecutionContext>(LifeStyle.Singleton);
         container.Register<IEventPublisher, EventPublisher>(LifeStyle.Singleton);
      }
   }
}