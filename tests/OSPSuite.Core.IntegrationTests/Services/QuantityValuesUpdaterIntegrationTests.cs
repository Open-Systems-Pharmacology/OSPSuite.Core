using System.Linq;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Helpers;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using static OSPSuite.Core.Domain.Constants.Distribution;
using IContainer = OSPSuite.Core.Domain.IContainer;

namespace OSPSuite.Core.Services
{
   internal abstract class concern_for_QuantityValuesUpdaterIntegration : ContextForIntegration<IQuantityValuesUpdater>
   {
      protected ISpatialStructureFactory _spatialStructureFactory;
      protected IObjectBaseFactory _objectBaseFactory;
      protected ModelHelperForSpecs _modelHelper;
      protected SpatialStructure _spatialStructure;
      protected IModel _model;
      protected IContainer _organism;
      protected IContainer _liver;
      protected IParameter _parameter;
      protected SimulationConfiguration _simulationConfiguration;
      protected SimulationBuilder _simulationBuilder;
      protected Module _module;
      protected ParameterValuesBuildingBlock _parameterValues;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _spatialStructureFactory = IoC.Resolve<ISpatialStructureFactory>();
         _objectBaseFactory = IoC.Resolve<IObjectBaseFactory>();
         _modelHelper = IoC.Resolve<ModelHelperForSpecs>();
         _simulationConfiguration = new SimulationConfiguration();
         _parameterValues = new ParameterValuesBuildingBlock();

         _spatialStructure = _spatialStructureFactory.Create();
         _organism = _objectBaseFactory.Create<IContainer>()
            .WithName("Organism")
            .WithMode(ContainerMode.Logical);

         _liver = _objectBaseFactory.Create<IContainer>()
            .WithName("Liver")
            .WithMode(ContainerMode.Physical);

         _parameter = _modelHelper.NewConstantParameter("Param", 10);
         _liver.Add(_parameter);

         _organism.Add(_liver);

         _model = new Model();
         _model.Root = _organism;

         _simulationConfiguration = new SimulationConfiguration();


         _module = new Module();

         _spatialStructure.AddTopContainer(_organism);
      }

