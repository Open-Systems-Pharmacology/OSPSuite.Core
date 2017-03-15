using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Helpers;

namespace OSPSuite.Core
{
   public abstract class concern_for_EntityValidator : ContextSpecification<IEntityValidator>
   {
      private ISensitivityAnalysisValidator _sensitivityAnalysisValidator;

      protected override void Context()
      {
         var parameterIdentificationValidator = A.Fake<IParameterIdentificationValidator>();
         _sensitivityAnalysisValidator= A.Fake<ISensitivityAnalysisValidator>();
         sut = new EntityValidator(parameterIdentificationValidator,_sensitivityAnalysisValidator);
      }
   }

   public class When_validating_a_parameter_identification_with_value_outside_of_min_max_for_an_identification_parameter : concern_for_EntityValidator
   {
      private ParameterIdentification _parameterIdentification;
      private ValidationResult _result;

      protected override void Context()
      {
         base.Context();
         _parameterIdentification = new ParameterIdentification();
         var outputMapping = A.Fake<OutputMapping>();
         _parameterIdentification.AddOutputMapping(outputMapping);
         _parameterIdentification.AddIdentificationParameter(DomainHelperForSpecs.IdentificationParameter(min: 10, max: 20, startValue: 30));
         _parameterIdentification.Configuration.AlgorithmProperties = new OptimizationAlgorithmProperties("XX");
      }

      protected override void Because()
      {
         _result = sut.Validate(_parameterIdentification);
      }

      [Observation]
      public void should_report_an_error_state()
      {
         _result.ValidationState.ShouldBeEqualTo(ValidationState.Invalid);
      }
   }


   public class When_validating_a_container_container_two_parameters_with_one_invalid : concern_for_EntityValidator
   {
      private IContainer _container;
      private ValidationResult _result;
      private IParameter _invalidParameter;

      protected override void Context()
      {
         base.Context();
         _container = new Container().WithName("Container");
         _container.Add(DomainHelperForSpecs.ConstantParameterWithValue(5).WithName("Valid"));
         _invalidParameter = DomainHelperForSpecs.ConstantParameterWithValue(10).WithName("Invalid");
         _invalidParameter.Info.MaxIsAllowed = false;
         _invalidParameter.Info.MaxValue = 10;
         _container.Add(_invalidParameter);
      }

      protected override void Because()
      {
         _result = sut.Validate(_container);
      }

      [Observation]
      public void should_return_a_validation_results_having_the_state_invalid()
      {
         _result.ValidationState.ShouldBeEqualTo(ValidationState.Invalid);
      }

      [Observation]
      public void the_validation_messages_should_contain_the_entity_invalid()
      {
         _result.Messages.Count().ShouldBeEqualTo(1);
         _result.Messages.ElementAt(0).Object.ShouldBeEqualTo(_invalidParameter);
      }
   }
}