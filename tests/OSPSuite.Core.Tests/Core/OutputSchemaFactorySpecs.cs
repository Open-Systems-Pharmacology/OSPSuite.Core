using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core
{
   public abstract class concern_for_OutputSchemaFactory : ContextSpecification<IOutputSchemaFactory>
   {
      private IObjectBaseFactory _objectBaseFactory;
      protected IOutputIntervalFactory _outputIntervalFactory;
      private OutputSchema _outputSchema;
      protected OutputInterval _defaultInterval;

      protected override void Context()
      {
         _outputSchema = new OutputSchema();
         _defaultInterval = new OutputInterval();
         _objectBaseFactory = A.Fake<IObjectBaseFactory>();
         _outputIntervalFactory = A.Fake<IOutputIntervalFactory>();
         A.CallTo(() => _objectBaseFactory.Create<OutputSchema>()).Returns(_outputSchema);
         A.CallTo(() => _outputIntervalFactory.CreateDefault()).Returns(_defaultInterval);
         sut = new OutputSchemaFactory(_objectBaseFactory, _outputIntervalFactory);
      }
   }

   public class When_creating_a_default_output_schema : concern_for_OutputSchemaFactory
   {
      private OutputSchema _result;

      protected override void Because()
      {
         _result = sut.CreateDefault();
      }

      [Observation]
      public void should_use_the_default_interval()
      {
         _result.Intervals.ShouldContain(_defaultInterval);
      }
   }

   public class When_creating_a_output_schema_for_given_start_time_end_time_and_resolution : concern_for_OutputSchemaFactory
   {
      private OutputSchema _result;
      private double _min = 0;
      private double _max = 20;
      private double _resolution = 5;
      private OutputInterval _specificInterval;

      protected override void Context()
      {
         base.Context();
         _specificInterval = new OutputInterval();
         A.CallTo(() => _outputIntervalFactory.Create(_min, _max, _resolution)).Returns(_specificInterval);
      }

      protected override void Because()
      {
         _result = sut.Create(_min, _max, _resolution);
      }

      [Observation]
      public void should_use_the_default_interval()
      {
         _result.Intervals.ShouldContain(_specificInterval);
      }
   }
}