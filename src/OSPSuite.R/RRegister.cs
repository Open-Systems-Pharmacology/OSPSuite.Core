﻿using OSPSuite.Core;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.R.Domain;
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
            scan.ExcludeType<RGroupRepository>();
            scan.WithConvention<OSPSuiteRegistrationConvention>();
         });

         //Add specific implementations that are not registered automatically
         container.Register<SimulationBatch, SimulationBatch>();
         container.Register<ISimulationBatchFactory, SimulationBatchFactory>();

         //Singletons
         container.Register<IGroupRepository, RGroupRepository>(LifeStyle.Singleton);
         container.Register<IOSPSuiteExecutionContext, RExecutionContext>(LifeStyle.Singleton);
         container.Register<IOSPSuiteLogger, RLogger, RLogger>(LifeStyle.Singleton);
         container.Register<IEventPublisher, EventPublisher>(LifeStyle.Singleton);
         container.Register<ICsvDynamicSeparatorSelector, ICsvSeparatorSelector, CsvSeparatorSelector>(LifeStyle.Singleton);
      }
   }
}