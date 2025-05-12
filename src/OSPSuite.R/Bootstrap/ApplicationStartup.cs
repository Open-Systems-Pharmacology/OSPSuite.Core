using System;
using System.Threading;
using OSPSuite.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.PKAnalyses;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Infrastructure;
using OSPSuite.Infrastructure.Container.Autofac;
using OSPSuite.Infrastructure.Import;
using OSPSuite.R.Domain.UnitSystem;
using OSPSuite.R.MinimalImplementations;
using OSPSuite.Utility.Container;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.R.Bootstrap
{
   internal class ApplicationStartup
   {
      private static IContainer _container;

      public static IContainer Initialize(ApiConfig apiConfig, Action<IContainer> registerAction = null)
      {
         if (_container != null)
            return _container;

         _container = new ApplicationStartup().performInitialization(apiConfig, registerAction);

         return _container;
      }

      private IContainer performInitialization(ApiConfig apiConfig, Action<IContainer> registerAction)
      {
         var container = new AutofacContainer();

         container.RegisterImplementationOf((IContainer)container);

         var serializerRegister = new CoreSerializerRegister();

         using (container.OptimizeDependencyResolution())
         {
            if (registerAction != null)
               registerAction(container);

            container.RegisterImplementationOf(new SynchronizationContext());
            container.AddRegister(x => x.FromType<CoreRegister>());
            container.AddRegister(x => x.FromType<InfrastructureRegister>());
            container.AddRegister(x => x.FromType<InfrastructureImportRegister>());
            container.AddRegister(x => x.FromType<RRegister>());

            registerCoreDependencies(container);

            container.AddRegister(x => x.FromInstance(serializerRegister));
         }

         serializerRegister.PerformMappingForSerializerIn(container);

         initializeConfiguration(container, apiConfig);

         initializeDimensions(container);

         loadPKParameterRepository(container);

         return container;
      }

      private static void registerCoreDependencies(IContainer container)
      {
         container.Register<IObjectBaseFactory, ObjectBaseFactory>(LifeStyle.Singleton);
         container.Register<IApplicationConfiguration, RConfiguration>(LifeStyle.Singleton);
         container.Register<IDimensionFactory, RDimensionFactory>(LifeStyle.Singleton);
         container.Register<IFullPathDisplayResolver, FullPathDisplayResolver>();
         container.Register<IPathToPathElementsMapper, PathToPathElementsMapper>();
         container.Register<IQuantityPathToQuantityDisplayPathMapper, RQuantityPathToQuantityDisplayPathMapper>();
         container.Register<IDataColumnToPathElementsMapper, DataColumnToPathElementsMapper>();
         container.Register<IObjectIdResetter, ObjectIdResetter>();
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
         var amountDimension = dimensionFactory.Dimension(Constants.Dimension.MOLAR_AMOUNT);
         var massDimension = dimensionFactory.Dimension(Constants.Dimension.MASS_AMOUNT);
         var aucMolarDimension = dimensionFactory.Dimension(Constants.Dimension.MOLAR_AUC);
         var aucMassDimension = dimensionFactory.Dimension(Constants.Dimension.MASS_AUC);

         dimensionFactory.AddMergingInformation(new SimpleDimensionMergingInformation(molarConcentrationDimension, massConcentrationDimension));
         dimensionFactory.AddMergingInformation(new SimpleDimensionMergingInformation(massConcentrationDimension, molarConcentrationDimension));
         dimensionFactory.AddMergingInformation(new SimpleDimensionMergingInformation(amountDimension, massDimension));
         dimensionFactory.AddMergingInformation(new SimpleDimensionMergingInformation(massDimension, amountDimension));
         dimensionFactory.AddMergingInformation(new SimpleDimensionMergingInformation(aucMolarDimension, aucMassDimension));
         dimensionFactory.AddMergingInformation(new SimpleDimensionMergingInformation(aucMassDimension, aucMolarDimension));
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