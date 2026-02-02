using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Services;

internal abstract class concern_for_SimulationQuantityWarningTask : ContextSpecification<SimulationQuantityValueWarningTask>
{
   protected CoreUserSettings _userSettings;
   protected CreationResult _creationResult;
   protected Model _model;
   protected SimulationBuilder _simulationBuilder;
   protected IBuildingBlock _buildingBlock;
   private IObjectTypeResolver _objectTypeResolver;

   protected const string NAN_AMOUNT_ID = "nanAmount";
   protected const string NAN_AMOUNT_NAME = "nan amount";
   protected const string AMOUNT_BUILDER_NAME = "my amount";
   protected const string NAN_PARAMETER_ID = "nanParameter";
   protected const string NAN_PARAMETER_NAME = "nan parameter";
   protected const string PARAMETER_BUILDER_ID = "parameter";
   protected const string PARAMETER_BUILDER_NAME = "my parameter";

   protected class CoreUserSettings : ICoreUserSettings
   {
      public int MaximumNumberOfCoresToUse { get; set; }
      public int NumberOfBins { get; set; }
      public int NumberOfIndividualsPerBin { get; set; }
      public bool WarnForNonFiniteQuantities { get; set; }
   }

   protected override void Context()
   {
      base.Context();
      _userSettings = new CoreUserSettings();
      _objectTypeResolver = A.Fake<IObjectTypeResolver>();
      sut = new SimulationQuantityValueWarningTask(_userSettings, _objectTypeResolver);
      _model = new Model { Root = new ARootContainer() };
      _simulationBuilder = new SimulationBuilder(new SimulationConfiguration());
      _creationResult = new CreationResult(_model, _simulationBuilder);

      _buildingBlock = new ReactionBuildingBlock();

      A.CallTo(() => _objectTypeResolver.TypeFor<IParameter>()).Returns("Parameter");
      A.CallTo(() => _objectTypeResolver.TypeFor<MoleculeAmount>()).Returns("MoleculeAmount");
   }
}

internal abstract class When_creating_warnings_for_non_finite_quantities : concern_for_SimulationQuantityWarningTask
{
   protected override void Context()
   {
      base.Context();
      var quantity = NewBuilder();
      var nanParameter = NewSimulationQuantity();
      var nonFiniteQuantities = new List<IQuantity> { nanParameter };
      _userSettings.WarnForNonFiniteQuantities = ShouldWarnForFiniteParameters;
      _model.Root.AddChildren(nonFiniteQuantities);

      _simulationBuilder.AddToBuilderSource(quantity, _buildingBlock);
      _simulationBuilder.AddSimulationEntitySource(nanParameter.Id, new SimulationEntitySource(_simulationBuilder.BuilderSourceFor(quantity).BuildingBlock, quantity.EntityPath(), quantity));
   }

   protected abstract IQuantity NewSimulationQuantity();

   protected abstract IQuantity NewBuilder();

   protected abstract bool ShouldWarnForFiniteParameters { get; }
}

internal class creating_warnings_during_simulation_run_for_non_finite_molecule_amounts_and_the_user_setting_disables_warnings : When_creating_warnings_for_non_finite_quantities
{
   private RunValidationResult _runValidationResult;

   protected override bool ShouldWarnForFiniteParameters => false;

   protected override void Context()
   {
      base.Context();
      _runValidationResult = new RunValidationResult();
   }

   protected override void Because()
   {
      sut.WarnForNonFiniteQuantities(_model, _runValidationResult);
   }

   protected override IQuantity NewSimulationQuantity()
   {
      return new MoleculeAmount().WithValue(double.NaN).WithId(NAN_AMOUNT_ID).WithName(NAN_AMOUNT_NAME);
   }

   protected override IQuantity NewBuilder()
   {
      return new MoleculeAmount().WithId(NAN_AMOUNT_ID).WithName(AMOUNT_BUILDER_NAME);
   }

   [Observation]
   public void should_add_a_warning_to_the_creation_result()
   {
      _runValidationResult.ValidationResult.Messages.Count().ShouldBeEqualTo(0);
   }
}

internal class creating_warnings_during_simulation_run_for_non_finite_molecule_amounts_and_the_user_setting_enables_warnings : When_creating_warnings_for_non_finite_quantities
{
   private RunValidationResult _runValidationResult;
   protected override bool ShouldWarnForFiniteParameters => true;

   protected override void Context()
   {
      base.Context();
      _runValidationResult = new RunValidationResult();
   }

   protected override void Because()
   {
      sut.WarnForNonFiniteQuantities(_model, _runValidationResult);
   }

