using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.Mappers;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
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
      protected IDataRepositoryTask _dataRepositoryTask;
      protected IDialogCreator _dialogCreator;
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

      protected override void Context()
      {
         _chartDisplayView = A.Fake<IChartDisplayView>();
         _dataRepositoryTask = A.Fake<IDataRepositoryTask>();
         _dialogCreator = A.Fake<IDialogCreator>();
         _curveBinderFactory = A.Fake<ICurveBinderFactory>();
         _exceptionManager = A.Fake<IExceptionManager>();
         _dimensionFactory = A.Fake<IDimensionFactory>();
         _axisBinderFactory = A.Fake<IAxisBinderFactory>();
         _dataModeMapper = A.Fake<ICurveToDataModeMapper>();
         _contextMenuFactory = A.Fake<IViewItemContextMenuFactory>();

         sut = new ChartDisplayPresenter(_chartDisplayView, _dataRepositoryTask, _dialogCreator, _curveBinderFactory, _contextMenuFactory, _axisBinderFactory, _dataModeMapper);

         var dataRepository = DomainHelperForSpecs.SimulationDataRepositoryFor("Sim");

         A.CallTo(() => _dimensionFactory.GetMergedDimensionFor(A<DataColumn>._)).ReturnsLazily(x=>x.GetArgument<DataColumn>(0).Dimension);

         _curve = new Curve();
         _curve.SetxData(dataRepository.BaseGrid, _dimensionFactory);
         _curve.SetyData(dataRepository.AllButBaseGrid().First(), _dimensionFactory);

         A.CallTo(_curveBinderFactory).WithReturnType<ICurveBinder>().ReturnsLazily(x =>
         {
            var curve = x.GetArgument<Curve>(0);
            var curveBinder = A.Fake<ICurveBinder>();
            A.CallTo(() => curveBinder.SeriesIds).Returns(SeriesIdsFor(curve));
            A.CallTo(() => curveBinder.ContainsSeries(curve.Id)).Returns(true);
            A.CallTo(() => curveBinder.Id).Returns(curve.Id);
            A.CallTo(() => curveBinder.Curve).Returns(curve);
            return curveBinder;
         });

         _curveChart = new CurveChart();
         _curveChart.AddAxis(new Axis(AxisTypes.X) {Dimension = _curve.XDimension});
         _curveChart.AddAxis(new Axis(AxisTypes.Y) {Dimension = _curve.YDimension});

         _xAxisBinder = createAxisBinderFor(_curveChart.AxisBy(AxisTypes.X));
         _yAxisBinder = createAxisBinderFor(_curveChart.AxisBy(AxisTypes.Y));

         A.CallTo(() => _axisBinderFactory.Create(_curveChart.AxisBy(AxisTypes.X), _chartDisplayView.ChartControl, _curveChart)).Returns(_xAxisBinder);
         A.CallTo(() => _axisBinderFactory.Create(_curveChart.AxisBy(AxisTypes.Y), _chartDisplayView.ChartControl, _curveChart)).Returns(_yAxisBinder);

         SetupChart();
         sut.Edit(_curveChart);
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

   public class When_exporting_the_displayed_chart_to_excel : concern_for_ChartDisplayPresenter
   {
      private readonly string _fileName = "AAA";
      private List<DataColumn> _dataColumns;
      private Func<DataColumn, string> _namingFunc;
      private Curve _invisibleCurve;

      protected override void Context()
      {
         base.Context();
         A.CallTo(_dialogCreator).WithReturnType<string>().Returns(_fileName);
         A.CallTo(() => _dataRepositoryTask.ExportToExcel(A<IEnumerable<DataColumn>>._, _fileName, A<Func<DataColumn, string>>._, true))
            .Invokes(x =>
            {
               _dataColumns = x.GetArgument<IEnumerable<DataColumn>>(0).ToList();
               _namingFunc = x.GetArgument<Func<DataColumn, string>>(2);
            });
      }

      protected override void SetupChart()
      {
         base.SetupChart();
         _curveChart.Name = "Chart";
         _curve.Visible = true;
         _curve.Name = "CurveName";
         _curveChart.AddCurve(_curve);

         var anotherRepo = DomainHelperForSpecs.ObservedData("AnotherRepo");
         _invisibleCurve = new Curve {Visible = false};
         _invisibleCurve.SetxData(anotherRepo.BaseGrid, _dimensionFactory);
         _invisibleCurve.SetyData(anotherRepo.AllButBaseGrid().First(), _dimensionFactory);
         _curveChart.AddCurve(_invisibleCurve);
      }

      protected override void Because()
      {
         sut.ExportToExcel();
      }

      [Observation]
      public void should_only_export_data_of_visible_curves()
      {
         _dataColumns.ShouldOnlyContain(_curve.yData);
      }

      [Observation]
      public void should_use_the_name_of_the_curve_as_column_name_for_the_excel_table()
      {
         _namingFunc(_curve.yData).ShouldBeEqualTo(_curve.Name);
      }

      [Observation]
      public void should_use_the_column_name_for_base_grid()
      {
         _namingFunc(_curve.xData).ShouldBeEqualTo(_curve.xData.Name);
      }

      [Observation]
      public void should_ask_the_user_for_the_location_of_the_file_to_export()
      {
         A.CallTo(() => _dialogCreator.AskForFileToSave(Captions.ExportChartToExcel, Constants.Filter.EXCEL_SAVE_FILE_FILTER, Constants.DirectoryKey.REPORT, _curveChart.Name, null)).MustHaveHappened();
      }
   }
}