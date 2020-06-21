using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_SensitivityAnalysisValidator : ContextSpecification<ISensitivityAnalysisValidator>
   {
      protected SensitivityAnalysis _sensitivityAnalysis;
      protected ValidationResult _result;
      protected SensitivityParameter _sensitivityParameter;
      protected ISimulation _simulation;

      protected override void Context()
      {
         _simulation= A.Fake<ISimulation>();
         A.CallTo(() => _simulation.OutputSelections.HasSelection).Returns(true);

         _sensitivityAnalysis = new SensitivityAnalysis {Simulation = _simulation};
         sut = new SensitivityAnalysisValidator();

         _sensitivityParameter = DomainHelperForSpecs.SensitivityParameter();
         _sensitivityParameter.ParameterSelection = A.Fake<ParameterSelection>();

      }

      protected override void Because()
      {
         _result = sut.Validate(_sensitivityAnalysis);
      }
   }

   public class When_validating_an_sensitivity_analysis_with_invalid_sensitivity_parameter : concern_for_SensitivityAnalysisValidator
   {
      protected override void Context()
      {
         base.Context();
         var sensitivityParameter = DomainHelperForSpecs.SensitivityParameter();
         sensitivityParameter.ParameterSelection = new ParameterSelection(_simulation, "path");
         _sensitivityAnalysis.AddSensitivityParameter(sensitivityParameter);
      }

      [Observation]
      public void the_validation_fails()
      {
         _result.ValidationState.ShouldBeEqualTo(ValidationState.Invalid);
      }
   }

   public class When_validating_a_valid_sensitivity_analysis : concern_for_SensitivityAnalysisValidator
   {
      protected override void Context()
      {
         base.Context();
         _sensitivityAnalysis.AddSensitivityParameter(_sensitivityParameter);
         _sensitivityParameter.ParameterSelection.Parameter.Value = 10;
      }

      [Observation]
      public void should_return_a_valid_state()
      {
         _result.ValidationState.ShouldBeEqualTo(ValidationState.Valid);
      }
   }

   public class When_validating_a_sensitivity_analysis_without_a_valid_simulation : concern_for_SensitivityAnalysisValidator
   {
      protected override void Context()
      {
         base.Context();
         _sensitivityAnalysis = new SensitivityAnalysis();
      }

      [Observation]
      public void should_return_an_error_state()
      {
         _result.ValidationState.ShouldBeEqualTo(ValidationState.Invalid);
      }
   }


   public class When_validating_a_sensitivity_analysis_with_a_simulation_that_does_not_have_any_outputs : concern_for_SensitivityAnalysisValidator
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _simulation.OutputSelections.HasSelection).Returns(false);
      }

      [Observation]
      public void should_return_an_error_state()
      {
         _result.ValidationState.ShouldBeEqualTo(ValidationState.Invalid);
      }
   }

   public class When_validating_a_sensitivity_analysis_without_sensitivy_parameters : concern_for_SensitivityAnalysisValidator
   {
      [Observation]
      public void should_return_an_error_state()
      {
         _result.ValidationState.ShouldBeEqualTo(ValidationState.Invalid);
      }
   }

   public class When_validating_a_sensitivity_analysis_with_all_sensitivy_parameters_having_a_default_value_of_zero : concern_for_SensitivityAnalysisValidator
   {
      protected override void Context()
      {
         base.Context();
         _sensitivityAnalysis.AddSensitivityParameter(_sensitivityParameter);
         _sensitivityParameter.ParameterSelection.Parameter.Value = 0;
      }

      [Observation]
      public void should_return_an_error_state()
      {
         _result.ValidationState.ShouldBeEqualTo(ValidationState.Invalid);
      }
   }

}