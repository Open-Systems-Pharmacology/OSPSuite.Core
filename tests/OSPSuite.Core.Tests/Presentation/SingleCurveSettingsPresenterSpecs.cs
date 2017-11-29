using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Views.Charts;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_SingleCurveSettingsPresenter : ContextSpecification<ISingleCurveSettingsPresenter>
   {
      protected ISingleCurveSettingsView _view;
      protected IChartUpdater _chartUpdater;

      protected override void Context()
      {
         _view= A.Fake<ISingleCurveSettingsView>();
         _chartUpdater= A.Fake<IChartUpdater>();
         sut = new SingleCurveSettingsPresenter(_view,_chartUpdater);
      }
   }

   public class When_editing_the_curve_settings_of_a_single_curve_and_the_user_accepts_the_changes : concern_for_SingleCurveSettingsPresenter
   {
      private IChart _chart;
      private Curve _curve;
      private Curve _editedCurve;

      protected override void Context()
      {
         base.Context();
         _chart= A.Fake<IChart>();
         _curve = new Curve {Name = "Original Name"};
         A.CallTo(() => _view.BindTo(A<Curve>._)).Invokes(x =>
         {
            _editedCurve = x.GetArgument<Curve>(0);
            _editedCurve.Name = "Edited Name";
         });

         A.CallTo(() => _view.Canceled).Returns(false);
      }

      protected override void Because()
      {
         sut.Edit(_chart,_curve);
      }

      [Observation]
      public void should_edit_a_clone_of_the_given_curve()
      {
         _editedCurve.ShouldNotBeNull();
      }

      [Observation]
      public void should_update_the_edited_curve_with_the_edited_settings()
      {
         _curve.Name.ShouldBeEqualTo(_editedCurve.Name);
      }

      [Observation]
      public void should_update_the_chart()
      {
         A.CallTo(() => _chartUpdater.Update(_chart)).MustHaveHappened();
      }
   }

   public class When_editing_the_curve_settings_of_a_single_curve_and_the_user_cancels_the_changes : concern_for_SingleCurveSettingsPresenter
   {
      private IChart _chart;
      private Curve _curve;
      private Curve _editedCurve;

      protected override void Context()
      {
         base.Context();
         _chart = A.Fake<IChart>();
         _curve = new Curve { Name = "Original Name" };
         A.CallTo(() => _view.BindTo(A<Curve>._)).Invokes(x =>
         {
            _editedCurve = x.GetArgument<Curve>(0);
            _editedCurve.Name = "Edited Name";
         });

         A.CallTo(() => _view.Canceled).Returns(true);
      }

      protected override void Because()
      {
         sut.Edit(_chart, _curve);
      }

      [Observation]
      public void should_edit_a_clone_of_the_given_curve()
      {
         _editedCurve.ShouldNotBeNull();
      }

      [Observation]
      public void should_not_update_the_edited_curve_with_the_edited_settings()
      {
         _curve.Name.ShouldBeEqualTo("Original Name");
      }

      [Observation]
      public void should_not_update_the_chart()
      {
         A.CallTo(() => _chartUpdater.Update(_chart)).MustNotHaveHappened();
      }
   }

}