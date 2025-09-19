using System.Linq;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Helpers;
using OSPSuite.Utility.Container;
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
         _simulationBuilder = new SimulationBuilder(_simulationConfiguration);
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
         _simulationBuilder = new SimulationBuilder(_simulationConfiguration);
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
}