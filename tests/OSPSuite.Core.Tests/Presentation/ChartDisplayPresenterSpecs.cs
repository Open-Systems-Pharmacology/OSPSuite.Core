using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.Mappers;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;
using OSPSuite.Helpers;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Services.Charts;
using OSPSuite.Presentation.Views.Charts;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_ChartDisplayPresenter : ContextSpecification<ChartDisplayPresenter>
   {
      protected IChartDisplayView _chartDisplayView;
      protected ICurveBinderFactory _curveBinderFactory;
      protected IExceptionManager _exceptionManager;

      protected CurveChart _curveChart;
      protected Curve _curve;
      protected IDimensionFactory _dimensionFactory;
      private IAxisBinderFactory _axisBinderFactory;
      private ICurveToDataModeMapper _dataModeMapper;
      private IAxisBinder _xAxisBinder;
      private IAxisBinder _yAxisBinder;
      private IViewItemContextMenuFactory _contextMenuFactory;
      protected ICurveChartExportTask _chartExportTask;
      protected IApplicationSettings _applicationSettings;

      protected override void Context()
      {
         _chartDisplayView = A.Fake<IChartDisplayView>();
         _curveBinderFactory = A.Fake<ICurveBinderFactory>();
         _exceptionManager = A.Fake<IExceptionManager>();
         _dimensionFactory = A.Fake<IDimensionFactory>();
         _axisBinderFactory = A.Fake<IAxisBinderFactory>();
         _dataModeMapper = A.Fake<ICurveToDataModeMapper>();
         _contextMenuFactory = A.Fake<IViewItemContextMenuFactory>();
         _chartExportTask = A.Fake<ICurveChartExportTask>();
         _applicationSettings = A.Fake<IApplicationSettings>();

         sut = new ChartDisplayPresenter(_chartDisplayView, _curveBinderFactory, _contextMenuFactory, _axisBinderFactory, _dataModeMapper, _chartExportTask, _applicationSettings);
         var dataRepository = DomainHelperForSpecs.SimulationDataRepositoryFor("Sim");

         A.CallTo(() => _dimensionFactory.MergedDimensionFor(A<DataColumn>._)).ReturnsLazily(x => x.GetArgument<DataColumn>(0).Dimension);

         _curve = new Curve();
         _curve.SetxData(dataRepository.BaseGrid, _dimensionFactory);
         _curve.SetyData(dataRepository.AllButBaseGrid().First(), _dimensionFactory);

         A.CallTo(_curveBinderFactory).WithReturnType<ICurveBinder>().ReturnsLazily(x =>
         {
            var curve = x.GetArgument<Curve>(0);
            var curveBinder = A.Fake<ICurveBinder>();
            A.CallTo(() => curveBinder.SeriesIds).Returns(SeriesIdsFor(curve));
            A.CallTo(() => curveBinder.LLOQ).Returns(LLOQFor(curve));
            A.CallTo(() => curveBinder.ContainsSeries(curve.Id)).Returns(true);
            A.CallTo(() => curveBinder.Id).Returns(curve.Id);
            A.CallTo(() => curveBinder.Curve).Returns(curve);
            return curveBinder;
         });

         _curveChart = new CurveChart();
         _curveChart.AddAxis(new Axis(AxisTypes.X) {Dimension = _curve.xDimension});
         _curveChart.AddAxis(new Axis(AxisTypes.Y) {Dimension = _curve.yDimension});

         _xAxisBinder = createAxisBinderFor(_curveChart.AxisBy(AxisTypes.X));
         _yAxisBinder = createAxisBinderFor(_curveChart.AxisBy(AxisTypes.Y));

         A.CallTo(() => _axisBinderFactory.Create(_curveChart.AxisBy(AxisTypes.X), _chartDisplayView.ChartControl, _curveChart)).Returns(_xAxisBinder);
         A.CallTo(() => _axisBinderFactory.Create(_curveChart.AxisBy(AxisTypes.Y), _chartDisplayView.ChartControl, _curveChart)).Returns(_yAxisBinder);

         SetupChart();
         sut.Edit(_curveChart);
      }

      protected virtual double? LLOQFor(Curve curve)
      {
         return null;
      }

      protected virtual string[] SeriesIdsFor(Curve curve)
      {
         return new[] {curve.Id};
      }

      protected virtual void SetupChart()
      {
      }

      private IAxisBinder createAxisBinderFor(Axis axis)
      {
         var axisBinder = A.Fake<IAxisBinder>();
         A.CallTo(() => axisBinder.AxisType).Returns(axis.AxisType);
         A.CallTo(() => axisBinder.Axis).Returns(axis);
         return axisBinder;
      }
   }

   public class When_hiding_a_legend_entry_for_a_curve_whose_series_can_be_found_by_the_view : concern_for_ChartDisplayPresenter
   {
      protected override void SetupChart()
      {
         base.SetupChart();
         _curve.VisibleInLegend = true;
         _curveChart.AddCurve(_curve);
      }

      protected override void Because()
      {
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
      private Axis _resultX;
      private Axis _resultY;

      protected override void SetupChart()
      {
         base.SetupChart();
         _curveChart.AddCurve(_curve);
      }

      protected override void Because()
      {
         _resultX = sut.AxisBy(AxisTypes.X);
         _resultY = sut.AxisBy(AxisTypes.Y);
      }

      [Observation]
      public void axis_returned_should_be_correct()
      {
         _resultX.ShouldBeEqualTo(_curveChart.AxisBy(AxisTypes.X));
         _resultY.ShouldBeEqualTo(_curveChart.AxisBy(AxisTypes.Y));
      }
   }

   public class When_resolving_an_axis_from_type_and_the_axis_is_not_found_in_the_presenter : concern_for_ChartDisplayPresenter
   {
      private Axis _result;

      protected override void Because()
      {
         _result = sut.AxisBy(AxisTypes.Y2);
      }

      [Observation]
      public void returned_value_should_be_null()
      {
         _result.ShouldBeNull();
      }
   }

   public class When_resolving_a_curve_from_Id_and_the_curve_is_not_found_in_the_presenter : concern_for_ChartDisplayPresenter
   {
      private Curve _result;

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
      protected override void SetupChart()
      {
         base.SetupChart();
         _curveChart.AddCurve(_curve);
      }

      [Observation]
      public void should_be_able_to_retrieve_the_curve_from_the_related_curve()
      {
         sut.CurveFromSeriesId(_curve.Id).ShouldBeEqualTo(_curve);
      }

      protected override string[] SeriesIdsFor(Curve curve)
      {
         return new[] {_curve.Id, "relatedCurve"};
      }
   }

   public class When_removing_the_curve_from_the_chart : concern_for_ChartDisplayPresenter
   {
      protected override void SetupChart()
      {
         base.SetupChart();
         _curve.Description = "description";
         _curveChart.AddCurve(_curve);
      }

      protected override void Because()
      {
         _curveChart.RemoveCurve(_curve.Id);
         sut.Edit(_curveChart);
      }

      [Observation]
      public void the_curve_is_not_available_from_the_presenter()
      {
         sut.CurveFromSeriesId(_curve.Id).ShouldBeNull();
      }
   }

   public class When_retrieving_the_curve_description_from_the_curve_id : concern_for_ChartDisplayPresenter
   {
      protected override void SetupChart()
      {
         base.SetupChart();
         _curve.Description = "description";
         _curveChart.AddCurve(_curve);
      }

      [Observation]
      public void the_curve_description_should_be_retrieved()
      {
         sut.CurveDescriptionFromSeriesId(_curve.Id).ShouldBeEqualTo(_curve.Description);
      }
   }

   public class When_resolving_a_curve_from_Id_and_the_curve_is_found_in_the_presenter : concern_for_ChartDisplayPresenter
   {
      protected override void SetupChart()
      {
         base.SetupChart();
         _curveChart.AddCurve(_curve);
      }

      [Observation]
      public void should_return_the_curve()
      {
         sut.CurveFromSeriesId(_curve.Id).ShouldBeEqualTo(_curve);
      }
   }

   public class When_resolving_a_legend_index_from_Id_and_the_curve_is_found_in_the_presenter : concern_for_ChartDisplayPresenter
   {
      protected override void SetupChart()
      {
         base.SetupChart();
         _curve.LegendIndex = 5;
         _curveChart.AddCurve(_curve);
      }

      [Observation]
      public void should_return_the_curve_index()
      {
         sut.LegendIndexFromSeriesId(_curve.Id).ShouldBeEqualTo(_curve.LegendIndex.Value);
      }
   }

   public class When_checking_if_a_point_is_below_lloq_and_there_are_no_lloq_series_being_displayed : concern_for_ChartDisplayPresenter
   {
      protected override void SetupChart()
      {
         base.SetupChart();
         _curve.ShowLLOQ = false;
         _curve.yData.DataInfo.LLOQ = 0.5f;
         _curveChart.AddCurve(_curve);
      }

      [Observation]
      public void should_return_false()
      {
         sut.IsPointBelowLLOQ(new[] {0.4, 0.3}, _curve.Id).ShouldBeFalse();
      }
   }

   public class When_checking_if_a_point_is_below_lloq_and_there_are_lloq_series_being_displayed : concern_for_ChartDisplayPresenter
   {
      protected override void SetupChart()
      {
         base.SetupChart();
         _curve.ShowLLOQ = true;
         _curveChart.AddCurve(_curve);
      }

      protected override double? LLOQFor(Curve curve)
      {
         return 0.5d;
      }

      [Observation]
      public void should_return_false()
      {
         sut.IsPointBelowLLOQ(new[] {0.4, 0.3}, _curve.Id).ShouldBeTrue();
      }
   }

   public class When_resolving_a_legend_index_from_Id_and_the_curve_is_found_in_the_presenter_but_has_no_legend_index : concern_for_ChartDisplayPresenter
   {
      protected override void SetupChart()
      {
         base.SetupChart();
         _curveChart.AddCurve(_curve);
         _curve.LegendIndex = null;
      }

      [Observation]
      public void should_return_zero()
      {
         sut.LegendIndexFromSeriesId(_curve.Id).ShouldBeEqualTo(0);
      }
   }

   public class When_resolving_a_legend_index_from_Id_and_the_curve_is_not_found_in_the_presenter : concern_for_ChartDisplayPresenter
   {
      [Observation]
      public void should_return_zero()
      {
         sut.LegendIndexFromSeriesId(_curve.Id).ShouldBeEqualTo(0);
      }
   }

   public class When_copying_chart_to_clipboard : concern_for_ChartDisplayPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _applicationSettings.WatermarkTextToUse).Returns("Hello");
      }

      protected override void Because()
      {
         sut.CopyToClipboard();
      }

      [Observation]
      public void the_view_method_should_be_invoked_to_copy_the_chart_to_clipboard_with_the_watermark_defined_in_application_settings()
      {
         A.CallTo(() => _chartDisplayView.CopyToClipboard(_applicationSettings.WatermarkTextToUse)).MustHaveHappened();
      }
   }

   public class When_the_chart_display_presenter_is_exporting_the_displayed_chart_to_excel : concern_for_ChartDisplayPresenter
   {
      protected override void Because()
      {
         sut.ExportToExcel();
      }

      [Observation]
      public void should_use_the_chart_export_task_to_expor_the_char_to_excel()
      {
         A.CallTo(() => _chartExportTask.ExportToExcel(_curveChart)).MustHaveHappened();
      }
   }
}