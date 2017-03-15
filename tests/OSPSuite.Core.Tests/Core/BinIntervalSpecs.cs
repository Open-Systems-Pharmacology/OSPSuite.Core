using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using NUnit.Framework;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core
{
   public abstract class concern_for_BinInterval : ContextSpecification<BinInterval>
   {
      protected double _min;
      protected double _max;
   }

   public class When_initializing_a_bin_interval_with_a_min_and_max_value_equal : concern_for_BinInterval
   {
      protected override void Context()
      {
         _min = 6;
         _max = 6;
         sut = new BinInterval(_min, _max);
      }

      [Observation]
      public void should_return_that_a_value_is_in_the_bin_interval_only_if_the_value_is_equal_to_the_min()
      {
         sut.Contains(6).ShouldBeTrue();
         sut.Contains(5).ShouldBeFalse();
      }

      [Observation]
      public void the_value_should_be_equal_to_the_min()
      {
         sut.MeanValue.ShouldBeEqualTo(_min);
      }

      [Observation]
      public void the_interval_should_be_constant()
      {
         sut.IsConstant.ShouldBeTrue();
      }
   }

   public class When_initializing_a_bin_interval_with_a_min_and_max_value_equal_by_tolerance : concern_for_BinInterval
   {
      protected override void Context()
      {
         _min = 6;
         _max = 6.001;
         sut = new BinInterval(_min, _max);
      }

      [Observation]
      public void should_return_that_a_value_is_in_the_bin_interval_only_if_the_value_is_equal_to_the_min()
      {
         sut.Contains(6).ShouldBeTrue();
         sut.Contains(5).ShouldBeFalse();
      }

      [Observation]
      public void the_value_should_be_equal_to_the_min()
      {
         sut.MeanValue.ShouldBeEqualTo(_min);
      }

      [Observation]
      public void the_interval_should_be_constant()
      {
         sut.IsConstant.ShouldBeTrue();
      }

      [Observation]
      public void should_contain_the_min_and_max_value()
      {
         sut.Contains(_max).ShouldBeTrue();
      }

   }

   public class When_initializing_a_bin_interval_with_a_min_and_max_value_distinct : concern_for_BinInterval
   {
      protected override void Context()
      {
         _min = 2;
         _max = 6;
         sut = new BinInterval(_min, _max);
      }

      [Observation]
      public void should_return_that_a_value_is_in_the_bin_interval_only_if_the_value_is_superior_or_equal_to_the_min_but_strictly_inferior_to_the_max()
      {
         sut.Contains(4).ShouldBeTrue();
         sut.Contains(2).ShouldBeTrue();
         sut.Contains(6).ShouldBeFalse();
         sut.Contains(7).ShouldBeFalse();
      }

      [Observation]
      public void the_value_should_be_equal_to_the_mean_of_the_inteval_min_and_max()
      {
         sut.MeanValue.ShouldBeEqualTo((_min + _max) / 2);
      }

 
      [Observation]
      public void the_interval_should_not_be_constant()
      {
         sut.IsConstant.ShouldBeFalse();
      }
   }

   public class When_initializing_a_bin_interval_with_a_min_and_max_value_with_max_allowed : concern_for_BinInterval
   {
      protected override void Context()
      {
         _min = 2;
         _max = 6;
         sut = new BinInterval(_min, _max, maxAllowed:true);
      }

      [TestCase(1.9, false)]
      [TestCase(2, true)]
      [TestCase(4, true)]
      [TestCase(6, true)]
      [TestCase(6.1, false)]
      [Observation]
      public void check_if_interval_contains_value(double value, bool isIn)
      {
         if (isIn)
            sut.Contains(value).ShouldBeTrue();
         else
            sut.Contains(value).ShouldBeFalse();
      }

      [Observation]
      public void the_value_should_be_equal_to_the_mean_of_the_inteval_min_and_max()
      {
         sut.MeanValue.ShouldBeEqualTo((_min + _max) / 2);
      }


      [Observation]
      public void the_interval_should_not_be_constant()
      {
         sut.IsConstant.ShouldBeFalse();
      }
   }
}