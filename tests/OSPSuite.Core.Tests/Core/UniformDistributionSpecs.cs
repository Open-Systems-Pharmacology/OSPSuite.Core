using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Maths.Random;
using OSPSuite.Core.Maths.Statistics;

namespace OSPSuite.Core
{
   public abstract class concern_for_UniformDistribution : ContextSpecification<UniformDistribution>
   {
      protected RandomGenerator _randomGenerator;

      protected override void Context()
      {
         _randomGenerator = new RandomGenerator();
         sut = new UniformDistribution(10,14);
      }
   }

   public class When_generating_a_value_outside_of_the_interval : concern_for_UniformDistribution
   {
  
      [Observation]
      public void should_throw_an_exception_when_the_interval_is_bigger()
      {
         The.Action(() => sut.RandomDeviate(_randomGenerator, 15, 20)).ShouldThrowAn<LimitsNotInUniformDistributionFunctionRangeException>();
      }


      [Observation]
      public void should_throw_an_exception_when_the_interval_is_smaller()
      {
         The.Action(() => sut.RandomDeviate(_randomGenerator, 0, 8)).ShouldThrowAn<LimitsNotInUniformDistributionFunctionRangeException>();
      }
   }

   public class When_generating_a_value_in_the_interval : concern_for_UniformDistribution
   {

      [Observation]
      public void the_generated_value_should_be_in_the_correct_interval()
      {
         var value = sut.RandomDeviate(_randomGenerator, 0, 12);
         value.ShouldBeGreaterThan(10);
         value.ShouldBeSmallerThan(12);
      
         value = sut.RandomDeviate(_randomGenerator, 12, 20);
         value.ShouldBeGreaterThan(12);
         value.ShouldBeSmallerThan(14);
      }

   }

   public class When_generating_a_value_at_the_edge_of_the_interval : concern_for_UniformDistribution
   {

      [Observation]
      public void should_return_the_edge_value()
      {
         var value = sut.RandomDeviate(_randomGenerator, 14, 20);
         value.ShouldBeEqualTo(14);
         
         value = sut.RandomDeviate(_randomGenerator, 8, 10);
         value.ShouldBeEqualTo(10);
      }

   }
}	