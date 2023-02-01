using OSPSuite.BDDHelper;
using OSPSuite.Core.Maths;
using OSPSuite.BDDHelper.Extensions;

namespace OSPSuite.Core.Domain
{
   public class concern_for_SortedFloatArray : ContextSpecification<SortedFloatArray>
   {

   }

   public class When_calculating_the_median_of_an_unsorted_array : concern_for_SortedFloatArray
   {
      protected override void Context()
      {
         sut = new SortedFloatArray(new float[] { 1, 2, 7, 8, 5 }, false);
      }

      [Observation]
      public void should_return_the_expected_value()
      {
         ((double)sut.Median()).ShouldBeEqualTo(5);
      }
   }

   public class When_calculating_the_median_of_an_array : concern_for_SortedFloatArray
   {
      private SortedFloatArray _values1;
      private SortedFloatArray _values2;

      protected override void Context()
      {
         _values1 = new SortedFloatArray(new float[] { 1, 2, 5, 8, 7 }, true);
         _values2 = new SortedFloatArray(new float[] { 1, 2, 2, 6, 8, 7 }, true);
      }

      [Observation]
      public void should_return_the_expected_value()
      {
         ((double)_values1.Median()).ShouldBeEqualTo(5);
         ((double)_values2.Median()).ShouldBeEqualTo(4);
      }
   }

   public class When_calculating_the_percentile : concern_for_SortedFloatArray
   {
      protected override void Context()
      {
         sut = new SortedFloatArray(new[] { 0f, 1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f, 9f, 10f }, true);
      }

      [Observation]
      public void should_return_the_expected_value()
      {
         ((double)sut.Percentile(50)).ShouldBeEqualTo(sut.Median(), 1e-5);
      }
   }

   public class When_calculating_the_percentile_with_doubled_values : concern_for_SortedFloatArray
   {
      protected override void Context()
      {
         sut = new SortedFloatArray(new[] { 0f, 1f, 1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f, 9f, 10f }, true);
      }

      [Observation]
      public void should_return_the_expected_value()
      {
         ((double)sut.Percentile(50)).ShouldBeEqualTo(sut.Median(), 1e-5);
      }
   }

   public class When_calculating_the_quantile_with_doubled_values : concern_for_SortedFloatArray
   {
      private SortedFloatArray _values;
      private SortedFloatArray _values2;

      protected override void Context()
      {
         _values = new SortedFloatArray(new[] { 0f, 1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f, 9f, 10f }, true);
         _values2 = new SortedFloatArray(new[] { 2f, 3f, 3f, 4f, 5f, 6f }, true);
      }

      [Observation]
      public void should_return_the_expected_median()
      {
         ((double)_values.Quantile(0.5)).ShouldBeEqualTo(5f);
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

   public class When_calculating_the_quantile_for_an_array_with_only_one_value : concern_for_SortedFloatArray
   {
      [Observation]
      public void should_return_the_expected_values()
      {
         new SortedFloatArray(new[] { 1f }, true).Quantile(0.5).ShouldBeEqualTo(1f);
         new SortedFloatArray(new[] { 5f }, true).Quantile(0.5).ShouldBeEqualTo(5f);
      }
   }
}
