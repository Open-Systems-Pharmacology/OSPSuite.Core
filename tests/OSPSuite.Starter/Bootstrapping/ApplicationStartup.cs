using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Castle.Facilities.TypedFactory;
using Microsoft.Extensions.Logging;
using OSPSuite.Core;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.PKAnalyses;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Helpers;
using OSPSuite.Infrastructure;
using OSPSuite.Infrastructure.Container.Castle;
using OSPSuite.Infrastructure.Export;
using OSPSuite.Infrastructure.Import;
using OSPSuite.Infrastructure.Serialization;
using OSPSuite.Infrastructure.Services;
using OSPSuite.Presentation;
using OSPSuite.Presentation.Core;
using OSPSuite.Starter.Presenters;
using OSPSuite.Starter.Views;
using OSPSuite.UI;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Events;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.Starter.Bootstrapping
{
   public class ApplicationStartup
   {
      public static void Initialize()
      {
         initializeDependency();
         fillDimensions(IoC.Resolve<IDimensionFactory>());
         loadPKParameterRepository(IoC.Container);
         configureLogger(IoC.Container, LogLevel.Critical);
      }

      private static void configureLogger(IContainer container, LogLevel logLevel)
      {
         var loggerCreator = container.Resolve<ILoggerCreator>();

         loggerCreator
            .AddLoggingBuilderConfiguration(builder =>
               builder
                  .SetMinimumLevel(logLevel)
                  .AddFile("log.txt", logLevel, false)
            );
      }

      private static void loadPKParameterRepository(IContainer container)
      {
         var pkParameterRepository = container.Resolve<IPKParameterRepository>();
         var pKParameterLoader = container.Resolve<IPKParameterRepositoryLoader>();
         var configuration = container.Resolve<IApplicationConfiguration>();
         pKParameterLoader.Load(pkParameterRepository, configuration.PKParametersFilePath);
      }

      private static void fillDimensions(IDimensionFactory dimensionFactory)
      {
         var persistor = IoC.Resolve<IDimensionFactoryPersistor>();
         persistor.Load(dimensionFactory, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "OSPSuite.Dimensions.xml"));
         dimensionFactory.AddDimension(Constants.Dimension.NO_DIMENSION);
      }

      private static SynchronizationContext getCurrentContext()
      {
         var context = SynchronizationContext.Current;
         if (context != null) return SynchronizationContext.Current;

         context = new WindowsFormsSynchronizationContext();
         SynchronizationContext.SetSynchronizationContext(context);
         return SynchronizationContext.Current;
      }

      private static void initializeDependency()
      {
         var container = new CastleWindsorContainer();
         container.AddFacility<EventRegisterFacility>();
         container.AddFacility<TypedFactoryFacility>();


         IoC.InitializeWith(container);
         var serializerRegister = new CoreSerializerRegister();

         using (container.OptimizeDependencyResolution())
         {
            container.RegisterImplementationOf((IContainer) container);

            container.Register<IObjectBaseFactory, ObjectBaseFactory>(LifeStyle.Singleton);
            container.AddRegister(x => x.FromType<PresenterRegister>());
            container.AddRegister(x => x.FromType<UIRegister>());
            container.AddRegister(x => x.FromType<CoreRegister>());
            container.AddRegister(x => x.FromType<TestRegister>());
            container.AddRegister(x => x.FromType<InfrastructureRegister>());
            container.AddRegister(x => x.FromType<InfrastructureExportRegister>());
            container.AddRegister(x => x.FromType<InfrastructureSerializationRegister>());
            container.AddRegister(x => x.FromType<InfrastructureImportRegister>());
            container.AddRegister(x => x.FromInstance(serializerRegister));

            container.Register<IDimensionFactory, DimensionFactory>(LifeStyle.Singleton);
            container.Register<IApplicationController, ApplicationController>(LifeStyle.Singleton);
            container.Register<IEventPublisher, EventPublisher>(LifeStyle.Singleton);
            container.RegisterImplementationOf(getCurrentContext());
            container.Register<IHistoryManager, HistoryManager<MyContext>>();
            container.Register<IFullPathDisplayResolver, FullPathDisplayResolver>();
            container.Register<MyContext, MyContext>();
            container.Register<IConfigurableContainerLayoutView, AccordionLayoutView>();


            container.Register<DxContainer, DxContainer>(LifeStyle.Singleton);
            container.Register<RegionsContainer, RegionsContainer>(LifeStyle.Singleton);
         }


         serializerRegister.PerformMappingForSerializerIn(IoC.Container);
      }
   }
}