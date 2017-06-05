using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;
using OSPSuite.Helpers;

namespace OSPSuite.Core
{
   public abstract class concern_for_ParameterIdentificationValidator : ContextSpecification<ParameterIdentificationValidator>
   {
      protected ValidationResult _result;
      private IFullPathDisplayResolver _fullPathDisplayResolver;

      protected override void Context()
      {
         base.Context();
         _result = new ValidationResult();
         _fullPathDisplayResolver = A.Fake<IFullPathDisplayResolver>();
         sut = new ParameterIdentificationValidator(_fullPathDisplayResolver);
      }

      protected void ConfigureOutputMapping(OutputMapping outputMapping)
      {
         outputMapping.WeightedObservedData = new WeightedObservedData(DomainHelperForSpecs.ObservedData());
         var quantity = A.Fake<IQuantity>();
         quantity.Dimension = outputMapping.WeightedObservedData.ObservedData.FirstDataColumn().Dimension;

         A.CallTo(() => outputMapping.Output).Returns(quantity);
         A.CallTo(() => outputMapping.IsValid).Returns(true);
      }
   }

   public class When_validating_a_parameter_identification_without_output_mappings : concern_for_ParameterIdentificationValidator
   {
      private ParameterIdentification _parameterIdentification;
      

      protected override void Context()
      {
         base.Context();
         _parameterIdentification = new ParameterIdentification();
         _parameterIdentification.Configuration.AlgorithmProperties = new OptimizationAlgorithmProperties("XX");
         _parameterIdentification.AddIdentificationParameter(DomainHelperForSpecs.IdentificationParameter());
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

   public class When_validating_a_parameter_identification_with_one_observed_data_point_weighted_negative : concern_for_ParameterIdentificationValidator
   {
      private ParameterIdentification _parameterIdentification;
      protected override void Context()
      {
         base.Context();
         _parameterIdentification = new ParameterIdentification();
         var outputMapping = A.Fake<OutputMapping>();
         ConfigureOutputMapping(outputMapping);
         A.CallTo(() => outputMapping.IsValid).Returns(true);

         _parameterIdentification.AddOutputMapping(outputMapping);
         _parameterIdentification.Configuration.AlgorithmProperties = new OptimizationAlgorithmProperties("XX");
         _parameterIdentification.AddIdentificationParameter(DomainHelperForSpecs.IdentificationParameter());
         outputMapping.WeightedObservedData.Weights[0] = -1;
      }

      protected override void Because()
      {
         _result = sut.Validate(_parameterIdentification);
      }

      [Observation]
      public void The_validation_should_indicate_an_error()
      {
         _result.ValidationState.ShouldBeEqualTo(ValidationState.Invalid);
      }
   }

   public class When_validating_a_parameter_identification_with_an_output_mapping_not_mapped_to_any_observed_data : concern_for_ParameterIdentificationValidator
   {
      private ParameterIdentification _parameterIdentification;
      protected override void Context()
      {
         base.Context();
         _parameterIdentification = new ParameterIdentification();
         var outputMapping = A.Fake<OutputMapping>();
         ConfigureOutputMapping(outputMapping);
         outputMapping.WeightedObservedData = null;
         A.CallTo(() => outputMapping.IsValid).Returns(true);

         _parameterIdentification.AddOutputMapping(outputMapping);
         _parameterIdentification.Configuration.AlgorithmProperties = new OptimizationAlgorithmProperties("XX");
         _parameterIdentification.AddIdentificationParameter(DomainHelperForSpecs.IdentificationParameter());
      }

      protected override void Because()
      {
         _result = sut.Validate(_parameterIdentification);
      }

      [Observation]
      public void The_validation_should_indicate_an_error()
      {
         _result.ValidationState.ShouldBeEqualTo(ValidationState.Invalid);
      }
   }


   public class When_validating_a_parameter_identification_with_an_observed_data_repository_weighted_negative : concern_for_ParameterIdentificationValidator
   {
      private ParameterIdentification _parameterIdentification;

      protected override void Context()
      {
         base.Context();
         _parameterIdentification = new ParameterIdentification();
         var outputMapping = A.Fake<OutputMapping>();
         ConfigureOutputMapping(outputMapping);
         A.CallTo(() => outputMapping.IsValid).Returns(true);

         outputMapping.Weight = -1;
         _parameterIdentification.AddOutputMapping(outputMapping);
         _parameterIdentification.Configuration.AlgorithmProperties = new OptimizationAlgorithmProperties("XX");
         _parameterIdentification.AddIdentificationParameter(DomainHelperForSpecs.IdentificationParameter());
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

   public class When_validating_a_parameter_identification_without_output_mapping_not_in_a_valid_state : concern_for_ParameterIdentificationValidator
   {
      private ParameterIdentification _parameterIdentification;

      protected override void Context()
      {
         base.Context();
         _parameterIdentification = new ParameterIdentification();
         var outputMapping = A.Fake<OutputMapping>();
         ConfigureOutputMapping(outputMapping);
         A.CallTo(() => outputMapping.IsValid).Returns(false);
         _parameterIdentification.AddOutputMapping(outputMapping);
         _parameterIdentification.Configuration.AlgorithmProperties = new OptimizationAlgorithmProperties("XX");
         _parameterIdentification.AddIdentificationParameter(DomainHelperForSpecs.IdentificationParameter());
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

   public class When_validating_a_parameter_identification_without_identification_parameters : concern_for_ParameterIdentificationValidator
   {
      private ParameterIdentification _parameterIdentification;

      protected override void Context()
      {
         base.Context();
         _parameterIdentification = new ParameterIdentification();
         var outputMapping = A.Fake<OutputMapping>();
         ConfigureOutputMapping(outputMapping);
         _parameterIdentification.AddOutputMapping(outputMapping);
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

   public class When_validating_a_parameter_identification_with_multiple_outputs_mapped_to_different_scalings : concern_for_ParameterIdentificationValidator
   {
      private ParameterIdentification _parameterIdentification;

      protected override void Context()
      {
         base.Context();
         _parameterIdentification = new ParameterIdentification();
         _parameterIdentification.AddIdentificationParameter(DomainHelperForSpecs.IdentificationParameter());
         var outputMapping = A.Fake<OutputMapping>();
         ConfigureOutputMapping(outputMapping);
         _parameterIdentification.AddOutputMapping(outputMapping);
         outputMapping.Scaling = Scalings.Log;

         var outputMapping2 = A.Fake<OutputMapping>();
         ConfigureOutputMapping(outputMapping2);
         _parameterIdentification.AddOutputMapping(outputMapping2);
         outputMapping2.Scaling = Scalings.Linear;

         A.CallTo(() => outputMapping2.Output).Returns(outputMapping.Output);

         _parameterIdentification.Configuration.AlgorithmProperties = new OptimizationAlgorithmProperties("XX");
      }

      protected override void Because()
      {
         _result = sut.Validate(_parameterIdentification);
      }

      [Observation]
      public void should_be_invalid()
      {
         _result.ValidationState.ShouldBeEqualTo(ValidationState.Invalid);
      }
   }

   public class When_validating_a_parameter_identification_without_algorithm : concern_for_ParameterIdentificationValidator
   {
      private ParameterIdentification _parameterIdentification;

      protected override void Context()
      {
         base.Context();
         _parameterIdentification = new ParameterIdentification();
         var outputMapping = A.Fake<OutputMapping>();
         ConfigureOutputMapping(outputMapping);
         _parameterIdentification.AddOutputMapping(outputMapping);
         _parameterIdentification.AddIdentificationParameter(DomainHelperForSpecs.IdentificationParameter());
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

   public class When_validating_a_parameter_identification_that_is_valid : concern_for_ParameterIdentificationValidator
   {
      private ParameterIdentification _parameterIdentification;

      protected override void Context()
      {
         base.Context();
         _parameterIdentification = new ParameterIdentification();
         var outputMapping = A.Fake<OutputMapping>();
         ConfigureOutputMapping(outputMapping);

         _parameterIdentification.AddOutputMapping(outputMapping);
         _parameterIdentification.AddIdentificationParameter(DomainHelperForSpecs.IdentificationParameter());
         _parameterIdentification.Configuration.AlgorithmProperties = new OptimizationAlgorithmProperties("XX");
      }

      protected override void Because()
      {
         _result = sut.Validate(_parameterIdentification);
      }

      [Observation]
      public void should_report_a_valid_state()
      {
         _result.ValidationState.ShouldBeEqualTo(ValidationState.Valid);
      }
   }

   public class When_validating_a_parameter_identification_which_contains_a_mapping_with_mismatched_dimensions : concern_for_ParameterIdentificationValidator
   {
      private ParameterIdentification _parameterIdentification;

      protected override void Context()
      {
         base.Context();
         _parameterIdentification = new ParameterIdentification();
         var outputMapping = A.Fake<OutputMapping>();
         ConfigureOutputMapping(outputMapping);

         _parameterIdentification.AddOutputMapping(outputMapping);
         _parameterIdentification.AddIdentificationParameter(DomainHelperForSpecs.IdentificationParameter());
         _parameterIdentification.Configuration.AlgorithmProperties = new OptimizationAlgorithmProperties("XX");
         outputMapping.Output.Dimension = DomainHelperForSpecs.FractionDimensionForSpecs();
      }

      protected override void Because()
      {
         _result = sut.Validate(_parameterIdentification);
      }

      [Observation]
      public void the_validation_should_fail()
      {
         _result.ValidationState.ShouldBeEqualTo(ValidationState.Invalid);
      }
   }
}