   protected override IQuantity NewSimulationQuantity()
   {
      return new MoleculeAmount().WithValue(double.NaN).WithId(NAN_AMOUNT_ID).WithName(NAN_AMOUNT_NAME);
   }

   protected override IQuantity NewBuilder()
   {
      return new MoleculeAmount().WithId(NAN_AMOUNT_ID).WithName(AMOUNT_BUILDER_NAME);
   }

   [Observation]
   public void should_add_a_warning_to_the_creation_result()
   {
      _runValidationResult.ValidationResult.Messages.Count().ShouldBeEqualTo(1);
   }
}

internal class creating_warnings_during_simulation_creation_for_non_finite_molecule_amounts_and_the_user_setting_disables_warnings : When_creating_warnings_for_non_finite_quantities
{
   protected override bool ShouldWarnForFiniteParameters => false;

   protected override void Because()
   {
      sut.WarnForNonFiniteQuantities(_model, _creationResult);
   }

   protected override IQuantity NewSimulationQuantity()
   {
      return new MoleculeAmount().WithValue(double.NaN).WithId(NAN_AMOUNT_ID).WithName(NAN_AMOUNT_NAME);
   }

   protected override IQuantity NewBuilder()
   {
      return new MoleculeAmount().WithId(NAN_AMOUNT_ID).WithName(AMOUNT_BUILDER_NAME);
   }

   [Observation]
   public void should_not_add_a_warning_to_the_creation_result()
   {
      _creationResult.ValidationResult.Messages.Count().ShouldBeEqualTo(0);
   }
}

internal class creating_warnings_during_simulation_creation_for_non_finite_molecule_amounts_and_the_user_setting_enables_warnings : When_creating_warnings_for_non_finite_quantities
{
   protected override bool ShouldWarnForFiniteParameters => true;

   protected override void Because()
   {
      sut.WarnForNonFiniteQuantities(_model, _creationResult);
   }

   protected override IQuantity NewSimulationQuantity()
   {
      return new MoleculeAmount().WithValue(double.NegativeInfinity).WithId(NAN_AMOUNT_ID).WithName(NAN_AMOUNT_NAME);
   }

   protected override IQuantity NewBuilder()
   {
      return new MoleculeAmount().WithId(NAN_AMOUNT_ID).WithName(AMOUNT_BUILDER_NAME);
   }

   [Observation]
   public void should_add_a_warning_to_the_creation_result()
   {
      _creationResult.ValidationResult.Messages.Count().ShouldBeEqualTo(1);
   }
}

internal class creating_warnings_during_simulation_run_for_non_finite_parameters_and_the_user_setting_disables_warnings : When_creating_warnings_for_non_finite_quantities
{
   private RunValidationResult _runValidationResult;

   protected override bool ShouldWarnForFiniteParameters => false;

   protected override void Context()
   {
      base.Context();
      _runValidationResult = new RunValidationResult();
   }

   protected override void Because()
   {
      sut.WarnForNonFiniteQuantities(_model, _runValidationResult);
   }

   protected override IQuantity NewSimulationQuantity()
   {
      return new Parameter().WithValue(double.NaN).WithId(NAN_PARAMETER_ID).WithName(NAN_PARAMETER_NAME);
   }

   protected override IQuantity NewBuilder()
   {
      return new Parameter().WithId(PARAMETER_BUILDER_ID).WithName(PARAMETER_BUILDER_NAME);
   }

   [Observation]
   public void should_add_a_warning_to_the_creation_result()
   {
      _runValidationResult.ValidationResult.Messages.Count().ShouldBeEqualTo(0);
   }
}

internal class creating_warnings_during_simulation_run_for_non_finite_parameters_and_the_user_setting_enables_warnings : When_creating_warnings_for_non_finite_quantities
{
   private RunValidationResult _runValidationResult;
   protected override bool ShouldWarnForFiniteParameters => true;

   protected override void Context()
   {
      base.Context();
      _runValidationResult = new RunValidationResult();
   }

   protected override void Because()
   {
      sut.WarnForNonFiniteQuantities(_model, _runValidationResult);
   }

   protected override IQuantity NewSimulationQuantity()
   {
      return new Parameter().WithValue(double.NaN).WithId(NAN_PARAMETER_ID).WithName(NAN_PARAMETER_NAME);
   }

   protected override IQuantity NewBuilder()
   {
      return new Parameter().WithId(PARAMETER_BUILDER_ID).WithName(PARAMETER_BUILDER_NAME);
   }

