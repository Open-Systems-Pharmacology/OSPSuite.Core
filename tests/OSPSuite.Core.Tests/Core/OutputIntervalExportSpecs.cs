using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Serialization.SimModel.DTO;

namespace OSPSuite.Core
{
   public abstract class concern_for_OutputIntervalExport : ContextSpecification<OutputIntervalExport>
   {
      protected override void Context()
      {
         sut = new OutputIntervalExport();
         sut.EndTime = 60;
         sut.StartTime = 0;
      }
   }

   internal class When_have_regular_definition : concern_for_OutputIntervalExport
   {
      protected override void Because()
      {
         sut.Resolution = 5.0 / 60;
      }

      [Observation]
      public void should_return_correct_number_of_time_points()
      {
         sut.NumberOfTimePoints.ShouldBeEqualTo(6);
      }
   }

   internal class When_have_to_low_resolution : concern_for_OutputIntervalExport
   {
      protected override void Because()
      {
         sut.Resolution = 1 / 60;
      }

      [Observation]
      public void should_return_min_number_of_time_points()
      {
         sut.NumberOfTimePoints.ShouldBeEqualTo(Constants.MIN_NUMBER_OF_POINTS_PER_INTERVAL);
      }
   }

   internal class When_have_to_high_resolution : concern_for_OutputIntervalExport
   {
      protected override void Because()
      {
         sut.Resolution = 100000;
      }

      [Observation]
      public void should_return_max_number_of_time_points()
      {
         sut.NumberOfTimePoints.ShouldBeEqualTo(Constants.MAX_NUMBER_OF_POINTS_PER_INTERVAL);
      }
   }

   public class When_calculating_the_number_of_time_points_for_a_resolution_using_that_has_a_limited_number_of_digits : concern_for_OutputIntervalExport
   {
      protected override void Context()
      {
         base.Context();
         sut.EndTime = 120;
         sut.Resolution = 0.333333333; //20 pts/h
      }

      [Observation]
      public void should_go_to_the_top_integer()
      {
         sut.NumberOfTimePoints.ShouldBeEqualTo(41);
      }
   }
}