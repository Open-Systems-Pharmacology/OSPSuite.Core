using System;
using System.IO;
using Castle.Facilities.TypedFactory;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Converter.v5_2;
using OSPSuite.Core.Domain;
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
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Compression;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;

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
         container.Register<IDataNamingService, DataNamingServiceForSpecs>();
         container.Register<SimulationHelperForSpecs, SimulationHelperForSpecs>();
         container.Register<ModelHelperForSpecs, ModelHelperForSpecs>();
         container.Register<IDisplayNameProvider, DisplayNameProvider>();
         container.Register<ConcentrationBaseModelHelperForSpecs, ConcentrationBaseModelHelperForSpecs>();
         container.Register<IReactionDimensionRetriever, ReactionDimensionRetrieverForSpecs>(LifeStyle.Singleton);
         container.Register(typeof(IRepository<>), typeof(ImplementationRepository<>));
         container.RegisterImplementationOf(A.Fake<IStartOptions>());

         var stringCompression = A.Fake<IStringCompression>();
         A.CallTo(() => stringCompression.Compress(A<string>._)).ReturnsLazily(x => x.GetArgument<string>(0));
         A.CallTo(() => stringCompression.Decompress(A<string>._)).ReturnsLazily(x => x.GetArgument<string>(0));
         container.RegisterImplementationOf(stringCompression);

         container.RegisterImplementationOf(A.Fake<IObjectTypeResolver>());
         container.RegisterImplementationOf(A.Fake<IDisplayUnitRetriever>());
         container.RegisterImplementationOf(A.Fake<IOSPSuiteExecutionContext>());
         container.RegisterImplementationOf(A.Fake<IProjectRetriever>());
         container.RegisterImplementationOf(A.Fake<IApplicationDiscriminator>());
         container.RegisterImplementationOf(A.Fake<IRelatedItemDescriptionCreator>());
         container.RegisterImplementationOf(A.Fake<IJournalDiagramManagerFactory>());
         container.RegisterImplementationOf(A.Fake<ICoreUserSettings>());
         container.RegisterImplementationOf(A.Fake<ICoreSimulationFactory>());

         var applicationConfiguration = A.Fake<IApplicationConfiguration>();
         A.CallTo(() => applicationConfiguration.Product).Returns(Origins.Other);
         container.RegisterImplementationOf(applicationConfiguration);

         initGroupRepository();

         var progressManager = A.Fake<IProgressManager>();
         A.CallTo(() => progressManager.Create()).Returns(A.Fake<IProgressUpdater>());
         container.RegisterImplementationOf(progressManager);

         using (container.OptimizeDependencyResolution())
         {
            container.AddRegister(x => x.FromType<CoreRegister>());
            container.AddRegister(x => x.FromType<InfrastructureRegister>());
            var register = new CoreSerializerRegister();
            container.AddRegister(x => x.FromInstance(register));
            register.PerformMappingForSerializerIn(container);
         }


         initializeDimensions();

         Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
      }

      private static void initGroupRepository()
      {
         var groupRepository = IoC.Resolve<IGroupRepository>();
         var moBiGroup = new Group {Name = Constants.Groups.MOBI, Id = "1"};
         var undefinedGroup = new Group {Name = Constants.Groups.UNDEFINED, Id = "0"};
         var solverSettingsGroup = new Group {Name = Constants.Groups.SOLVER_SETTINGS, Id = "2"};
         groupRepository.AddGroup(moBiGroup);
         groupRepository.AddGroup(solverSettingsGroup);
         groupRepository.AddGroup(undefinedGroup);
      }

      private static void initializeDimensions()
      {
         var dimensionFactory = IoC.Resolve<IDimensionFactory>();
         var persistor = IoC.Resolve<IDimensionFactoryPersistor>();
         persistor.Load(dimensionFactory, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "OSPSuite.Dimensions.xml"));
         dimensionFactory.AddDimension(Constants.Dimension.NO_DIMENSION);
         var dimensionMapper = IoC.Resolve<IDimensionMapper>();
         dimensionMapper.DummyDimensionsForConversion.Each(dimensionFactory.AddDimension);

         var molarConcentrationDimension = dimensionFactory.Dimension(Constants.Dimension.MOLAR_CONCENTRATION);
         var massConcentrationDimension = dimensionFactory.Dimension(Constants.Dimension.MASS_CONCENTRATION);

         var concentrationDimensionsMergingInformation = new SimpleDimensionMergingInformation(molarConcentrationDimension, massConcentrationDimension);
         dimensionFactory.AddMergingInformation(concentrationDimensionsMergingInformation);
      }

      public override void GlobalCleanup()
      {
         base.GlobalCleanup();
         var withIdRepository = IoC.Resolve<IWithIdRepository>();
         withIdRepository.Clear();
      }
   }

   public abstract class ContextForModelConstructorIntegration : ContextForIntegration<CreationResult>
   {
      public SimulationTransfer LoadPKMLFile(string pkmlName)
      {
         var projectFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Data\\{pkmlName}.pkml");
         return SerializationHelperForSpecs.Load(projectFile);
      }

      public CreationResult CreateFrom(string pkmlName)
      {
         var simulationTransfer = LoadPKMLFile(pkmlName);
         var modelConstructor = IoC.Resolve<IModelConstructor>();
         var result = modelConstructor.CreateModelFrom(simulationTransfer.Simulation.BuildConfiguration, pkmlName);
         return result;
      }
   }
}