      protected override void Context()
      {
         sut = IoC.Resolve<QuantityValuesUpdater>();
      }
   }

   internal class When_updating_the_value_of_a_parameter_value_defined_as_formula_using_a_discrete_distributed_parameter : concern_for_QuantityValuesUpdaterIntegration
   {
      private ModelConfiguration _modelConfiguration;
      private ModuleConfiguration _moduleConfiguration;
      private ParameterValue _meanParameterValue;
      private ParameterValue _distributedParameterValue;
      private ValidationResult _result;
      private ExpressionProfileBuildingBlock _expressionProfileBuildingBlock;

      public override void GlobalContext()
      {
         base.GlobalContext();

         _distributedParameterValue = new ParameterValue
         {
            DistributionType = DistributionType.Discrete,
            Name = "Param",
            ContainerPath = new ObjectPath(_organism.Name, _liver.Name),
         };

         _meanParameterValue = new ParameterValue
         {
            Name = MEAN,
            ContainerPath = new ObjectPath(_organism.Name, _liver.Name, _parameter.Name),
            Value = 20
         };

         _parameterValues.Add(_distributedParameterValue);
         _parameterValues.Add(_meanParameterValue);

         _moduleConfiguration = new ModuleConfiguration(_module, null, _parameterValues);
         _simulationConfiguration.AddModuleConfiguration(_moduleConfiguration);
         _expressionProfileBuildingBlock = new ExpressionProfileBuildingBlock().WithName("molecule|species|category");
         _simulationConfiguration.AddExpressionProfile(_expressionProfileBuildingBlock);
         _simulationBuilder = new SimulationBuilderForSpecs(_simulationConfiguration);
         _modelConfiguration = new ModelConfiguration(_model, _simulationConfiguration, _simulationBuilder);
      }

      protected override void Because()
      {
         _result = sut.UpdateQuantitiesValues(_modelConfiguration);
      }

      [Observation]
      public void the_validation_result_indicates_missing_protein_for_present_expression()
      {
         _result.Messages.Single(x => x.BuildingBlock.Equals(_expressionProfileBuildingBlock)).Text.ShouldBeEqualTo(Warning.ExpressionMoleculeNotFoundInSimulation(_expressionProfileBuildingBlock.MoleculeName));
      }

      [Observation]
      public void should_overwrite_the_parameter_value_with_mean_value_of_the_parameter()
      {
         var parameter = _model.Root.EntityAt<IParameter>("Liver", "Param");
         parameter.Value.ShouldBeEqualTo(20);
      }
   }

   internal class When_updating_the_value_of_a_parameter_value_defined_as_formula_using_a_normal_distributed_parameter : concern_for_QuantityValuesUpdaterIntegration
   {
      private ModelConfiguration _modelConfiguration;
      private ModuleConfiguration _moduleConfiguration;
      private ParameterValue _meanParameterValue;
      private ParameterValue _distributedParameterValue;
      private ParameterValue _deviationParameterValue;
      private ParameterValue _percentileParameter;

      protected override void Context()
      {
         base.Context();

         _distributedParameterValue = new ParameterValue
         {
            DistributionType = DistributionType.Normal,
            Name = "Param",
            ContainerPath = new ObjectPath(_organism.Name, _liver.Name),
         };

         _meanParameterValue = new ParameterValue
         {
            Name = MEAN,
            ContainerPath = new ObjectPath(_organism.Name, _liver.Name, _parameter.Name),
            Value = 20
         };

         _deviationParameterValue = new ParameterValue
         {
            Name = DEVIATION,
            ContainerPath = new ObjectPath(_organism.Name, _liver.Name, _parameter.Name),
            Value = 5
         };

         _percentileParameter = new ParameterValue
         {
            Name = PERCENTILE,
            ContainerPath = new ObjectPath(_organism.Name, _liver.Name, _parameter.Name),
            Value = 0.8
         };

         _parameterValues.Add(_distributedParameterValue);
         _parameterValues.Add(_meanParameterValue);
         _parameterValues.Add(_deviationParameterValue);
         _parameterValues.Add(_percentileParameter);

         _moduleConfiguration = new ModuleConfiguration(_module, null, _parameterValues);
         _simulationConfiguration.AddModuleConfiguration(_moduleConfiguration);
         _simulationBuilder = new SimulationBuilderForSpecs(_simulationConfiguration);
         _modelConfiguration = new ModelConfiguration(_model, _simulationConfiguration, _simulationBuilder);
      }

      protected override void Because()
      {
         sut.UpdateQuantitiesValues(_modelConfiguration);
      }

      [Observation]
      public void should_overwrite_the_parameter_value_with_the_value_of_the_parameter()
      {
         var parameter = _model.Root.EntityAt<IParameter>("Liver", "Param");
         parameter.Value.ShouldBeEqualTo(24.28, 1e-2);
      }
   }

   internal abstract class concern_for_QuantityValuesUpdaterIntegration_with_value_origin : concern_for_QuantityValuesUpdaterIntegration
   {
      protected IParameter _individualParameter;
      protected IParameter _expressionParameter;
      protected MoleculeAmount _moleculeAmount;
      protected ValueOrigin _originalValueOrigin;
      protected IndividualBuildingBlock _individual;
      protected ExpressionProfileBuildingBlock _expressionProfile;
      protected InitialConditionsBuildingBlock _initialConditions;
      protected ModelConfiguration _modelConfiguration;

      public override void GlobalContext()
      {
         base.GlobalContext();
         var formulaFactory = IoC.Resolve<IFormulaFactory>();
         var amountDimension = IoC.Resolve<IDimensionFactory>().Dimension(Constants.Dimension.MOLAR_AMOUNT);

         _originalValueOrigin = new ValueOrigin
         {
            Source = ValueOriginSources.Publication,
            Method = ValueOriginDeterminationMethods.InVitro,
            Description = "Original"
         };

         _individualParameter = _modelHelper.NewConstantParameter("IndividualParam", 10);
         _expressionParameter = _modelHelper.NewConstantParameter("ExpressionParam", 10);
         _moleculeAmount = new MoleculeAmount
         {
            Name = "A",
            Dimension = amountDimension,
            Formula = formulaFactory.ConstantFormula(1, amountDimension)
         };

         new IQuantity[] { _parameter, _individualParameter, _expressionParameter, _moleculeAmount }
            .Each(x => x.ValueOrigin.UpdateAllFrom(_originalValueOrigin));

         _liver.Add(_individualParameter);
         _liver.Add(_expressionParameter);
         _liver.Add(_moleculeAmount);

         var molecules = new MoleculeBuildingBlock { new MoleculeBuilder { Name = "A" } };
         _module.Add(molecules);

         _individual = new IndividualBuildingBlock();
         _expressionProfile = new ExpressionProfileBuildingBlock().WithName("A|Human|Healthy");
         _initialConditions = new InitialConditionsBuildingBlock();

         AddBuilders(amountDimension);

         _simulationConfiguration.Individual = _individual;
         _simulationConfiguration.AddExpressionProfile(_expressionProfile);
         _simulationConfiguration.AddModuleConfiguration(new ModuleConfiguration(_module, _initialConditions, _parameterValues));
         _simulationBuilder = new SimulationBuilderForSpecs(_simulationConfiguration);
         _modelConfiguration = new ModelConfiguration(_model, _simulationConfiguration, _simulationBuilder);
      }

      protected abstract void AddBuilders(IDimension amountDimension);

      protected override void Because()
      {
         sut.UpdateQuantitiesValues(_modelConfiguration);
      }

      protected ValueOrigin ValueOriginFor(string parameterName) => _model.Root.EntityAt<IParameter>("Liver", parameterName).ValueOrigin;
   }

   internal class When_updating_the_quantities_values_using_builders_defining_a_value_origin : concern_for_QuantityValuesUpdaterIntegration_with_value_origin
   {
      private ValueOrigin _parameterValueOrigin;
      private ValueOrigin _individualValueOrigin;
      private ValueOrigin _expressionValueOrigin;
      private ValueOrigin _initialConditionValueOrigin;

      protected override void AddBuilders(IDimension amountDimension)
      {
         _parameterValueOrigin = valueOrigin(ValueOriginSources.Database, "ParameterValue");
         _individualValueOrigin = valueOrigin(ValueOriginSources.Internet, "IndividualParameter");
         _expressionValueOrigin = valueOrigin(ValueOriginSources.ParameterIdentification, "ExpressionParameter");
         _initialConditionValueOrigin = valueOrigin(ValueOriginSources.Other, "InitialCondition");

         _parameterValues.Add(withValueOrigin(new ParameterValue
         {
            Path = new ObjectPath(_organism.Name, _liver.Name, _parameter.Name),
            Value = 20
         }, _parameterValueOrigin));

         _individual.Add(withValueOrigin(new IndividualParameter
         {
            Path = new ObjectPath(_organism.Name, _liver.Name, _individualParameter.Name),
            Value = 30
         }, _individualValueOrigin));

         _expressionProfile.Add(withValueOrigin(new ExpressionParameter
         {
            Path = new ObjectPath(_organism.Name, _liver.Name, _expressionParameter.Name),
            Value = 40
         }, _expressionValueOrigin));

         _initialConditions.Add(withValueOrigin(new InitialCondition
         {
            Path = new ObjectPath(_organism.Name, _liver.Name, _moleculeAmount.Name),
            Value = 50,
            IsPresent = true,
            Dimension = amountDimension
         }, _initialConditionValueOrigin));
      }

      private T withValueOrigin<T>(T pathAndValueEntity, ValueOrigin valueOrigin) where T : PathAndValueEntity
      {
         pathAndValueEntity.ValueOrigin.UpdateAllFrom(valueOrigin);
         return pathAndValueEntity;
      }

      private ValueOrigin valueOrigin(ValueOriginSource source, string description) =>
         new ValueOrigin { Source = source, Method = ValueOriginDeterminationMethods.Assumption, Description = description };

      [Observation]
      public void should_take_over_the_value_origin_defined_in_the_parameter_value()
      {
         ValueOriginFor(_parameter.Name).ShouldBeEqualTo(_parameterValueOrigin);
      }

      [Observation]
      public void should_take_over_the_value_origin_defined_in_the_individual_parameter()
      {
         ValueOriginFor(_individualParameter.Name).ShouldBeEqualTo(_individualValueOrigin);
      }

      [Observation]
      public void should_take_over_the_value_origin_defined_in_the_expression_parameter()
      {
         ValueOriginFor(_expressionParameter.Name).ShouldBeEqualTo(_expressionValueOrigin);
      }

      [Observation]
      public void should_take_over_the_value_origin_defined_in_the_initial_condition()
      {
         _model.Root.EntityAt<MoleculeAmount>("Liver", "A").ValueOrigin.ShouldBeEqualTo(_initialConditionValueOrigin);
      }
   }

   internal class When_updating_the_quantities_values_using_builders_that_do_not_override_the_value : concern_for_QuantityValuesUpdaterIntegration_with_value_origin
   {
      protected override void AddBuilders(IDimension amountDimension)
      {
         _parameterValues.Add(new ParameterValue
         {
            Path = new ObjectPath(_organism.Name, _liver.Name, _parameter.Name)
         });

         _individual.Add(new IndividualParameter
         {
            Path = new ObjectPath(_organism.Name, _liver.Name, _individualParameter.Name)
         });

         _expressionProfile.Add(new ExpressionParameter
         {
            Path = new ObjectPath(_organism.Name, _liver.Name, _expressionParameter.Name)
         });
      }

      [Observation]
      public void should_have_kept_the_value_origin_defined_in_the_model()
      {
         ValueOriginFor(_parameter.Name).ShouldBeEqualTo(_originalValueOrigin);
         ValueOriginFor(_individualParameter.Name).ShouldBeEqualTo(_originalValueOrigin);
         ValueOriginFor(_expressionParameter.Name).ShouldBeEqualTo(_originalValueOrigin);
      }
   }
}