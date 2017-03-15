using System;
using System.IO;
using System.Threading;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Compression;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Events;
using Castle.Facilities.TypedFactory;
using OSPSuite.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Infrastructure;
using OSPSuite.Infrastructure.Container.Castle;
using OSPSuite.Infrastructure.Services;
using OSPSuite.Presentation;
using OSPSuite.Presentation.Core;
using OSPSuite.UI;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.Starter.Bootstrapping
{
   public class ApplicationStartup
   {
      public static void Initialize()
      {
         initializeDependency();
         fillDimensions(IoC.Resolve<IDimensionFactory>());
      }

      private static void fillDimensions(IDimensionFactory dimensionFactory)
      {
         var persistor = IoC.Resolve<IDimensionFactoryPersistor>();
         persistor.Load(dimensionFactory, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "OSPSuite.Dimensions.xml"));
         dimensionFactory.AddDimension(Constants.Dimension.NO_DIMENSION);
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

         container.Register<IDimensionFactory, DimensionFactory>(LifeStyle.Singleton);
         container.Register<IApplicationController, ApplicationController>(LifeStyle.Singleton);
         container.Register<ICompression, SharpLibCompression>();
         container.Register<IEventPublisher, EventPublisher>(LifeStyle.Singleton);
         container.Register<IStringCompression, StringCompression>(LifeStyle.Singleton);
         container.RegisterImplementationOf(new SynchronizationContext());

         container.Register(typeof(IRepository<>), typeof(ImplementationRepository<>));

         IoC.InitializeWith(container);

         var register = new CoreSerializerRegister();
         IoC.Container.AddRegister(x => x.FromInstance(register));
         register.PerformMappingForSerializerIn(IoC.Container);
      }
   }
}