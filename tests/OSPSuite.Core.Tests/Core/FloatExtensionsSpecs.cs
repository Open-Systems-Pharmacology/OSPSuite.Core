using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core
{
   public class When_checking_if_float_values_are_valid : StaticContextSpecification
   {
      [Observation]
      public void should_evaluate_values()
      {
         0.5F.IsValid().ShouldBeEqualTo(true);
         0F.IsValid().ShouldBeEqualTo(true);
         (-0.5F).IsValid().ShouldBeEqualTo(true);
         float.NaN.IsValid().ShouldBeEqualTo(false);
         float.NegativeInfinity.IsValid().ShouldBeEqualTo(false);
         float.PositiveInfinity.IsValid().ShouldBeEqualTo(false);
         float.MinValue.IsValid().ShouldBeEqualTo(true);
         float.MaxValue.IsValid().ShouldBeEqualTo(true);
      }
   }

   public class When_purifying_a_float_arrays : StaticContextSpecification
   {
      [Observation]
      public void should_return_a_sorted_array_where_invalid_values_are_removed()
      {
         new[] {1f, float.NaN, 3f, float.PositiveInfinity, 2f}.OrderedAndPurified().ShouldOnlyContainInOrder(1f, 2f, 3f);
      }
   }
}