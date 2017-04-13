using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Extensions;
using FakeItEasy;
using NUnit.Framework;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Maths.Random;

namespace OSPSuite.Core
{
   public abstract class concern_for_BinIntervalsCreator : ContextSpecification<IBinIntervalsCreator>
   {
      protected ICoreUserSettings _userSettings;
      protected List<double> _allValues;
      protected IReadOnlyList<BinInterval> _intervals;
      protected int _numberOfElementsPerBin;
      protected int _numberOfBins;

      protected override void Context()
      {
         _userSettings = A.Fake<ICoreUserSettings>();
         _allValues = new List<double>();
         sut = new BinIntervalsCreator(_userSettings);
      }

      protected override void Because()
      {
         _intervals = sut.CreateIntervalsFor(_allValues, _numberOfElementsPerBin, _numberOfBins);
      }

      protected IEnumerable<double> RandomValues(double min, double max, int count)
      {
         var random = new RandomGenerator();
         for (int i = 0; i < count; i++)
         {
            yield return random.UniformDeviate(min, max);
         }
      }


      protected void VerifyIntervalContent(int expectedNumberOfIntervals)
      {
         _intervals.Count.ShouldBeEqualTo(expectedNumberOfIntervals);
         var orderedValues = _allValues.OrderBy(x => x).ToList();
         for (int binIndex = 0; binIndex < _intervals.Count; binIndex++)
         {
            for (int i = 0; i < orderedValues.Count / _intervals.Count; i++)
            {
               double value = orderedValues[(orderedValues.Count / _intervals.Count) * binIndex + i];
               _intervals[binIndex].Contains(value).ShouldBeTrue($"BinIndex = {binIndex}, value = {value}");
            }
         }
      }
   }

   public class When_retriving_the_bin_intervals_for_a_set_of_parameter_values_with_are_relatively_constant : concern_for_BinIntervalsCreator
   {
      protected override void Context()
      {
         base.Context();
         _allValues.AddRange(new[] {0.80, 0.79990001});
         _numberOfElementsPerBin = 2;
         _numberOfBins = 5;
      }

      [Observation]
      public void should_have_returned_one_interval()
      {
         _intervals.Count.ShouldBeEqualTo(1);
      }
   }

   public class When_retrieving_the_bin_intervals_for_a_set_of_values_bigger_than_the_number_of_bins_time_the_number_of_individual_per_bins : concern_for_BinIntervalsCreator
   {
      protected override void Context()
      {
         base.Context();
         _allValues.AddRange(new[] {1.0,  1.1, 2, 5, 4, 1.2});
         _numberOfElementsPerBin = 3;
         _numberOfBins = 2;
      }

      [Test]
      public void should_create_the_expected_number_of_bins()
      {
         _intervals.Count.ShouldBeEqualTo(2);
      }

      [Observation]
      public void should_be_able_to_retrieve_the_orignal_values_in_the_created_intervals()
      {
         _intervals[0].Contains(1.0).ShouldBeTrue();
         _intervals[0].Contains(1.1).ShouldBeTrue();
         _intervals[0].Contains(1.2).ShouldBeTrue();
         _intervals[1].Contains(2).ShouldBeTrue();
         _intervals[1].Contains(4).ShouldBeTrue();
         _intervals[1].Contains(5).ShouldBeTrue();
      }

      [Observation]
      public void the_created_intervals_should_not_contain_the_minimum_values_except_for_the_first_elemet()
      {
         _intervals[1].Contains(1.2).ShouldBeFalse();
      }

   }

   public class When_retriving_the_bin_intervals_for_a_set_of_parameter_values : concern_for_BinIntervalsCreator
   {
      protected override void Context()
      {
         base.Context();
         _allValues.AddRange(new[] {0.77, 0.99});
         _numberOfElementsPerBin = 1;
         _numberOfBins = 2;
      }

      [Observation]
      public void the_number_of_returned_interval_should_be_equal_to_the_value_defined_in_the_settings()
      {
         _intervals.Count.ShouldBeEqualTo(_numberOfBins);
      }

      [Observation]
      public void should_be_able_to_retrieve_the_orignal_values_in_the_created_intervals()
      {
         _intervals.ElementAt(0).Contains(0.77).ShouldBeTrue();
         _intervals.ElementAt(1).Contains(0.99).ShouldBeTrue();
      }

      [Observation]
      public void only_values_in_range_should_be_within_the_created_intervals()
      {
         _intervals.ElementAt(0).Contains(0.76).ShouldBeFalse();
         _intervals.ElementAt(1).Contains(1.00).ShouldBeFalse();
      }
   }


   public class When_creating_bin_invervals_for_a_number_of_element_superior_as_the_desired_number_of_elements_per_bin : concern_for_BinIntervalsCreator
   {
      protected override void Context()
      {
         base.Context();
         //100 between 0 and 10
         _allValues.AddRange(RandomValues(0, 10, 100));
         //100 between 10 and 100
         _allValues.AddRange(RandomValues(10, 100, 100));
         //50 between 100 and 1000
         _allValues.AddRange(RandomValues(10, 1000, 50));
         
         _numberOfElementsPerBin = 100;
         _numberOfBins = 2;
      }

      [Observation]
      public void should_fill_at_least_the_desired_number_of_elements_in_each_bin()
      {
         //expectation is 2 intervals containing 125 values each
         VerifyIntervalContent(2);
      }
   }

   public class When_creating_bin_invervals_for_a_number_of_element_resulting_in_less_bin_as_expected : concern_for_BinIntervalsCreator
   {
      protected override void Context()
      {
         base.Context();
         //100 between 0 and 10
         _allValues.AddRange(RandomValues(0, 10, 100));
         //100 between 10 and 100
         _allValues.AddRange(RandomValues(10, 100, 100));
    
         _numberOfElementsPerBin = 50;
         _numberOfBins = 5;
      }

      [Observation]
      public void should_have_created_less_bin_than_expected_but_fill_each_bin_with_the_expected_number_of_values()
      {
         //expectation is 4 intervals containing 50 values each
         VerifyIntervalContent(4);
      }
   }
}