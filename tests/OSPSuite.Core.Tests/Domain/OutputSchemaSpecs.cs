using System.Collections.Generic;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Services;
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
         sut.AddInterval(new OutputInterval { DomainHelperForSpecs.ConstantParameterWithValue(10).WithName(Constants.Parameters.END_TIME) }.WithName("Int1"));
         sut.AddInterval(new OutputInterval { DomainHelperForSpecs.ConstantParameterWithValue(20).WithName(Constants.Parameters.END_TIME) }.WithName("Int2"));
      }

      [Observation]
      public void should_return_the_maximum_end_time_for_an_output_schema_with_multiple_intervals()
      {
         sut.EndTime.ShouldBeEqualTo(20);
      }
   }

   public class When_updating_properties_from_time_point_schema : concern_for_OutputSchema
   {
      private OutputSchema _sourceSchema;
      private ICloneManager _cloneManager;

      protected override void Context()
      {
         base.Context();
         _cloneManager = A.Fake<ICloneManager>();
         _sourceSchema = new OutputSchema();
         _sourceSchema.AddTimePoint(1);
         _sourceSchema.AddTimePoint(2);
         _sourceSchema.AddTimePoint(3);
      }

      protected override void Because()
      {
         sut.UpdatePropertiesFrom(_sourceSchema, _cloneManager);
      }

      [Observation]
      public void the_updated_schema_should_contain_the_time_points()
      {
         sut.TimePoints.ShouldOnlyContain(1d, 2d, 3d);
      }
   }

   public class When_clearing_an_output_schema : concern_for_OutputSchema
   {
      protected override void Context()
      {
         base.Context();
         sut.AddInterval(new OutputInterval { DomainHelperForSpecs.ConstantParameterWithValue(10).WithName(Constants.Parameters.END_TIME) }.WithName("Int1"));
         sut.AddTimePoint(5);
      }

      protected override void Because()
      {
         sut.Clear();
      }

      [Observation]
      public void should_have_removed_all_intervals_and_single_time_points()
      {
         sut.Intervals.ShouldBeEmpty();
         sut.TimePoints.ShouldBeEmpty();
      }
   }
}