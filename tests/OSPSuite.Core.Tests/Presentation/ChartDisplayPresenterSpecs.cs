using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Exceptions;
using FakeItEasy;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Services.Charts;
using OSPSuite.Presentation.Views.Charts;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_ChartDisplayPresenter : ContextSpecification<ChartDisplayPresenter>
   {
      protected IChartDisplayView _chartDisplayView;
      protected IDataRepositoryTask _dataRepositoryTask;
      protected IDialogCreator _dialogCreator;
      protected ICurveAdapterFactory _curveAdapterFactory;
      protected ICurveChartContextMenuFactory _curveChartContextMenuFactory;
      protected ICurveContextMenuFactory _curveContextMenuFactory;
      protected IAxisContextMenuFactory _axisContextMenuFactory;
      protected IExceptionManager _exceptionManager;

      protected CurveChart _curveChart;
      protected ICurveAdapter _curveAdapter;
      protected ICurve _curve;

      protected override void Context()
      {
         _chartDisplayView = A.Fake<IChartDisplayView>();
         _dataRepositoryTask = A.Fake<IDataRepositoryTask>();
         _dialogCreator = A.Fake<IDialogCreator>();
         _curveAdapterFactory = A.Fake<ICurveAdapterFactory>();
         _curveChartContextMenuFactory = A.Fake<ICurveChartContextMenuFactory>();
         _curveContextMenuFactory = A.Fake<ICurveContextMenuFactory>();
         _axisContextMenuFactory = A.Fake<IAxisContextMenuFactory>();
         _exceptionManager = A.Fake<IExceptionManager>();
         _curveAdapter = A.Fake<ICurveAdapter>();
         _curve = A.Fake<ICurve>();
         A.CallTo(() => _curve.Id).Returns("curveId");
         A.CallTo(() => _curveAdapter.SeriesIds).Returns(new [] { _curve.Id });

         A.CallTo(() => _curveAdapterFactory.CreateFor(A<ICurve>._, A<IAxis>._, A<IAxis>._)).Returns(_curveAdapter);
         A.CallTo(() => _curveAdapter.ContainsSeries(_curve.Id)).Returns(true);
         A.CallTo(() => _curveAdapter.Id).Returns(_curve.Id);
         A.CallTo(() => _curveAdapter.Curve).Returns(_curve);

         sut = new ChartDisplayPresenter(
            _chartDisplayView,
            _dataRepositoryTask,
            _dialogCreator,
            _curveAdapterFactory,
            _curveChartContextMenuFactory,
            _curveContextMenuFactory,
            _axisContextMenuFactory,
            _exceptionManager
            );

         _curveChart = new CurveChart();
         sut.DataSource = _curveChart;
      }
   }

   public class When_hiding_a_legend_entry_for_a_curve_whose_series_can_be_found_by_the_view : concern_for_ChartDisplayPresenter
   {
      protected override void Context()
      {
         base.Context();
         _curve.VisibleInLegend = true;
         _curveChart.AddCurve(_curve);
      }

      protected override void Because()
      {
         base.Because();
         _curve.VisibleInLegend = true;
         sut.ShowCurveInLegend(_curve, false);
      }

      [Observation]
      public void the_curve_must_be_marked_as_hidden_by_the_presenter()
      {
         _curve.VisibleInLegend.ShouldBeFalse();
      }
   }

   public class When_resolving_an_axis_from_type_and_the_axis_is_found_by_the_presenter : concern_for_ChartDisplayPresenter
   {
      private IAxis _resultX;
      private IAxis _resultY;

      protected override void Context()
      {
         base.Context();
         _curveChart.AddCurve(_curve);
      }

      protected override void Because()
      {
         _resultX = sut.GetAxisFrom(AxisTypes.X);
         _resultY = sut.GetAxisFrom(AxisTypes.Y);
      }

      [Observation]
      public void axis_returned_should_be_correct()
      {
         _resultX.ShouldBeEqualTo(_curveChart.Axes[AxisTypes.X]);
         _resultY.ShouldBeEqualTo(_curveChart.Axes[AxisTypes.Y]);
      }
   }

   public class When_resolving_an_axis_from_type_and_the_axis_is_not_found_in_the_presenter : concern_for_ChartDisplayPresenter
   {
      private IAxis _result;

      protected override void Because()
      {
         _result = sut.GetAxisFrom(AxisTypes.Y2);
      }

      [Observation]
      public void returned_value_should_be_null()
      {
         _result.ShouldBeNull();
      }
   }

   public class When_resolving_a_curve_from_Id_and_the_curve_is_not_found_in_the_presenter : concern_for_ChartDisplayPresenter
   {
      private ICurve _result;

      protected override void Because()
      {
         _result = sut.CurveFromSeriesId("id");
      }

      [Observation]
      public void the_curve_returned_should_be_null()
      {
         _result.ShouldBeNull();
      }
   }

   public class When_adding_a_curve_to_the_chart_with_a_related_column : concern_for_ChartDisplayPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _curveAdapter.SeriesIds).Returns(new[] { _curve.Id, "relatedCurve" });      
      }

      protected override void Because()
      {
         _curveChart.AddCurve(_curve);
      }

      [Observation]
      public void should_be_able_to_retrieve_the_curve_from_the_related_curve()
      {
         sut.CurveFromSeriesId("curveId").ShouldBeEqualTo(_curve);
      }
   }

   public class When_removing_the_curve_from_the_chart : concern_for_ChartDisplayPresenter
   {
      protected override void Context()
      {
         base.Context();
         _curve.Description = "description";
         _curveChart.AddCurve(_curve);
      }

      protected override void Because()
      {
         _curveChart.RemoveCurve("curveId");
      }

      [Observation]
      public void the_curve_is_not_available_from_the_presenter()
      {
         sut.CurveFromSeriesId("curveId").ShouldBeNull();
      }
   }

   public class When_retrieving_the_curve_description_from_the_curve_id : concern_for_ChartDisplayPresenter
   {
      protected override void Context()
      {
         base.Context();
         _curve.Description = "description";
         _curveChart.AddCurve(_curve);
      }

      [Observation]
      public void the_curve_description_should_be_retrieved()
      {
         sut.CurveDescriptionFromSeriesId("curveId").ShouldBeEqualTo(_curve.Description);
      }
   }

   public class When_resolving_a_curve_from_Id_and_the_curve_is_found_in_the_presenter : concern_for_ChartDisplayPresenter
   {
      protected override void Context()
      {
         base.Context();
         _curveChart.AddCurve(_curve);
      }

      [Observation]
      public void should_return_the_curve()
      {
         sut.CurveFromSeriesId("curveId").ShouldBeEqualTo(_curve);
      }
   }

   public class When_resolving_a_legend_index_from_Id_and_the_curve_is_found_in_the_presenter : concern_for_ChartDisplayPresenter
   {
      protected override void Context()
      {
         base.Context();
         _curve.LegendIndex = 5;
         _curveChart.AddCurve(_curve);
      }

      [Observation]
      public void should_return_the_curve_index()
      {
         sut.LegendIndexFromSeriesId("curveId").ShouldBeEqualTo(_curve.LegendIndex.Value);
      }
   }

   public class When_resolving_a_legend_index_from_Id_and_the_curve_is_found_in_the_presenter_but_has_no_legend_index : concern_for_ChartDisplayPresenter
   {
      protected override void Context()
      {
         base.Context();
         _curveChart.AddCurve(_curve);
         _curve.LegendIndex = null;
      }

      [Observation]
      public void should_return_zero()
      {
         sut.LegendIndexFromSeriesId("curveId").ShouldBeEqualTo(0);
      }
   }

   public class When_resolving_a_legend_index_from_Id_and_the_curve_is_not_found_in_the_presenter : concern_for_ChartDisplayPresenter
   {
      [Observation]
      public void should_return_zero()
      {
         sut.LegendIndexFromSeriesId("curveId").ShouldBeEqualTo(0);
      }
   }

   public class When_copying_chart_to_clipboard : concern_for_ChartDisplayPresenter
   {
      protected override void Because()
      {
         sut.CopyToClipboard();
      }

      [Observation]
      public void the_view_method_should_be_invoked_to_copy_the_chart_to_clipboard()
      {
         A.CallTo(() => _chartDisplayView.CopyToClipboardWithExportSettings()).MustHaveHappened();
      }
   }

   public class When_calling_reset_zoom_for_chart : concern_for_ChartDisplayPresenter
   {
      protected override void Because()
      {
         sut.ResetZoom();
      }

      [Observation]
      public void the_view_method_should_be_invoked_to_reset_the_zoom()
      {
         A.CallTo(() => _chartDisplayView.ResetChartZoom()).MustHaveHappened();
      }
   }
}