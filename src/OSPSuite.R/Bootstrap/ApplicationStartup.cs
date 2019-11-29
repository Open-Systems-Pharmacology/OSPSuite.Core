using System.Threading;
using OSPSuite.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.PKAnalyses;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Infrastructure;
using OSPSuite.Infrastructure.Container.Autofac;
using OSPSuite.Infrastructure.Import;
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

         var container = new AutofacContainer();

         IoC.InitializeWith(container);
         IoC.RegisterImplementationOf(IoC.Container);

         var serializerRegister = new CoreSerializerRegister();

         using (container.OptimizeDependencyResolution())
         {
            container.RegisterImplementationOf(new SynchronizationContext());
            container.AddRegister(x => x.FromType<CoreRegister>());
            container.AddRegister(x => x.FromType<InfrastructureRegister>());
            container.AddRegister(x => x.FromType<InfrastructureImportRegister>());
            container.AddRegister(x => x.FromType<RRegister>());

            registerCoreDependencies(container);

            container.AddRegister(x => x.FromInstance(serializerRegister));
         }

         serializerRegister.PerformMappingForSerializerIn(container);

         initializeGroups(container);
         initializeConfiguration(container, apiConfig);

         initializeDimensions(container);

         loadPKParameterRepository(container);
      }

      private static void registerCoreDependencies(IContainer container)
      {
         container.Register<IObjectBaseFactory, ObjectBaseFactory>(LifeStyle.Singleton);
         container.Register<IApplicationConfiguration, RConfiguration>(LifeStyle.Singleton);
         container.Register<IDimensionFactory, DimensionFactory>(LifeStyle.Singleton);
         container.Register<IFullPathDisplayResolver, FullPathDisplayResolver>(LifeStyle.Singleton);
      }

      private void initializeConfiguration(IContainer container, ApiConfig apiConfig)
      {
         var applicationConfiguration = container.Resolve<IApplicationConfiguration>();
         applicationConfiguration.PKParametersFilePath = apiConfig.PKParametersFilePath;
         applicationConfiguration.DimensionFilePath = apiConfig.DimensionFilePath;
      }

      private static void initializeDimensions(IContainer container)
      {
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
         var groupRepository = container.Resolve<IGroupRepository>();
         var undefinedGroup = new Group {Name = Constants.Groups.UNDEFINED, Id = "0"};
         groupRepository.AddGroup(undefinedGroup);
      }

      private static void loadPKParameterRepository(IContainer container)
      {
         var pkParameterRepository = container.Resolve<IPKParameterRepository>();
         var pKParameterLoader = container.Resolve<IPKParameterRepositoryLoader>();
         var configuration = container.Resolve<IApplicationConfiguration>();
         pKParameterLoader.Load(pkParameterRepository, configuration.PKParametersFilePath);
      }
   }
}