using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Comparison;
using OSPSuite.Core.Converters;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.ParameterIdentifications.Algorithms;
using OSPSuite.Core.Domain.PKAnalyses;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Core.Domain.Services.SensitivityAnalyses;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Container;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.Core
{
   public class CoreRegister : Register
   {
      /// <summary>
      ///    Specifies if the parameter objects should also be registered in the container
      ///    Default value is true
      /// </summary>
      public bool RegisterParameter { get; set; }

      public CoreRegister()
      {
         RegisterParameter = true;
      }

      public override void RegisterInContainer(IContainer container)
      {
         //REGISTER DOMAIN OBJECTS
         container.AddScanner(scan =>
         {
            scan.AssemblyContainingType<CoreRegister>();

            scan.ExcludeNamespaceContainingType<IDiffBuilder>();
            scan.ExcludeNamespaceContainingType<IObjectConverter>();

            //special registration handled by other registers or conventions
            scan.ExcludeNamespaceContainingType<CoreSerializerRegister>();
            scan.ExcludeNamespaceContainingType<IOptimizationAlgorithm>();

            //should be registered as singleton explicitly
            scan.ExcludeType<ObjectBaseFactory>();
            scan.ExcludeType<WithIdRepository>();
            scan.ExcludeType<DimensionFactory>();
            scan.ExcludeType<PKParameterRepository>();
            scan.ExcludeType<CoreCalculationMethodRepository>();
            scan.ExcludeType<PKParameterRepositoryLoader>();
            scan.ExcludeType<ParameterIdentificationRunner>();
            scan.ExcludeType<SensitivityAnalysisRunner>();
            scan.ExcludeType<ConfirmationManager>();

            scan.ExcludeType(typeof(ExtendedProperty<>));
            scan.ExcludeType(typeof(HistoryManager<>));
            scan.ExcludeType(typeof(MacroCommand<>));
            scan.ExcludeType(typeof(RollBackCommand<>));

            //Exclude these implementations that are specific to each application
            scan.ExcludeType<ObjectIdResetter>();
            scan.ExcludeType<DisplayNameProvider>();
            scan.ExcludeType<FullPathDisplayResolver>();
            scan.ExcludeType<PathToPathElementsMapper>();
            scan.ExcludeType<QuantityPathToQuantityDisplayPathMapper>();
            scan.ExcludeType<DataColumnToPathElementsMapper>();

            if (!RegisterParameter)
            {
               scan.ExcludeType<Parameter>();
               scan.ExcludeType<DistributedParameter>();
            }

            scan.WithConvention(new OSPSuiteRegistrationConvention(registerConcreteType: true));
         });

         container.Register<IModelCircularReferenceChecker, CircularReferenceChecker>();
         container.Register(typeof(IHistoryManager<>), typeof(HistoryManager<>));

         //this should be registered as singleton
         container.Register<IWithIdRepository, WithIdRepository>(LifeStyle.Singleton);
         container.Register<IDiffBuilderRepository, DiffBuilderRepository>(LifeStyle.Singleton);
         container.Register<IPKParameterRepository, PKParameterRepository>(LifeStyle.Singleton);
         container.Register<ICoreCalculationMethodRepository, CoreCalculationMethodRepository>(LifeStyle.Singleton);
         container.Register<IPKParameterRepositoryLoader, PKParameterRepositoryLoader>(LifeStyle.Singleton);
         container.Register<IParameterIdentificationRunner, ParameterIdentificationRunner>(LifeStyle.Singleton);
         container.Register<ISensitivityAnalysisRunner, SensitivityAnalysisRunner>(LifeStyle.Singleton);
         container.Register<IConfirmationManager, ConfirmationManager>(LifeStyle.Singleton);

         //Register specifically since this object does not implement ANY interface besides its own
         container.Register<SimulationBuilder, SimulationBuilder>();

         registerComparers(container);

         registerConverters(container);

         registerParameterIdentificationRunFactories(container);

         registerThirdPartyComponents(container);

         container.RegisterFactory<IStartableProcessFactory>();

         //Register Optimization algorithm explicitly
         container.Register<IOptimizationAlgorithm, NelderMeadOptimizer>(Constants.OptimizationAlgorithm.NELDER_MEAD_PKSIM);
         container.Register<IOptimizationAlgorithm, MPFitLevenbergMarquardtOptimizer>(Constants.OptimizationAlgorithm.MPFIT);
         container.Register<IOptimizationAlgorithm, MonteCarloOptimizer>(Constants.OptimizationAlgorithm.MONTE_CARLO);
      }

      private static void registerConverters(IContainer container)
      {
         container.AddScanner(scan =>
         {
            scan.AssemblyContainingType<CoreRegister>();
            scan.IncludeNamespaceContainingType<IObjectConverter>();
            scan.WithConvention<RegisterTypeConvention<IObjectConverter>>();
            scan.ExcludeType<ObjectConverterFinder>();
         });
         container.Register<IObjectConverterFinder, ObjectConverterFinder>(LifeStyle.Singleton);
      }

      private static void registerComparers(IContainer container)
      {
         container.AddScanner(scan =>
         {
            scan.AssemblyContainingType<CoreRegister>();
            scan.IncludeNamespaceContainingType<IDiffBuilder>();
            scan.ExcludeType<DiffBuilderRepository>();
            scan.ExcludeType(typeof(Comparison<>));
            scan.WithConvention<RegisterTypeConvention<IDiffBuilder>>();
         });
         container.Register(typeof(WithValueOriginComparison<>), typeof(WithValueOriginComparison<>));
         container.Register(typeof(IComparison<>), typeof(Comparison<>));
         container.Register<EnumerableComparer, EnumerableComparer>();
      }

      private static void registerParameterIdentificationRunFactories(IContainer container)
      {
         container.Register<IParameterIdentificationRunSpecificationFactory, CategorialParameterIdentificationRunFactory>();
         container.Register<IParameterIdentificationRunSpecificationFactory, MultipleParameterIdentificationRunFactory>();
         container.Register<IParameterIdentificationRunSpecificationFactory, StandardParameterIdentificationRunFactory>();
      }

      private static void registerThirdPartyComponents(IContainer container)
      {
         container.Register(typeof(IRepository<>), typeof(ImplementationRepository<>));
      }
   }
}