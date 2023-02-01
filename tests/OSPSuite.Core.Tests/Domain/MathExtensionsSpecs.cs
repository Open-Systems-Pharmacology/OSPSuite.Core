using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Maths;

namespace OSPSuite.Core.Domain
{
   public class When_calculating_the_standard_deviation_of_an_array : StaticContextSpecification
   {
      private float[] _values;

      protected override void Context()
      {
         _values = new float[] {2, 4, 4, 4, 5, 5, 7, 9};
      }

      [Observation]
      public void should_return_the_expected_value()
      {
         ((double) _values.ArithmeticStandardDeviation()).ShouldBeEqualTo(2.138089, 1e-5);
      }
   }

   public class When_calculating_the_mean_of_an_array : StaticContextSpecification
   {
      private float[] _values;

      protected override void Context()
      {
         _values = new float[] {2, 4, 4, 4, 5, 5, 7, 9};
      }

      [Observation]
      public void should_return_the_expected_value()
      {
         _values.ArithmeticMean().ShouldBeEqualTo(5);
      }
   }

   public class When_calculating_the_absolute_values_of_an_array : StaticContextSpecification
   {
      private float[] _values;

      protected override void Context()
      {
         _values = new float[] {2, -4, -4, 7, -9};
      }

      [Observation]
      public void should_return_the_expected_value()
      {
         _values.AbsoluteValues().ShouldOnlyContainInOrder(2, 4, 4, 7, 9);
      }
   }

   public class When_calculating_the_mean_of_an_array_with_NAN : StaticContextSpecification
   {
      private float[] _values;

      protected override void Context()
      {
         _values = new[] {2, 4, 4, 4, 5, 5, 7, 9, float.NaN};
      }

      [Observation]
      public void should_return_the_expected_value()
      {
         _values.ArithmeticMean().ShouldBeEqualTo(float.NaN);
      }
   }

   public class When_calculating_the_geometric_std : StaticContextSpecification
   {
      private float[] _values;

      protected override void Context()
      {
         _values = new float[] {1.15f, 0.9f, 1.22f, 1.3f};
      }

      [Observation]
      public void should_return_the_expected_value()
      {
         ((double) _values.GeometricStandardDeviation()).ShouldBeEqualTo(1.1745, 1e-5);
      }
   }

   public class When_calculating_the_geometric_mean : StaticContextSpecification
   {
      private float[] _values;

      protected override void Context()
      {
         _values = new float[] {1.80f, 1.166666f, 1.428571f};
      }

      [Observation]
      public void should_return_the_expected_value()
      {
         ((double) _values.GeometricMean()).ShouldBeEqualTo(1.442249, 1e-5);
      }
   }

   public class When_calculating_the_geometric_mean_of_an_array_with_zeros : StaticContextSpecification
   {
      private float[] _values;

      protected override void Context()
      {
         _values = new float[] { 0f, 0f};
      }

      [Observation]
      public void should_return_the_expected_value()
      {
         ((double)_values.GeometricMean()).ShouldBeEqualTo(0f);
      }
   }

   public class When_checking_if_all_values_in_an_array_are_zero_: StaticContextSpecification
   {
      [Observation]
      public void should_return_true_if_the_array_only_contains_zero()
      {
         new[] { 0f, 0f}.AllValuesAreZero().ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_otherwise()
      {
         new[] { 1.80f, 1.166666f, 1.428571f }.AllValuesAreZero().ShouldBeFalse();
         new[] { 0f, float.NaN, 0f }.AllValuesAreZero().ShouldBeFalse();
      }
   }

   
}