   [Observation]
   public void should_add_a_warning_to_the_creation_result()
   {
      _runValidationResult.ValidationResult.Messages.Count().ShouldBeEqualTo(1);
   }
}

internal class creating_warnings_during_simulation_creation_for_non_finite_parameters_and_the_user_setting_disables_warnings : When_creating_warnings_for_non_finite_quantities
{
   protected override bool ShouldWarnForFiniteParameters => false;

   protected override void Because()
   {
      sut.WarnForNonFiniteQuantities(_model, _creationResult);
   }

   protected override IQuantity NewSimulationQuantity()
   {
      return new Parameter().WithValue(double.NaN).WithId(NAN_PARAMETER_ID).WithName(NAN_PARAMETER_NAME);
   }

   protected override IQuantity NewBuilder()
   {
      return new Parameter().WithId(PARAMETER_BUILDER_ID).WithName(PARAMETER_BUILDER_NAME);
   }

   [Observation]
   public void should_not_add_a_warning_to_the_creation_result()
   {
      _creationResult.ValidationResult.Messages.Count().ShouldBeEqualTo(0);
   }
}

internal class creating_warnings_during_simulation_creation_for_non_finite_parameters_and_the_user_setting_enables_warnings : When_creating_warnings_for_non_finite_quantities
{
   protected override bool ShouldWarnForFiniteParameters => true;

   protected override void Because()
   {
      sut.WarnForNonFiniteQuantities(_model, _creationResult);
   }

   protected override IQuantity NewSimulationQuantity()
   {
      return new Parameter().WithValue(double.NaN).WithId(NAN_PARAMETER_ID).WithName(NAN_PARAMETER_NAME);
   }

   protected override IQuantity NewBuilder()
   {
      return new Parameter().WithId(PARAMETER_BUILDER_ID).WithName(PARAMETER_BUILDER_NAME);
   }

   [Observation]
   public void should_add_a_warning_to_the_creation_result()
   {
      _creationResult.ValidationResult.Messages.Count().ShouldBeEqualTo(1);
   }
}

internal class When_creating_warnings_for_optimized_and_removed_parameters_and_the_user_setting_enables_warnings : concern_for_SimulationQuantityWarningTask
{
   private IReadOnlyList<IParameter> _optimizedParameters;

   protected override void Context()
   {
      base.Context();
      var parameter = new Parameter().WithId(PARAMETER_BUILDER_ID);
      var nanParameter = parameter.WithValue(double.NaN).WithId(NAN_PARAMETER_ID);
      _optimizedParameters = new List<IParameter> { nanParameter };
      _userSettings.WarnForNonFiniteQuantities = true;
      _model.Root.AddChildren(_optimizedParameters);

      _simulationBuilder.AddToBuilderSource(parameter, _buildingBlock);
      _simulationBuilder.AddSimulationEntitySource(nanParameter.Id, new SimulationEntitySource(_simulationBuilder.BuilderSourceFor(parameter).BuildingBlock, parameter.EntityPath(), parameter));
   }

   protected override void Because()
   {
      sut.WarnForOptimizedLocalMoleculeParameters(_optimizedParameters, _creationResult);
   }

   [Observation]
   public void should_add_a_warning_to_the_creation_result()
   {
      _creationResult.ValidationResult.Messages.Count().ShouldBeEqualTo(1);
   }
}

internal class When_creating_warnings_for_optimized_and_removed_parameters_and_the_user_setting_disables_warnings : concern_for_SimulationQuantityWarningTask
{
   private IReadOnlyList<IParameter> _optimizedParameters;

   protected override void Context()
   {
      base.Context();
      var parameter = new Parameter().WithId(PARAMETER_BUILDER_ID);
      var nanParameter = parameter.WithValue(double.NaN).WithId(NAN_PARAMETER_ID);
      _optimizedParameters = new List<IParameter> { nanParameter };
      _userSettings.WarnForNonFiniteQuantities = false;
      _model.Root.AddChildren(_optimizedParameters);

      _simulationBuilder.AddToBuilderSource(parameter, _buildingBlock);
      _simulationBuilder.AddSimulationEntitySource(nanParameter.Id, new SimulationEntitySource(_simulationBuilder.BuilderSourceFor(parameter).BuildingBlock, parameter.EntityPath(), parameter));
   }

   protected override void Because()
   {
      sut.WarnForOptimizedLocalMoleculeParameters(_optimizedParameters, _creationResult);
   }

   [Observation]
   public void should_not_add_a_warning_to_the_creation_result()
   {
      _creationResult.ValidationResult.Messages.Count().ShouldBeEqualTo(0);
   }
}