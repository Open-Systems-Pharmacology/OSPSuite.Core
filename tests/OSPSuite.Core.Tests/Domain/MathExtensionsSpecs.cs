using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Extensions;

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

   public class When_calculating_the_median_of_an_array : StaticContextSpecification
   {
      private float[] _values1;
      private float[] _values2;

      protected override void Context()
      {
         _values1 = new float[] {1, 2, 5, 8, 7};
         _values2 = new float[] {1, 2, 2, 6, 8, 7};
      }

      [Observation]
      public void should_return_the_expected_value()
      {
         ((double) _values1.Median()).ShouldBeEqualTo(5);
         ((double) _values2.Median()).ShouldBeEqualTo(4);
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

   public class When_calculating_the_percentile : StaticContextSpecification
   {
      private float[] _values;

      protected override void Context()
      {
         _values = new float[] {0f, 1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f, 9f, 10f};
      }

      [Observation]
      public void should_return_the_expected_value()
      {
         ((double) _values.Percentile(50)).ShouldBeEqualTo(_values.Median(), 1e-5);
      }
   }

   public class When_calculating_the_percentile_with_doubled_values : StaticContextSpecification
   {
      private float[] _values;

      protected override void Context()
      {
         _values = new float[] {0f, 1f, 1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f, 9f, 10f};
      }

      [Observation]
      public void should_return_the_expected_value()
      {
         ((double) _values.Percentile(50)).ShouldBeEqualTo(_values.Median(), 1e-5);
      }
   }

   public class When_calculating_the_quantile_with_doubled_values : StaticContextSpecification
   {
      private float[] _values;
      private float[] _values2;

      protected override void Context()
      {
         _values = new[] {0f, 1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f, 9f, 10f};
         _values2 = new[] {2f, 3f, 3f, 4f, 5f, 6f};
      }

      [Observation]
      public void should_return_the_expected_median()
      {
         ((double) _values.Quantile(0.5)).ShouldBeEqualTo(5f);
      }

      [Observation]
      public void should_calculate_Median()
      {
         _values.Quantile(0.5).ShouldBeEqualTo(5f);
      }

      [Observation]
      public void should_calculate_2_5Percentile()
      {
         _values.Quantile(0.025).ShouldBeEqualTo(0.25f);
      }

      [Observation]
      public void should_calculate_5Percentile()
      {
         _values.Quantile(0.05).ShouldBeEqualTo(0.5f);
      }

      [Observation]
      public void should_calculate_25Percentile()
      {
         _values.Quantile(0.25).ShouldBeEqualTo(2.5f);
      }

      [Observation]
      public void should_calculate_75Percentile()
      {
         _values.Quantile(0.75).ShouldBeEqualTo(7.5f);
      }

      [Observation]
      public void should_calculate_Percentiles()
      {
         _values.Quantile(0.83).ShouldBeEqualTo(8.3f);
         _values.Quantile(0.8).ShouldBeEqualTo(8f);

         _values2.Quantile(0.24).ShouldBeEqualTo(3f);
         _values2.Quantile(0).ShouldBeEqualTo(2f);
         _values2.Quantile(0.05).ShouldBeEqualTo(2.25f);
         _values2.Quantile(0.95).ShouldBeEqualTo(5.75f);
      }
   }

   public class When_calculating_the_quantile_for_an_array_with_only_one_value : StaticContextSpecification
   {
      [Observation]
      public void should_return_the_expected_values()
      {
         new[] {1f}.Quantile(0.5).ShouldBeEqualTo(1f);
         new[] {5f}.Quantile(0.5).ShouldBeEqualTo(5f);
      }
   }
}