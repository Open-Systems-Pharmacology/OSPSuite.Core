using OSPSuite.Core;
using OSPSuite.Core.Services;
using OSPSuite.R.Domain;
using OSPSuite.R.Mapper;
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

            //Register Mappers
            scan.IncludeNamespaceContainingType<ISensitivityAnalysisToCoreSensitivityAnalysisMapper>();

            scan.WithConvention<OSPSuiteRegistrationConvention>();
         });

         //Add specific implementations that are not registered automatically
         container.Register<SimulationBatch, SimulationBatch>();
         container.Register<ISimulationBatchFactory, SimulationBatchFactory>();

         container.Register<IOSPSuiteLogger, RLogger, RLogger>(LifeStyle.Singleton);
         container.Register<IEventPublisher, EventPublisher>(LifeStyle.Singleton);
      }
   }
}