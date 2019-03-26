using OSPSuite.Core.Chart;
using OSPSuite.Core.Comparison;
using OSPSuite.Core.Converter;
using OSPSuite.Core.Converter.v5_2;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.ParameterIdentifications.Algorithms;
using OSPSuite.Core.Domain.PKAnalyses;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Core.Domain.Services.SensitivityAnalyses;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization;
using OSPSuite.Core.Serialization.SimModel.Services;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Container;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.Core
{
   public class CoreRegister : Register
   {
      /// <summary>
      ///    Specifies if the parameter objects should also be registerd in the container
      ///    Default value is true
      /// </summary>
      public bool RegisterParameter { get; set; }

      public CoreRegister()
      {
         RegisterParameter = true;
      }

      public override void RegisterInContainer(IContainer container)
      {
         //REGISTSTER DOMAIN OBJECTS
         container.AddScanner(scan =>
         {
            scan.AssemblyContainingType<CoreRegister>();

            scan.ExcludeNamespaceContainingType<IDiffBuilder>();
            scan.ExcludeNamespaceContainingType<IObjectConverter>();

            //sepcial registration handled by other registers or conventions
            scan.ExcludeNamespaceContainingType<CoreSerializerRegister>();
            scan.ExcludeNamespaceContainingType<IOptimizationAlgorithm>();

            //should be registered as singleton explicitely
            scan.ExcludeType<ObjectBaseFactory>();
            scan.ExcludeType<WithIdRepository>();
            scan.ExcludeType<DimensionFactory>();
            scan.ExcludeType<PKParameterRepository>();
            scan.ExcludeType<CoreCalculationMethodRepository>();
            scan.ExcludeType<PKParameterRepositoryLoader>();
            scan.ExcludeType<ParameterIdentificationRunner>();
            scan.ExcludeType<SensitivityAnalysisRunner>();

            //PK-Sim registers its own implementation
            scan.ExcludeType<ObjectIdResetter>();

            if (!RegisterParameter)
            {
               scan.ExcludeType<Parameter>();
               scan.ExcludeType<DistributedParameter>();
            }

            scan.WithConvention(new OSPSuiteRegistrationConvention(registerConcreteType: true));
         });

         //this should be registered as singleton
         container.Register<IWithIdRepository, WithIdRepository>(LifeStyle.Singleton);
         container.Register<IDiffBuilderRepository, DiffBuilderRepository>(LifeStyle.Singleton);
         container.Register<IPKParameterRepository, PKParameterRepository>(LifeStyle.Singleton);
         container.Register<ICoreCalculationMethodRepository, CoreCalculationMethodRepository>(LifeStyle.Singleton);
         container.Register<IPKParameterRepositoryLoader, PKParameterRepositoryLoader>(LifeStyle.Singleton);
         container.Register<IParameterIdentificationRunner, ParameterIdentificationRunner>(LifeStyle.Singleton);
         container.Register<ISensitivityAnalysisRunner, SensitivityAnalysisRunner>(LifeStyle.Singleton);

         registerComparers(container);

         reigsterConverters(container);

         registerParameterIdentificationRunFactories(container);

         //FACTORIES
         container.RegisterFactory<IDiagramModelFactory>();
         container.RegisterFactory<IModelValidatorFactory>();
         container.RegisterFactory<IParameterIdentificationEngineFactory>();
         container.RegisterFactory<ISimModelBatchFactory>();
         container.RegisterFactory<IParameterIdentificationRunInitializerFactory>();
         container.RegisterFactory<ISensitivityAnalysisEngineFactory>();
         container.RegisterFactory<IStartableProcessFactory>();
         container.RegisterFactory<ISimulationExportCreatorFactory>();

         //Register Optimization algorithm explicitely
         container.Register<IOptimizationAlgorithm, NelderMeadOptimizer>(Constants.OptimizationAlgorithm.NELDER_MEAD_PKSIM);
         container.Register<IOptimizationAlgorithm, MPFitLevenbergMarquardtOptimizer>(Constants.OptimizationAlgorithm.MPFIT);
         container.Register<IOptimizationAlgorithm, MonteCarloOptimizer>(Constants.OptimizationAlgorithm.MONTE_CARLO);
      }

      private static void reigsterConverters(IContainer container)
      {
         container.AddScanner(scan =>
         {
            scan.AssemblyContainingType<CoreRegister>();
            scan.IncludeNamespaceContainingType<IObjectConverter>();
            scan.WithConvention<RegisterTypeConvention<IObjectConverter>>();
            scan.ExcludeType<ObjectConverterFinder>();
            scan.ExcludeType<DimensionMapper>();
            scan.ExcludeType<FormulaMapper>();
            scan.ExcludeType<SerializationDimensionFactory>();
         });
         container.Register<IObjectConverterFinder, ObjectConverterFinder>(LifeStyle.Singleton);
         container.Register<IDimensionMapper, DimensionMapper>(LifeStyle.Singleton);
         container.Register<IFormulaMapper, FormulaMapper>(LifeStyle.Singleton);
         container.Register<ISerializationDimensionFactory, SerializationDimensionFactory>(LifeStyle.Singleton);
      }

      private static void registerComparers(IContainer container)
      {
         container.AddScanner(scan =>
         {
            scan.AssemblyContainingType<CoreRegister>();
            scan.IncludeNamespaceContainingType<IDiffBuilder>();
            scan.ExcludeType<DiffBuilderRepository>();
            scan.WithConvention<RegisterTypeConvention<IDiffBuilder>>();
         });
         container.Register(typeof(WithValueOriginComparison<>), typeof(WithValueOriginComparison<>));
         container.Register<EnumerableComparer, EnumerableComparer>();
      }

      private static void registerParameterIdentificationRunFactories(IContainer container)
      {
         container.Register<IParameterIdentificationRunSpecificationFactory, CategorialParameterIdentificationRunFactory>();
         container.Register<IParameterIdentificationRunSpecificationFactory, MultipleParameterIdentificationRunFactory>();
         container.Register<IParameterIdentificationRunSpecificationFactory, StandardParameterIdentificationRunFactory>();
      }
   }
}