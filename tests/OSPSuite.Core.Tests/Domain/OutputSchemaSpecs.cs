using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;

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
}	