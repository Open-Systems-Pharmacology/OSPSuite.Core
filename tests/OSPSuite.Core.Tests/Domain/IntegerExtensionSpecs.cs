using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Domain
{
   public class concern_for_IntegerExtensions : StaticContextSpecification
   {
      [Observation]
      public void should_round_an_even_multiple_to_the_same_multiple()
      {
         16.RoundUpToMultiple(8).ShouldBeEqualTo(16);
      }

      [Observation]
      public void should_round_a_negative_number_up()
      {
         (-15).RoundUpToMultiple(8).ShouldBeEqualTo(-8);
      }

      [Observation]
      public void should_round_a_positive_number_close_to_zero_up()
      {
         1.RoundUpToMultiple(8).ShouldBeEqualTo(8);
      }

      [Observation]
      public void should_round_0_to_0()
      {
         0.RoundUpToMultiple(8).ShouldBeEqualTo(0);
      }

      [Observation]
      public void when_rounding_any_number_and_the_multiple_is_0_should_round_to_the_original_number()
      {
         5.RoundUpToMultiple(0).ShouldBeEqualTo(5);
      }
   }
}
