using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.ParameterIdentifications.Algorithms;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Presentation.Mappers.ParameterIdentifications;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_ParameterIdentificationConfigurationToParameterIdentificationConfigurationDTOMapper : ContextSpecification<IParameterIdentificationConfigurationToParameterIdentificationConfigurationDTOMapper>
   {
      protected IReadOnlyList<IOptimizationAlgorithm> _allAlgorithms;
      protected IOptimizationAlgorithm _option1;
      private IOptimizationAlgorithm _option2;
      private IOptimizationAlgorithm _option3;
      protected ParameterIdentificationConfiguration _parameterIdentificationConfiguration;

      protected override void Context()
      {
         sut = new ParameterIdentificationConfigurationToParameterIdentificationConfigurationDTOMapper();

         _parameterIdentificationConfiguration = new ParameterIdentificationConfiguration();
         _option1 = A.Fake<IOptimizationAlgorithm>();
         A.CallTo(() => _option1.Name).Returns("option1");

         _option2 = A.Fake<IOptimizationAlgorithm>();
         A.CallTo(() => _option2.Name).Returns("option2");

         _option3 = A.Fake<IOptimizationAlgorithm>();
         A.CallTo(() => _option3.Name).Returns("option3");

         _allAlgorithms = new[] {_option2, _option1, _option3};
      }
   }

   public class When_mapping_a_parameter_ientification_configuration_to_a_parameter_identification_configuration_dto : concern_for_ParameterIdentificationConfigurationToParameterIdentificationConfigurationDTOMapper
   {
      private ParameterIdentificationConfigurationDTO _dto;

      protected override void Context()
      {
         base.Context();
         _parameterIdentificationConfiguration.AlgorithmProperties = new OptimizationAlgorithmProperties("option1");
      }

      protected override void Because()
      {
         _dto = sut.MapFrom(_parameterIdentificationConfiguration, _allAlgorithms);
      }

      [Observation]
      public void the_algorithm_with_the_matching_name_should_be_initialized()
      {
         _dto.OptimizationAlgorithm.ShouldBeEqualTo(_option1);
      }
   }

   public class When_mapping_a_parameter_ientification_configuration_to_a_parameter_identification_configuration_dto_and_the_algo_does_not_exist : concern_for_ParameterIdentificationConfigurationToParameterIdentificationConfigurationDTOMapper
   {
      private ParameterIdentificationConfigurationDTO _dto;

      protected override void Context()
      {
         base.Context();
         _parameterIdentificationConfiguration.AlgorithmProperties = new OptimizationAlgorithmProperties("option4");
      }

      protected override void Because()
      {
         _dto = sut.MapFrom(_parameterIdentificationConfiguration, _allAlgorithms);
      }

      [Observation]
      public void should_return_null()
      {
         _dto.OptimizationAlgorithm.ShouldBeNull();
      }
   }
}