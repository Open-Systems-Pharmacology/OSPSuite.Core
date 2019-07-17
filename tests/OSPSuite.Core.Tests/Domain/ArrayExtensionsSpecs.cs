using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Domain
{
   public  class When_checking_that_two_arrays_are_equal : StaticContextSpecification
   {
      private float[] _array1;
      private float[] _array2;
      private float[] _array3;
      private float[] _array4;
      private float[] _array5;
      private float[] _array6;

      protected override void Context()
      {
         _array1 = new float[] { 1, 2, 3 };
         _array2 = new float[] { 1, 2, 3, 4 };
         _array3 = new float[] { 2, 3, 4 };
         _array4 = new float[] { 1, 2, 3 };
         _array5 = new float[] { 0.0001f, 0.0002f};
         _array6 = new float[] { 0.0002f, 0.0003f };
      }

      [Observation]
      public void should_return_true_if_the_arrays_are_indeed_equal()
      {
         _array1.IsEqual(_array4).ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_if_the_arrays_have_different_length()
      {
         _array1.IsEqual(_array2).ShouldBeFalse();
      }

      [Observation]
      public void should_return_false_if_the_arrays_have_different_values()
      {
         _array1.IsEqual(_array3).ShouldBeFalse();
      }

      [Observation]
      public void should_return_false_if_the_absolute_values_difference_is_bigger_than_the_given_tolerance()
      {
         _array5.IsEqual(_array6,0.001f).ShouldBeTrue();
         _array5.IsEqual(_array6,0.000001f).ShouldBeFalse();
         _array5.IsEqual(_array6).ShouldBeFalse();
      }

      [Observation]
      public void should_return_false_if_one_array_is_null()
      {
         _array1.IsEqual(null).ShouldBeFalse();
         ArrayExtensions.IsEqual(null,_array1).ShouldBeFalse();
      }
   }

   public class When_transforming_an_array_into_a_path_string : StaticContextSpecification
   {
      [Observation]
      public void should_return_a_string_containg_all_the_items_in_the_array_concatenated_with_the_object_path_delimiter()
      {
         new []{"a","b","c"}.ToPathString().ShouldBeEqualTo("a|b|c");
      }
   }

}	