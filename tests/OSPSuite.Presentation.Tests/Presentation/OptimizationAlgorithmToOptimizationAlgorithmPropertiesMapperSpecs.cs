using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.ParameterIdentifications.Algorithms;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_OptimizationAlgorithmToOptimizationAlgorithmPropertiesMapper : ContextSpecification<OptimizationAlgorithmToOptimizationAlgorithmPropertiesMapper>
   {
      protected override void Context()
      {
         sut = new OptimizationAlgorithmToOptimizationAlgorithmPropertiesMapper();
      }
   }

   public class when_mapping_from_an_optimization_algorithm_to_a_identification_algorithm : concern_for_OptimizationAlgorithmToOptimizationAlgorithmPropertiesMapper
   {
      private OptimizationAlgorithmProperties _result;
      private IOptimizationAlgorithm _optimizationAlgorithm;
      private OptimizationAlgorithmProperties _cloneOfAlgoProperties;

      protected override void Context()
      {
         base.Context(); 
         _optimizationAlgorithm = A.Fake<IOptimizationAlgorithm>();
         _cloneOfAlgoProperties = new OptimizationAlgorithmProperties("CLONE");
         A.CallTo(() => _optimizationAlgorithm.Properties.Clone()).Returns(_cloneOfAlgoProperties);
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_optimizationAlgorithm);
      }

      [Observation]
      public void should_return_a_clone_of_properties_defined_in_the_optimization_algorithm()
      {
         _result.ShouldBeEqualTo(_cloneOfAlgoProperties);
      }
   }
}