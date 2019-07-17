using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Views.Charts;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_SingleAxisSettingsPresenter : ContextSpecification<ISingleAxisSettingsPresenter>
   {
      protected ISingleAxisSettingsView _view;
      private IDimensionFactory _dimensionFactory;
      protected IChartUpdater _chartUpdater;

      protected override void Context()
      {
         _view = A.Fake<ISingleAxisSettingsView>();
         _dimensionFactory = A.Fake<IDimensionFactory>();
         _chartUpdater = A.Fake<IChartUpdater>();
         sut = new SingleAxisSettingsPresenter(_view, _dimensionFactory, _chartUpdater);
      }
   }

   public class When_editing_the_axis_settings_of_a_given_axis_and_the_user_accepts_the_changes : concern_for_SingleAxisSettingsPresenter
   {
      private IChart _chart;
      private Axis _axis;
      private Axis _editedAxis;

      protected override void Context()
      {
         base.Context();
         _chart = A.Fake<IChart>();
         _axis = new Axis(AxisTypes.Y)
         {
            Min = 10,
            Max = 20
         };

         A.CallTo(() => _view.BindTo(A<Axis>._)).Invokes(x =>
         {
            _editedAxis = x.GetArgument<Axis>(0);
            _editedAxis.Max = 50;
         });

         A.CallTo(() => _view.Canceled).Returns(false);
      }

      protected override void Because()
      {
         sut.Edit(_chart, _axis);
      }

      [Observation]
      public void shoudld_edit_the_settings_of_a_clone_of_the_axis()
      {
         _editedAxis.ShouldNotBeNull();
      }

      [Observation]
      public void should_udate_the_axis_with_the_edited_properties()
      {
         _axis.Max.ShouldBeEqualTo(_editedAxis.Max);
      }

      [Observation]
      public void should_update_the_chart()
      {
         A.CallTo(() => _chartUpdater.Update(_chart)).MustHaveHappened();
      }
   }

   public class When_editing_the_axis_settings_of_a_given_axis_and_the_user_cancels_the_changes : concern_for_SingleAxisSettingsPresenter
   {
      private IChart _chart;
      private Axis _axis;
      private Axis _editedAxis;

      protected override void Context()
      {
         base.Context();
         _chart = A.Fake<IChart>();
         _axis = new Axis(AxisTypes.Y)
         {
            Min = 10,
            Max = 20
         };

         A.CallTo(() => _view.BindTo(A<Axis>._)).Invokes(x =>
         {
            _editedAxis = x.GetArgument<Axis>(0);
            _editedAxis.Max = 50;
         });

         A.CallTo(() => _view.Canceled).Returns(true);
      }

      protected override void Because()
      {
         sut.Edit(_chart, _axis);
      }

      [Observation]
      public void shoudld_edit_the_settings_of_a_clone_of_the_axis()
      {
         _editedAxis.ShouldNotBeNull();
      }

      [Observation]
      public void should_not_udate_the_axis_with_the_edited_properties()
      {
         _axis.Max.ShouldBeEqualTo(20);
      }

      [Observation]
      public void should_not_update_the_chart()
      {
         A.CallTo(() => _chartUpdater.Update(_chart)).MustNotHaveHappened();
      }
   }
}