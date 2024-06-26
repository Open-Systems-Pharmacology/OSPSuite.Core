﻿using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_OutputIntervalFactory : ContextSpecification<IOutputIntervalFactory>
   {
      protected IObjectBaseFactory _objectBaseFactory;
      private IDimensionFactory _dimensionFactory;
      protected IContainerTask _containerTask;

      protected override void Context()
      {
         _objectBaseFactory = A.Fake<IObjectBaseFactory>();
         _dimensionFactory = A.Fake<IDimensionFactory>();
         A.CallTo(() => _objectBaseFactory.Create<IParameter>()).ReturnsLazily(x=>new Parameter());
         A.CallTo(() => _dimensionFactory.Dimension(A<string>._)).Returns(DomainHelperForSpecs.ConcentrationDimensionForSpecs());
         _containerTask = A.Fake<IContainerTask>();
         sut = new OutputIntervalFactory(_objectBaseFactory, _dimensionFactory, _containerTask);
      }
   }

   public class When_creating_an_interval_for_given_start_end_and_resolution_value : concern_for_OutputIntervalFactory
   {
      private OutputInterval _interval;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _objectBaseFactory.Create<OutputInterval>()).Returns(new OutputInterval());
         A.CallTo(() => _objectBaseFactory.Create<ConstantFormula>()).ReturnsLazily(x=>new ConstantFormula());
      }

      protected override void Because()
      {
         _interval = sut.Create(0, 10, 5);
      }

      [Observation]
      public void should_return_an_interval_having_the_expected_min_max_and_resolution_value()
      {
         _interval.StartTime.Value.ShouldBeEqualTo(0);
         _interval.EndTime.Value.ShouldBeEqualTo(10);
         _interval.Resolution.Value.ShouldBeEqualTo(5);
      }

    
      [Observation]
      public void the_parameter_should_be_in_the_default_settings_group()
      {
         _interval.StartTime.GroupName.ShouldBeEqualTo(Constants.Groups.SIMULATION_SETTINGS);
         _interval.EndTime.GroupName.ShouldBeEqualTo(Constants.Groups.SIMULATION_SETTINGS);
         _interval.Resolution.GroupName.ShouldBeEqualTo(Constants.Groups.SIMULATION_SETTINGS);
      }

      [Observation]
      public void the_min_value_for_resolution_should_not_be_allowed()
      {
         _interval.Resolution.Info.MinIsAllowed.ShouldBeFalse();
      }

      [Observation]
      public void the_created_parameters_should_be_visible()
      {
         _interval.StartTime.Visible.ShouldBeTrue();
         _interval.EndTime.Visible.ShouldBeTrue();
         _interval.Resolution.Visible.ShouldBeTrue();
      }

      [Observation]
      public void the_created_parameters_should_be_editable()
      {
         _interval.StartTime.Editable.ShouldBeTrue();
         _interval.EndTime.Editable.ShouldBeTrue();
         _interval.Resolution.Editable.ShouldBeTrue();
      }


      [Observation]
      public void the_created_parameters_should_be_set_to_default()
      {
         _interval.StartTime.IsDefault.ShouldBeTrue();
         _interval.EndTime.IsDefault.ShouldBeTrue();
         _interval.Resolution.IsDefault.ShouldBeTrue();
      }

      [Observation]
      public void the_created_parameters_should_not_be_variable()
      {
         _interval.StartTime.CanBeVaried.ShouldBeFalse();
         _interval.EndTime.CanBeVaried.ShouldBeFalse();
         _interval.Resolution.CanBeVaried.ShouldBeFalse();
      }
   }

   public class When_creating_an_interval_for_a_given_output_schema : concern_for_OutputIntervalFactory
   {
      private OutputSchema _schema;
      private OutputInterval _interval;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _objectBaseFactory.Create<OutputInterval>()).Returns(new OutputInterval());
         A.CallTo(() => _objectBaseFactory.Create<ConstantFormula>()).ReturnsLazily(x => new ConstantFormula());
         _schema = new OutputSchema();
         _schema.AddInterval(new OutputInterval());
      }
      protected override void Because()
      {
         _interval = sut.CreateFor(_schema, 10, 20, 1);
      }

      [Observation]
      public void should_have_generated_a_unique_name_based_on_the_schema()
      {
         _interval.Name.ShouldNotBeEqualTo(Constants.OUTPUT_INTERVAL);
      }

      [Observation]
      public void should_not_have_added_the_interval_to_the_output()
      {
         _schema.Intervals.ShouldNotContain(_interval);
      }
   }
}