using System.Drawing;
using System.Linq;
using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Helpers;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Presentation.Mappers.ParameterIdentifications;
using OSPSuite.Presentation.Services;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_ParameterIdentificationRunResultToRunResultDTOMapper : ContextSpecification<IParameterIdentificationRunResultToRunResultDTOMapper>
   {
      protected ParameterIdentificationRunResult _runResult;
      protected ParameterIdentification _parameterIdentification;
      protected ParameterIdentificationRunResultDTO _dto;
      protected IdentificationParameter _identificationParameter1;
      protected IdentificationParameter _identificationParameter2;
      protected IdentificationParameter _identificationParameter3;
      protected OptimizationRunResult _optimizationRunResult;
      protected IOptimizedParameterRangeImageCreator _rangeImageCreator;
      protected OptimizedParameterValue _optimizedParameterValue1;
      protected OptimizedParameterValue _optimizedParameterValue2;

      protected override void Context()
      {
         _rangeImageCreator = A.Fake<IOptimizedParameterRangeImageCreator>();
         sut = new ParameterIdentificationRunResultToRunResultDTOMapper(_rangeImageCreator);

         _parameterIdentification = new ParameterIdentification();
         _runResult = A.Fake<ParameterIdentificationRunResult>().WithDescription("Desc");
         _runResult.Index = 5;
         _optimizationRunResult = new OptimizationRunResult();
         _runResult.BestResult = _optimizationRunResult;
         A.CallTo(() => _runResult.NumberOfEvaluations).Returns(10);
         A.CallTo(() => _runResult.TotalError).Returns(5);

         _identificationParameter1 = new IdentificationParameter().WithName("P1");
         _identificationParameter1.Add(parameterNamed(1, Constants.Parameters.MIN_VALUE));
         _identificationParameter1.Add(parameterNamed(2, Constants.Parameters.START_VALUE));
         _identificationParameter1.Add(parameterNamed(3, Constants.Parameters.MAX_VALUE));

         _identificationParameter2 = new IdentificationParameter().WithName("P2");
         _identificationParameter2.Add(parameterNamed(4, Constants.Parameters.MIN_VALUE));
         _identificationParameter2.Add(parameterNamed(5, Constants.Parameters.START_VALUE));
         _identificationParameter2.Add(parameterNamed(6, Constants.Parameters.MAX_VALUE));

         _identificationParameter3 = new IdentificationParameter().WithName("P3");
         _identificationParameter3.Add(parameterNamed(7, Constants.Parameters.MIN_VALUE));
         _identificationParameter3.Add(parameterNamed(8, Constants.Parameters.START_VALUE));
         _identificationParameter3.Add(parameterNamed(9, Constants.Parameters.MAX_VALUE));
         _identificationParameter3.IsFixed = true;

         _parameterIdentification.AddIdentificationParameter(_identificationParameter1);
         _parameterIdentification.AddIdentificationParameter(_identificationParameter2);
         _parameterIdentification.AddIdentificationParameter(_identificationParameter3);

         _optimizedParameterValue1 = new OptimizedParameterValue("P1", 2.5, 2.1);
         _optimizationRunResult.AddValue(_optimizedParameterValue1);
         _optimizedParameterValue2 = new OptimizedParameterValue("P2", 5.5, 5.2);
         _optimizationRunResult.AddValue(_optimizedParameterValue2);
         //does not exist in PI anymore
         _optimizationRunResult.AddValue(new OptimizedParameterValue("P DOES NOT EXIST", 50, 60));

         A.CallTo(_rangeImageCreator).WithReturnType<Image>().Returns(ApplicationIcons.OK);
      }

      protected override void Because()
      {
         _dto = sut.MapFrom(_parameterIdentification, _runResult);
      }

      private IParameter parameterNamed(double parmaeterValue, string parameterName)
      {
         var parameter = DomainHelperForSpecs.ConstantParameterWithValue(parmaeterValue).WithName(parameterName);
         //change display unit to ensure that we test conversion to display unit
         parameter.DisplayUnit = parameter.Dimension.Units.ElementAt(1);
         return parameter;
      }
   }

   public class When_mapping_some_parameter_identification_run_result_to_run_result_DTO : concern_for_ParameterIdentificationRunResultToRunResultDTOMapper
   {
      [Observation]
      public void should_map_the_run_properties()
      {
         _dto.Description.ShouldBeEqualTo(_runResult.Description);
         _dto.Index.ShouldBeEqualTo(_runResult.Index);
         _dto.NumberOfEvaluations.ShouldBeEqualTo(_runResult.NumberOfEvaluations);
         _dto.TotalError.ShouldBeEqualTo(_runResult.TotalError);
      }

      [Observation]
      public void should_save_a_reference_to_the_original_run_result()
      {
         _dto.RunResult.ShouldBeEqualTo(_runResult);
      }

      [Observation]
      public void should_have_created_one_optimized_parameter_dto_for_each_identification_parameter()
      {
         _dto.OptimizedParameters.Count.ShouldBeEqualTo(3);
         validateIdentificationParameter(_dto.OptimizedParameters[0], _identificationParameter1, _optimizedParameterValue1.Value, _optimizedParameterValue1.StartValue);
         validateIdentificationParameter(_dto.OptimizedParameters[1], _identificationParameter2, _optimizedParameterValue2.Value, _optimizedParameterValue2.StartValue);
         validateIdentificationParameter(_dto.OptimizedParameters[2], _identificationParameter3, _identificationParameter3.StartValue, _identificationParameter3.StartValue);
      }

      private void validateIdentificationParameter(OptimizedParameterDTO optimizedParameterDTO, IdentificationParameter identificationParameter, double optimalValue, double startValue)
      {
         optimizedParameterDTO.Name.ShouldBeEqualTo(identificationParameter.Name);
         optimizedParameterDTO.OptimalValue.DisplayValue.ShouldBeEqualTo(identificationParameter.StartValueParameter.ConvertToDisplayUnit(optimalValue));
         optimizedParameterDTO.StartValue.DisplayValue.ShouldBeEqualTo(identificationParameter.StartValueParameter.ConvertToDisplayUnit(startValue));
         optimizedParameterDTO.MinValue.DisplayValue.ShouldBeEqualTo(identificationParameter.MinValueParameter.ValueInDisplayUnit);
         optimizedParameterDTO.MaxValue.DisplayValue.ShouldBeEqualTo(identificationParameter.MaxValueParameter.ValueInDisplayUnit);
         optimizedParameterDTO.ValueIsCloseToBoundary.ShouldBeFalse();
         optimizedParameterDTO.RangeImage.ShouldNotBeNull();
      }
   }

   public class When_mapping_some_parmaeter_identification_run_result_were_optimal_value_are_close_to_boundaries : concern_for_ParameterIdentificationRunResultToRunResultDTOMapper
   {
      protected override void Context()
      {
         base.Context();
         _optimizationRunResult = new OptimizationRunResult();
         _runResult.BestResult = _optimizationRunResult;

         _optimizationRunResult.AddValue(new OptimizedParameterValue("P1", _identificationParameter1.MinValueParameter.Value * 1.005, 1));
         _optimizationRunResult.AddValue(new OptimizedParameterValue("P2", _identificationParameter2.MaxValueParameter.Value * 0.995, 4));
      }

      [Observation]
      public void should_set_value_is_close_to_boundary_flag_to_true()
      {
         _dto.OptimizedParameters[0].ValueIsCloseToBoundary.ShouldBeTrue();
         _dto.OptimizedParameters[1].ValueIsCloseToBoundary.ShouldBeTrue();
      }
   }
}