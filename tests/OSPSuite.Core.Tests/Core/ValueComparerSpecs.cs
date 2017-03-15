using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using NUnit.Framework;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core
{
   public abstract class concern_for_ValueComparer : StaticContextSpecification
   {
   }

   public class When_assessing_if_to_double_values_are_equal : concern_for_ValueComparer
   {
      [TestCase(0, 0, true)]
      [TestCase(double.NaN, double.NaN, true)]
      [TestCase(double.NaN, 10, false)]
      [TestCase(1d, 1d + Constants.DOUBLE_RELATIVE_EPSILON / 2, true)]
      [TestCase(1d, 1d + Constants.DOUBLE_RELATIVE_EPSILON * 2, false)]
      public void should_return_the_expected_result_using_the_default_tolerance(double d1, double d2, bool areEqual)
      {
         ValueComparer.AreValuesEqual(d1, d2).ShouldBeEqualTo(areEqual);
      }
   }
}