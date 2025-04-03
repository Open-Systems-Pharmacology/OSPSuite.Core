using System;
using System.IO;
using System.Threading.Tasks;
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
      private readonly BaseContextForIntegration _baseContext = new BaseContextForIntegration();

      public override void GlobalContext()
      {
         if (IoC.Container != null) return;

         base.GlobalContext();
         _baseContext.InitializeContainer();
      }

      public override void GlobalCleanup()
      {
         base.GlobalCleanup();
         _baseContext.GlobalCleanup();
      }
   }

   [IntegrationTests]
   public abstract class ContextForIntegrationAsync<T> : ContextSpecificationAsync<T>
   {
      private readonly BaseContextForIntegration _baseContext = new BaseContextForIntegration();

      public override async Task GlobalContext()
      {
         await base.GlobalContext();
         if (IoC.Container != null) return;
         _baseContext.InitializeContainer();
      }

      public override async Task GlobalCleanup()
      {
         await base.GlobalCleanup();
         _baseContext.GlobalCleanup();
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

   public class BaseContextForIntegration
   {
      public void InitializeContainer()
      {
         var container = new CastleWindsorContainer();
         RegisterComponents(container);
         InitGroupRepository();
         InitializeDimensions();
         InitPKParameters();
         Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
      }

      public void RegisterComponents(CastleWindsorContainer container)
      {
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

         var progressManager = A.Fake<IProgressManager>();
         A.CallTo(() => progressManager.Create()).Returns(A.Fake<IProgressUpdater>());
         container.RegisterImplementationOf(progressManager);

         var csvSeparatorSelector = A.Fake<ICsvSeparatorSelector>();
         A.CallTo(() => csvSeparatorSelector.GetCsvSeparator(A<string>.Ignored))
            .Returns(new CSVSeparators { ColumnSeparator = ';', DecimalSeparator = '.' });
         container.RegisterImplementationOf(csvSeparatorSelector);

         using (container.OptimizeDependencyResolution())
         {
            container.AddRegister(x => x.FromType<CoreRegister>());
            container.AddRegister(x => x.FromType<InfrastructureRegister>());
            var register = new CoreSerializerRegister();
            container.AddRegister(x => x.FromInstance(register));
            register.PerformMappingForSerializerIn(container);
         }
      }

      public void InitGroupRepository()
      {
         var groupRepository = IoC.Resolve<IGroupRepository>();
         groupRepository.AddGroup(new Group { Name = Constants.Groups.MOBI, Id = "1" });
         groupRepository.AddGroup(new Group { Name = Constants.Groups.UNDEFINED, Id = "0" });
         groupRepository.AddGroup(new Group { Name = Constants.Groups.SOLVER_SETTINGS, Id = "2" });
      }

      public void InitializeDimensions()
      {
         var dimensionFactory = IoC.Resolve<IDimensionFactory>();
         var persistor = IoC.Resolve<IDimensionFactoryPersistor>();
         persistor.Load(dimensionFactory, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Constants.Files.DIMENSIONS_FILE_NAME));
         dimensionFactory.AddDimension(Constants.Dimension.NO_DIMENSION);

         var molarConcentrationDimension = dimensionFactory.Dimension(Constants.Dimension.MOLAR_CONCENTRATION);
         var massConcentrationDimension = dimensionFactory.Dimension(Constants.Dimension.MASS_CONCENTRATION);
         dimensionFactory.AddMergingInformation(new SimpleDimensionMergingInformation(molarConcentrationDimension, massConcentrationDimension));
      }

      public void InitPKParameters()
      {
         var pkParameterRepository = IoC.Resolve<IPKParameterRepository>();
         var pKParameterLoader = IoC.Resolve<IPKParameterRepositoryLoader>();
         pKParameterLoader.Load(pkParameterRepository, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Constants.Files.PK_PARAMETERS_FILE_NAME));
      }

      public void GlobalCleanup()
      {
         var withIdRepository = IoC.Resolve<IWithIdRepository>();
         withIdRepository.Clear();
      }
   }
}