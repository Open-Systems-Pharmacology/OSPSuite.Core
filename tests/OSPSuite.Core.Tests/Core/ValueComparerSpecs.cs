using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
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
      [TestCase(double.NaN, 10d, false)]
      [TestCase(float.NaN, 10f, false)]
      [TestCase(10d, double.NaN, false)]
      [TestCase(10f, float.NaN, false)]
      [TestCase(1d, 1d + Constants.DOUBLE_RELATIVE_EPSILON / 2, true)]
      [TestCase(1d, 1d + Constants.DOUBLE_RELATIVE_EPSILON * 2, false)]
      public void should_return_the_expected_result_using_the_default_tolerance(double d1, double d2, bool areEqual)
      {
         ValueComparer.AreValuesEqual(d1, d2).ShouldBeEqualTo(areEqual);
      }
   }

   public class When_assessing_if_two_parameters_have_equal_values : concern_for_ValueComparer
   {
      private Parameter _parameter1;
      private Parameter _parameter2;

      protected override void Context()
      {
         base.Context();
         _parameter1 = new Parameter().WithFormula(new ConstantFormula(0));
         _parameter2 = new Parameter().WithFormula(new ConstantFormula(0));
      }

      [Observation]
      public void should_return_true_if_both_parameter_are_null()
      {
         ValueComparer.AreValuesEqual(null, null).ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_if_only_one_parameter_is_null()
      {
         ValueComparer.AreValuesEqual(_parameter1, null).ShouldBeFalse();
         ValueComparer.AreValuesEqual(null, _parameter2).ShouldBeFalse();
      }

      [Observation]
      public void should_return_true_if_the_parametesr_have_the_same_value()
      {
         ValueComparer.AreValuesEqual(_parameter1, _parameter2).ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_if_the_parameters_do_not_have_the_same_value()
      {
         _parameter1.Value = 20;
         ValueComparer.AreValuesEqual(_parameter1, _parameter2).ShouldBeFalse();
      }
   }

   public class When_assessing_if_one_parameter_has_a_given_value : concern_for_ValueComparer
   {
      private Parameter _parameter;

      protected override void Context()
      {
         base.Context();
         _parameter = new Parameter().WithFormula(new ConstantFormula(10));
      }

      [Observation]
      public void should_return_false_if_the_parameter_is_null()
      {
         ValueComparer.AreValuesEqual(_parameter, 20).ShouldBeFalse();
      }

      [Observation]
      public void should_return_true_if_the_parametesr_has_the_given_value()
      {
         ValueComparer.AreValuesEqual(_parameter, _parameter.Value).ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_if_the_parameter_does_not_have_the_same_value()
      {
         ValueComparer.AreValuesEqual(_parameter, 40).ShouldBeFalse();
      }
   }
}