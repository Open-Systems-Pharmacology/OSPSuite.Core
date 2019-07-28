using System;
using System.Threading;
using OSPSuite.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Infrastructure;
using OSPSuite.Infrastructure.Container.Castle;
using OSPSuite.R.MinimalImplementations;
using OSPSuite.Utility.Container;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.R.Bootstrap
{
   internal class ApplicationStartup
   {
      private static bool _initialized;

      public static void Initialize(ApiConfig apiConfig)
      {
         if (_initialized) return;

         new ApplicationStartup().performInitialization(apiConfig);

         _initialized = true;
      }

      private void performInitialization(ApiConfig apiConfig)
      {
         if (IoC.Container != null)
            return;

         var container = new CastleWindsorContainer();

         IoC.InitializeWith(container);
         IoC.RegisterImplementationOf(IoC.Container);


         using (container.OptimizeDependencyResolution())
         {
            container.RegisterImplementationOf(new SynchronizationContext());
            container.AddRegister(x => x.FromType<CoreRegister>());
            container.AddRegister(x => x.FromType<InfrastructureRegister>());
            container.AddRegister(x => x.FromType<RRegister>());

            initializeGroups(container);

            var register = new CoreSerializerRegister();
            container.AddRegister(x => x.FromInstance(register));
            register.PerformMappingForSerializerIn(container);

            registerCoreDependencies(container);
         }

         initializeConfiguration(container, apiConfig);

         initializeDimensions(container);
      }

      private void initializeConfiguration(IContainer container, ApiConfig apiConfig)
      {
         var applicationConfiguration = container.Resolve<IApplicationConfiguration>();
         applicationConfiguration.PKParametersFilePath = apiConfig.PKParametersFilePath;
         applicationConfiguration.DimensionFilePath = apiConfig.DimensionFilePath;
      }

      private static void registerCoreDependencies(IContainer container)
      {
         container.Register<IObjectBaseFactory, ObjectBaseFactory>(LifeStyle.Singleton);
         container.Register<IApplicationConfiguration, RConfiguration>(LifeStyle.Singleton);
      }

      private static void initializeDimensions(IContainer container)
      {
         container.Register<IDimensionFactory, DimensionFactory>(LifeStyle.Singleton);
         var applicationConfiguration = container.Resolve<IApplicationConfiguration>();
         var dimensionFactory = container.Resolve<IDimensionFactory>();
         var persistor = container.Resolve<IDimensionFactoryPersistor>();
         persistor.Load(dimensionFactory, applicationConfiguration.DimensionFilePath);
         dimensionFactory.AddDimension(Constants.Dimension.NO_DIMENSION);

         var molarConcentrationDimension = dimensionFactory.Dimension(Constants.Dimension.MOLAR_CONCENTRATION);
         var massConcentrationDimension = dimensionFactory.Dimension(Constants.Dimension.MASS_CONCENTRATION);

         var concentrationDimensionsMergingInformation = new SimpleDimensionMergingInformation(molarConcentrationDimension, massConcentrationDimension);
         dimensionFactory.AddMergingInformation(concentrationDimensionsMergingInformation);
      }

      private static void initializeGroups(IContainer container)
      {
         container.Register<IGroupRepository, GroupRepository>(LifeStyle.Singleton);
         var groupRepository = container.Resolve<IGroupRepository>();
         var undefinedGroup = new Group {Name = Constants.Groups.UNDEFINED, Id = "0"};
         groupRepository.AddGroup(undefinedGroup);
      }
   }
}