using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core
{
   public abstract class concern_for_OutputIntervalFactory : ContextSpecification<IOutputIntervalFactory>
   {
      protected IObjectBaseFactory _objectBaseFactory;
      private IDimensionFactory _dimensionFactory;

      protected override void Context()
      {
         _objectBaseFactory = A.Fake<IObjectBaseFactory>();
         _dimensionFactory = A.Fake<IDimensionFactory>();
         A.CallTo(() => _objectBaseFactory.Create<IParameter>()).ReturnsLazily(x=>new Parameter());
         A.CallTo(() => _dimensionFactory.Dimension(A<string>._)).Returns(new Dimension());
         sut = new OutputIntervalFactory(_objectBaseFactory, _dimensionFactory);
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
}