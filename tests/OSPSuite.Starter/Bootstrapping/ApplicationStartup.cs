using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Autofac;
using Castle.Facilities.TypedFactory;
using DevExpress.LookAndFeel;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraTabbedMdi;
using OSPSuite.Core;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.PKAnalyses;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Core.Services;
using OSPSuite.Helpers;
using OSPSuite.Infrastructure;
using OSPSuite.Infrastructure.Container.Autofac;
using OSPSuite.Infrastructure.Container.Castle;
using OSPSuite.Infrastructure.Export;
using OSPSuite.Infrastructure.Serialization;
using OSPSuite.Presentation;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Importer;
using OSPSuite.Presentation.Importer;
using OSPSuite.Presentation.Regions;
using OSPSuite.Starter.Presenters;
using OSPSuite.Starter.Services;
using OSPSuite.UI;
using OSPSuite.UI.Controls;
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
//         var container = new AutofacContainer();
//         container.AddActivationHook<EventRegistrationHook>();
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
            container.AddRegister(x => x.FromType<PresentationImporterRegister>());
            container.AddRegister(x => x.FromType<InfrastructureSerializationRegister>());
            container.AddRegister(x => x.FromInstance(serializerRegister));

            container.Register<IDimensionFactory, DimensionFactory>(LifeStyle.Singleton);
            container.Register<IApplicationController, ApplicationController>(LifeStyle.Singleton);
            container.Register<IEventPublisher, EventPublisher>(LifeStyle.Singleton);
            container.RegisterImplementationOf(getCurrentContext());
            container.Register<IHistoryManager, HistoryManager<MyContext>>();
            container.Register<IFullPathDisplayResolver, FullPathDisplayResolver>();
            container.Register<MyContext, MyContext>();


            container.Register<DxContainer, DxContainer>(LifeStyle.Singleton);
//            container.AutofacBuilder.Register(c => c.Resolve<DxContainer>().ApplicationMenu).As<ApplicationMenu>();
//            container.AutofacBuilder.Register(c => c.Resolve<DxContainer>().BarManager).As<BarManager>();
//            container.AutofacBuilder.Register(c => c.Resolve<DxContainer>().PanelControl).As<PanelControl>();
//            container.AutofacBuilder.Register(c => c.Resolve<DxContainer>().RibbonControl).As<RibbonControl>();
//            container.AutofacBuilder.Register(c => c.Resolve<DxContainer>().RibbonBarManager).As<RibbonBarManager>();
//            container.AutofacBuilder.Register(c => c.Resolve<DxContainer>().UserLookAndFeel).As<UserLookAndFeel>();
//            container.AutofacBuilder.Register(c => c.Resolve<DxContainer>().XtraTabbedMdiManager).As<XtraTabbedMdiManager>();

            container.Register<RegionsContainer, RegionsContainer>(LifeStyle.Singleton);
//            container.AutofacBuilder.Register(c => c.Resolve<RegionsContainer>().Journal).Keyed<IRegion>(RegionNames.Journal.Name);
//            container.AutofacBuilder.Register(c => c.Resolve<RegionsContainer>().Comparison).Keyed<IRegion>(RegionNames.Comparison.Name);
//            container.AutofacBuilder.Register(c => c.Resolve<RegionsContainer>().Explorer).Keyed<IRegion>(RegionNames.Explorer.Name);
         }


         serializerRegister.PerformMappingForSerializerIn(IoC.Container);
      }
   }
}