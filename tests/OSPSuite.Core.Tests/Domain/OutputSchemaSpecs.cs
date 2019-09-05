using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_OutputSchema : ContextSpecification<OutputSchema>
   {
      protected override void Context()
      {
         sut = new OutputSchema();
      }
   }

   public class When_adding_duplicate_time_points_to_an_output_schema : concern_for_OutputSchema
   {
      private IReadOnlyList<double> _timePoints;

      protected override void Context()
      {
         base.Context();
         sut.AddTimePoint(5);
         sut.AddTimePoint(10);
         sut.AddTimePoint(5);
         sut.AddTimePoint(4);
      }

      protected override void Because()
      {
         _timePoints = sut.TimePoints;
      }

      [Observation]
      public void should_not_add_the_duplicate_points()
      {
         _timePoints.Count.ShouldBeEqualTo(3);
      }

      [Observation]
      public void should_return_the_point_sorted()
      {
         _timePoints.ShouldOnlyContainInOrder(4, 5, 10);
      }
   }

   public class When_retrieving_the_end_time_for_an_output_schema_without_intervals : concern_for_OutputSchema
   {
      [Observation]
      public void should_return_null()
      {
         sut.EndTime.ShouldBeNull();
      }

   }

   public class When_retrieving_the_end_time_for_an_output_schema_with_one_or_more_intervals : concern_for_OutputSchema
   {
      protected override void Context()
      {
         base.Context();
         sut.AddInterval(new OutputInterval {DomainHelperForSpecs.ConstantParameterWithValue(10).WithName(Constants.Parameters.END_TIME)}.WithName("Int1"));
         sut.AddInterval(new OutputInterval {DomainHelperForSpecs.ConstantParameterWithValue(20).WithName(Constants.Parameters.END_TIME)}.WithName("Int2"));
      }

      [Observation]
      public void should_return_the_maximum_endtime_for_an_output_schema_with_multiple_intervals()
      {
         sut.EndTime.ShouldBeEqualTo(20);
      }
   }
}