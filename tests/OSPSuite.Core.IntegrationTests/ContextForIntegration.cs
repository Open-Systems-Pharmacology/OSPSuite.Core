using System;
using System.IO;
using Castle.Facilities.TypedFactory;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.PKAnalyses;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Journal;
using OSPSuite.Core.Serialization;
using OSPSuite.Core.Serialization.Exchange;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Core.Services;
using OSPSuite.Helpers;
using OSPSuite.Infrastructure;
using OSPSuite.Infrastructure.Container.Castle;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Core.DataFormat;
using OSPSuite.Infrastructure.Import.Core.DataSourceFileReaders;
using OSPSuite.Infrastructure.Import.Core.Mappers;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Events;

namespace OSPSuite.Core
{
   [IntegrationTests]
   public abstract class ContextForIntegration<T> : ContextSpecification<T>
   {
      public override void GlobalContext()
      {
         if (IoC.Container != null) return;

         base.GlobalContext();
         var container = new CastleWindsorContainer();

         IoC.InitializeWith(container);
         IoC.RegisterImplementationOf(IoC.Container);

         container.WindsorContainer.AddFacility<TypedFactoryFacility>();
         container.Register<IObjectBaseFactory, ObjectBaseFactory>(LifeStyle.Singleton);
         container.Register<IDimensionFactory, DimensionFactoryForIntegrationTests>(LifeStyle.Singleton);
         container.Register<IGroupRepository, GroupRepositoryForSpecs>(LifeStyle.Singleton);
         container.Register<IDataRepositoryTask, DataRepositoryTask>();
         container.Register<IImporter, Importer>();
         container.Register<IDataSourceFileParser, DataSourceFileParser>();
         container.Register<ICsvDataSourceFile, CsvDataSourceFile>();
         container.Register<IImportLogger, ImportLogger>();
         container.Register<IExcelDataSourceFile, ExcelDataSourceFile>();
         container.Register<IDataSetToDataRepositoryMapper, DataSetToDataRepositoryMapper>();
         container.Register<IDataFormat, MixColumnsDataFormat>();

         container.Register<SimulationHelperForSpecs, SimulationHelperForSpecs>();
         container.Register<ModelHelperForSpecs, ModelHelperForSpecs>();
         container.Register<ModuleHelperForSpecs, ModuleHelperForSpecs>();
         container.Register<IDisplayNameProvider, DisplayNameProvider>();
         container.Register<ConcentrationBaseModelHelperForSpecs, ConcentrationBaseModelHelperForSpecs>();
         container.Register<IReactionDimensionRetriever, ReactionDimensionRetrieverForSpecs>(LifeStyle.Singleton);
         container.RegisterImplementationOf(A.Fake<IStartOptions>());

         container.RegisterImplementationOf(A.Fake<IApplicationController>());
         container.RegisterImplementationOf(A.Fake<IObjectTypeResolver>());
         container.RegisterImplementationOf(A.Fake<IDisplayUnitRetriever>());
         container.RegisterImplementationOf(A.Fake<IOSPSuiteExecutionContext>());
         container.RegisterImplementationOf(A.Fake<IProjectRetriever>());
         container.RegisterImplementationOf(A.Fake<IApplicationDiscriminator>());
         container.RegisterImplementationOf(A.Fake<IRelatedItemDescriptionCreator>());
         container.RegisterImplementationOf(A.Fake<IJournalDiagramManagerFactory>());
         container.RegisterImplementationOf(A.Fake<ICoreUserSettings>());
         container.RegisterImplementationOf(A.Fake<ICoreSimulationFactory>());
         container.RegisterImplementationOf(A.Fake<IFullPathDisplayResolver>());

         var applicationConfiguration = A.Fake<IApplicationConfiguration>();
         A.CallTo(() => applicationConfiguration.Product).Returns(Origins.Other);
         container.RegisterImplementationOf(applicationConfiguration);

         initGroupRepository();

         var progressManager = A.Fake<IProgressManager>();
         A.CallTo(() => progressManager.Create()).Returns(A.Fake<IProgressUpdater>());
         container.RegisterImplementationOf(progressManager);

         var csvSeparatorSelector = A.Fake<ICsvSeparatorSelector>();
         A.CallTo(() => csvSeparatorSelector.GetCsvSeparator(A<string>.Ignored)).Returns(new CSVSeparators { ColumnSeparator = ';', DecimalSeparator = '.' });
         container.RegisterImplementationOf(csvSeparatorSelector);


         using (container.OptimizeDependencyResolution())
         {
            container.AddRegister(x => x.FromType<CoreRegister>());
            container.AddRegister(x => x.FromType<InfrastructureRegister>());
            var register = new CoreSerializerRegister();
            container.AddRegister(x => x.FromInstance(register));
            register.PerformMappingForSerializerIn(container);
         }


         initializeDimensions();
         initPKParameters();

         Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
      }

      private static void initGroupRepository()
      {
         var groupRepository = IoC.Resolve<IGroupRepository>();
         var moBiGroup = new Group { Name = Constants.Groups.MOBI, Id = "1" };
         var undefinedGroup = new Group { Name = Constants.Groups.UNDEFINED, Id = "0" };
         var solverSettingsGroup = new Group { Name = Constants.Groups.SOLVER_SETTINGS, Id = "2" };
         groupRepository.AddGroup(moBiGroup);
         groupRepository.AddGroup(solverSettingsGroup);
         groupRepository.AddGroup(undefinedGroup);
      }

      private static void initializeDimensions()
      {
         var dimensionFactory = IoC.Resolve<IDimensionFactory>();
         var persistor = IoC.Resolve<IDimensionFactoryPersistor>();
         persistor.Load(dimensionFactory, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Constants.Files.DIMENSIONS_FILE_NAME));
         dimensionFactory.AddDimension(Constants.Dimension.NO_DIMENSION);

         var molarConcentrationDimension = dimensionFactory.Dimension(Constants.Dimension.MOLAR_CONCENTRATION);
         var massConcentrationDimension = dimensionFactory.Dimension(Constants.Dimension.MASS_CONCENTRATION);

         var concentrationDimensionsMergingInformation = new SimpleDimensionMergingInformation(molarConcentrationDimension, massConcentrationDimension);
         dimensionFactory.AddMergingInformation(concentrationDimensionsMergingInformation);
      }

      private void initPKParameters()
      {
         var pkParameterRepository = IoC.Resolve<IPKParameterRepository>();
         var pKParameterLoader = IoC.Resolve<IPKParameterRepositoryLoader>();
         pKParameterLoader.Load(pkParameterRepository, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Constants.Files.PK_PARAMETERS_FILE_NAME));
      }

      public override void GlobalCleanup()
      {
         base.GlobalCleanup();
         var withIdRepository = IoC.Resolve<IWithIdRepository>();
         withIdRepository.Clear();
      }
   }

   public abstract class ContextWithLoadedSimulation<T> : ContextForIntegration<T>
   {
      public SimulationTransfer LoadPKMLFile(string pkmlName)
      {
         var projectFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Data\\{pkmlName}.pkml");
         return SerializationHelperForSpecs.Load(projectFile);
      }
   }

   public abstract class ContextForModelConstructorIntegration : ContextWithLoadedSimulation<CreationResult>
   {
      public CreationResult CreateFrom(string pkmlName)
      {
         var simulationTransfer = LoadPKMLFile(pkmlName);
         var modelConstructor = IoC.Resolve<IModelConstructor>();
         var result = modelConstructor.CreateModelFrom(simulationTransfer.Simulation.Configuration, pkmlName);
         return result;
      }
   }
}