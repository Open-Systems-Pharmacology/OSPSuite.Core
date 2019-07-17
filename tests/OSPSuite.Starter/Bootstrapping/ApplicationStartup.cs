using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Castle.Facilities.TypedFactory;
using OSPSuite.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.PKAnalyses;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure;
using OSPSuite.Infrastructure.Container.Castle;
using OSPSuite.Infrastructure.Export;
using OSPSuite.Infrastructure.Services;
using OSPSuite.Presentation;
using OSPSuite.Presentation.Core;
using OSPSuite.Starter.Services;
using OSPSuite.UI;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Compression;
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
         //TODO XMLSchemaCache.InitializeFromFile("./OSPSuite.SimModel.xsd");
         fillDimensions(IoC.Resolve<IDimensionFactory>());
         loadPKParameterRepository(IoC.Container);

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

         container.RegisterImplementationOf((IContainer) container);
         container.WindsorContainer.AddFacility<EventRegisterFacility>();
         container.WindsorContainer.AddFacility<TypedFactoryFacility>();

         container.Register<IObjectBaseFactory, ObjectBaseFactory>(LifeStyle.Singleton);
         container.AddRegister(x => x.FromType<PresenterRegister>());
         container.AddRegister(x => x.FromType<UIRegister>());
         container.AddRegister(x => x.FromType<CoreRegister>());
         container.AddRegister(x => x.FromType<TestRegister>());
         container.AddRegister(x => x.FromType<InfrastructureRegister>());
         container.AddRegister(x => x.FromType<InfrastructureExportRegister>());
         container.AddRegister(x => x.FromType<UIImporterRegister>());
         container.AddRegister(x => x.FromType<PresentationImporterRegister>());
         container.AddRegister(x => x.FromType<InfrastructureSerializationRegister>());

         container.Register<IDimensionFactory, DimensionFactory>(LifeStyle.Singleton);
         container.Register<IApplicationController, ApplicationController>(LifeStyle.Singleton);
         container.Register<ICompression, SharpLibCompression>();
         container.Register<IEventPublisher, EventPublisher>(LifeStyle.Singleton);
         container.Register<IStringCompression, StringCompression>(LifeStyle.Singleton);
         container.Register<ILogger, OSPLogger>(LifeStyle.Singleton);
         container.RegisterImplementationOf(getCurrentContext());

         container.Register(typeof(IRepository<>), typeof(ImplementationRepository<>));

         IoC.InitializeWith(container);

         var register = new CoreSerializerRegister();
         IoC.Container.AddRegister(x => x.FromInstance(register));
         register.PerformMappingForSerializerIn(IoC.Container);
      }
   }
}