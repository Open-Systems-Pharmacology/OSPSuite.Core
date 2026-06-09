using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Views.Charts;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_ChartSettingsPresenter : ContextSpecification<IChartSettingsPresenter>
   {
      protected IChartSettingsView _view;

      protected override void Context()
      {
         _view = A.Fake<IChartSettingsView>();
         sut = new ChartSettingsPresenter(_view);
      }
   }

   public class When_the_chart_settings_presenter_is_editing_the_settings_of_a_given_chart : concern_for_ChartSettingsPresenter
   {
      private IChart _chart;

      protected override void Context()
      {
         base.Context();
         _chart = A.Fake<IChart>();
      }

      protected override void Because()
      {
         sut.Edit(_chart);
      }

      [Observation]
      public void should_bind_those_settings_to_the_view()
      {
         A.CallTo(() => _view.BindTo(_chart)).MustHaveHappened();
      }
   }

   public class When_the_chart_settings_presenter_is_editing_the_settings_of_a_given_chart_template : concern_for_ChartSettingsPresenter
   {
      private CurveChartTemplate _chartTemplate;

      protected override void Context()
      {
         base.Context();
         _chartTemplate = new CurveChartTemplate();
      }

      protected override void Because()
      {
         sut.Edit(_chartTemplate);
      }

      [Observation]
      public void should_bind_those_settings_to_the_view()
      {
         A.CallTo(() => _view.BindTo(_chartTemplate)).MustHaveHappened();
      }
   }

   public class When_the_chart_settings_presenter_is_notified_that_chart_settings_property_were_changed : concern_for_ChartSettingsPresenter
   {
      private bool _settingsChanged;

      protected override void Context()
      {
         base.Context();
         sut.ChartSettingsChanged += (o,e) => _settingsChanged = true;
      }

      protected override void Because()
      {
         sut.NotifyChartSettingsChanged();
      }

      [Observation]
      public void should_notify_its_own_chart_setttings_changed_event()
      {
         _settingsChanged.ShouldBeTrue();
      }
   }

   public class When_the_chart_settings_presenter_is_told_to_clear_its_content : concern_for_ChartSettingsPresenter
   {
      protected override void Because()
      {
         sut.Clear();
      }

      [Observation]
      public void should_delete_any_binding()
      {
         A.CallTo(() => _view.DeleteBinding()).MustHaveHappened();
      }
   }

   public class When_the_chart_settings_presenter_is_told_that_the_name_of_the_object_should_not_be_visible : concern_for_ChartSettingsPresenter
   {
      protected override void Because()
      {
         sut.NameVisible = false;
      }

      [Observation]
      public void should_hide_the_name_in_the_view()
      {
         _view.NameVisible.ShouldBeFalse();
      }
   }

   public class When_the_chart_settings_presenter_is_asked_for_the_display_string_of_a_legend_position : concern_for_ChartSettingsPresenter
   {
      [Observation]
      public void should_describe_a_legacy_outside_position_with_its_vertical_horizontal_and_placement()
      {
         sut.LegendPositionDisplayFor(LegendPositions.Right).ShouldBeEqualTo("Top Right (Outside)");
      }

      [Observation]
      public void should_describe_a_legacy_inside_position_with_its_vertical_horizontal_and_placement()
      {
         sut.LegendPositionDisplayFor(LegendPositions.BottomInside).ShouldBeEqualTo("Bottom Right (Inside)");
      }

      [Observation]
      public void should_describe_a_new_left_outside_position()
      {
         sut.LegendPositionDisplayFor(LegendPositions.TopLeftOutside).ShouldBeEqualTo("Top Left (Outside)");
      }

      [Observation]
      public void should_describe_a_new_left_inside_position()
      {
         sut.LegendPositionDisplayFor(LegendPositions.BottomLeftInside).ShouldBeEqualTo("Bottom Left (Inside)");
      }

      [Observation]
      public void should_return_the_enum_name_when_the_legend_is_hidden()
      {
         sut.LegendPositionDisplayFor(LegendPositions.None).ShouldBeEqualTo("None");
      }
   }
}