using System.Collections.Generic;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Views.Charts;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_AxisSettingsPresenter : ContextSpecification<IAxisSettingsPresenter>
   {
      protected IAxisSettingsView _view;
      private IDimensionFactory _dimensionFactory;
      protected List<Axis> _axes;
      protected Axis _axisX;
      protected Axis _axisY;

      protected override void Context()
      {
         _view = A.Fake<IAxisSettingsView>();
         _dimensionFactory = A.Fake<IDimensionFactory>();
         sut = new AxisSettingsPresenter(_view, _dimensionFactory);
         _axisX = new Axis(AxisTypes.X);
         _axisY = new Axis(AxisTypes.Y);
         _axes = new List<Axis> {_axisX, _axisY};

         sut.Edit(_axes);
      }
   }

   public class When_the_axis_settings_presenter_is_notified_that_the_unit_have_changed_for_the_edited_axis : concern_for_AxisSettingsPresenter
   {
      protected override void Context()
      {
         base.Context();
         _axisX.Max = 10;
         _axisX.Min = 5;
      }

      protected override void Because()
      {
         sut.UnitChanged(_axisX);
      }

      [Observation]
      public void should_reset_the_axis_range()
      {
         _axisX.Min.ShouldBeNull();
         _axisX.Max.ShouldBeNull();
      }
   }

   public class When_the_axis_settings_presenter_is_refreshing_its_content : concern_for_AxisSettingsPresenter
   {
      protected override void Because()
      {
         sut.Refresh();
      }

      [Observation]
      public void should_bind_to_the_view()
      {
         A.CallTo(() => _view.BindTo(_axes)).MustHaveHappenedTwiceExactly();
      }
   }

   public class When_the_axis_settings_presenter_is_notified_that_the_axis_property_of_a_given_axis_have_changed : concern_for_AxisSettingsPresenter
   {
      private Axis _axisChanged;

      protected override void Context()
      {
         base.Context();
         sut.AxisPropertyChanged += (o, e) => _axisChanged = e.Axis;
      }

      protected override void Because()
      {
         sut.NotifyAxisPropertyChanged(_axisX);
      }

      [Observation]
      public void should_notify_its_own_axis_property_changed()
      {
         _axisChanged.ShouldBeEqualTo(_axisX);
      }
   }

   public class When_the_axis_settings_presenter_is_notified_that_an_axis_was_removed : concern_for_AxisSettingsPresenter
   {
      private Axis _axisRemoved;

      protected override void Context()
      {
         base.Context();
         sut.AxisRemoved += (o, e) => _axisRemoved = e.Axis;
      }

      protected override void Because()
      {
         sut.RemoveAxis(_axisX);
      }

      [Observation]
      public void should_send_the_axis_removed_event()
      {
         _axisRemoved.ShouldBeEqualTo(_axisX);
      }
   }

   public class When_the_axis_settings_presenter_is_notified_that_an_axis_was_added : concern_for_AxisSettingsPresenter
   {
      private bool _axisAddedNotified;

      protected override void Context()
      {
         base.Context();
         sut.AxisAdded += (o,e) => _axisAddedNotified = true;
      }

      protected override void Because()
      {
         sut.AddYAxis();
      }

      [Observation]
      public void should_notify_the_axis_added_event()
      {
         _axisAddedNotified.ShouldBeTrue();
      }
   }

   public class When_the_axis_settings_presenter_is_told_to_clear_its_content : concern_for_AxisSettingsPresenter
   {
      protected override void Because()
      {
         sut.Clear();
      }

      [Observation]
      public void should_delete_the_binding()
      {
         A.CallTo(() => _view.DeleteBinding()).MustHaveHappened();
      }
   }

   public class When_the_axis_settings_presenter_is_retrieving_all_the_units_defined_for_a_given_dimension : concern_for_AxisSettingsPresenter
   {
      private IDimension _dimension;

      protected override void Context()
      {
         base.Context();
         _dimension = A.Fake<IDimension>();
         A.CallTo(() => _dimension.GetUnitNames()).Returns(new[] {"A", "B"});
      }

      [Observation]
      public void should_return_an_empty_list_for_an_undefined_dimension()
      {
         sut.AllUnitNamesFor(null).ShouldBeEmpty();
      }

      [Observation]
      public void should_return_the_list_of_units_for_a_defined_dimension()
      {
         sut.AllUnitNamesFor(_dimension).ShouldOnlyContainInOrder("A", "B");
      }
   }
}