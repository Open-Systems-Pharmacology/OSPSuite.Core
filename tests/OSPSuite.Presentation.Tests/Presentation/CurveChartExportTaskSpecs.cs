using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;
using OSPSuite.Helpers;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_CurveChartExportTask : ContextSpecification<ICurveChartExportTask>
   {
      protected IDialogCreator _dialogCreator;
      protected IDataRepositoryExportTask _dataRepositoryTask;
      protected IDimensionFactory _dimensionFactory;
      protected Curve _curve;
      protected IDimension _mergedDimensionDataColumn;

      protected override void Context()
      {
         _dialogCreator = A.Fake<IDialogCreator>();
         _dataRepositoryTask = A.Fake<IDataRepositoryExportTask>();
         _dimensionFactory = A.Fake<IDimensionFactory>();
         sut = new CurveChartExportTask(_dialogCreator, _dataRepositoryTask, _dimensionFactory);

         var dataRepository = DomainHelperForSpecs.SimulationDataRepositoryFor("Sim");

         _curve = new Curve();
         _curve.SetxData(dataRepository.BaseGrid, _dimensionFactory);
         var dataColumn = dataRepository.AllButBaseGrid().First();
         _curve.SetyData(dataColumn, _dimensionFactory);

         _mergedDimensionDataColumn = A.Fake<IDimension>();
         A.CallTo(() => _dimensionFactory.MergedDimensionFor(dataRepository.BaseGrid)).Returns(dataRepository.BaseGrid.Dimension);
         A.CallTo(() => _dimensionFactory.MergedDimensionFor(dataColumn)).Returns(_mergedDimensionDataColumn);
      }
   }

   public class When_the_curve_chart_export_task_is_exporting_the_displayed_chart_to_excel : concern_for_CurveChartExportTask
   {
      private readonly string _fileName = "AAA";
      private List<DataColumn> _dataColumns;
      private Func<DataColumn, string> _namingFunc;
      private Curve _invisibleCurve;
      private CurveChart _curveChart;
      private Func<DataColumn, IDimension> _dimensionFunc;

      protected override void Context()
      {
         base.Context();
         A.CallTo(_dialogCreator).WithReturnType<string>().Returns(_fileName);
         A.CallTo(() => _dataRepositoryTask.ExportToExcel(A<IEnumerable<DataColumn>>._, _fileName, true, A<DataColumnExportOptions>._))
            .Invokes(x =>
            {
               _dataColumns = x.GetArgument<IEnumerable<DataColumn>>(0).ToList();
               _namingFunc = x.GetArgument<DataColumnExportOptions>(3).ColumnNameRetriever;
               _dimensionFunc = x.GetArgument<DataColumnExportOptions>(3).DimensionRetriever;
            });

         _curveChart = new CurveChart {Name = "Chart"};
         _curve.Visible = true;
         _curve.Name = "CurveName";
         _curveChart.AddCurve(_curve);

         var anotherRepo = DomainHelperForSpecs.ObservedData("AnotherRepo");
         _invisibleCurve = new Curve {Visible = false};
         _invisibleCurve.SetxData(anotherRepo.BaseGrid, _dimensionFactory);
         var otherDataColumn = anotherRepo.AllButBaseGrid().First();
         _invisibleCurve.SetyData(otherDataColumn, _dimensionFactory);
         _curveChart.AddCurve(_invisibleCurve);

         A.CallTo(() => _dimensionFactory.MergedDimensionFor(anotherRepo.BaseGrid)).Returns(anotherRepo.BaseGrid.Dimension);
         A.CallTo(() => _dimensionFactory.MergedDimensionFor(otherDataColumn)).Returns(_mergedDimensionDataColumn);
      }

      protected override void Because()
      {
         sut.ExportToExcel(_curveChart);
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
      public void should_use_the_merge_dimension_for_column_dimension()
      {
         _dimensionFunc(_curve.yData).ShouldBeEqualTo(_mergedDimensionDataColumn);
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

   public class When_the_curve_chart_export_task_is_exporting_the_displayed_chart_to_excel_and_the_underlying_data_are_using_a_real_curve_as_x_axis : concern_for_CurveChartExportTask
   {
      private readonly string _fileName = "AAA";
      private List<DataColumn> _dataColumns;
      private Func<DataColumn, string> _namingFunc;
      private CurveChart _curveChart;
      private Func<DataColumn, IDimension> _dimensionFunc;

      protected override void Context()
      {
         base.Context();
         A.CallTo(_dialogCreator).WithReturnType<string>().Returns(_fileName);
         A.CallTo(() => _dataRepositoryTask.ExportToExcel(A<IEnumerable<DataColumn>>._, _fileName, true, A<DataColumnExportOptions>._))
            .Invokes(x =>
            {
               _dataColumns = x.GetArgument<IEnumerable<DataColumn>>(0).ToList();
               _namingFunc = x.GetArgument<DataColumnExportOptions>(3).ColumnNameRetriever;
               _dimensionFunc = x.GetArgument<DataColumnExportOptions>(3).DimensionRetriever;
            });

         _curveChart = new CurveChart {Name = "Chart"};
         _curve.Visible = true;
         _curve.Name = "CurveName";

         var anotherRepo = DomainHelperForSpecs.ObservedData("OBS_DATA");
         var dataColumn = anotherRepo.AllButBaseGrid().First();
         _curve.SetxData(dataColumn, _dimensionFactory);
         A.CallTo(() => _dimensionFactory.MergedDimensionFor(dataColumn)).Returns(_mergedDimensionDataColumn);

         _curveChart.AddCurve(_curve);
      }

      protected override void Because()
      {
         sut.ExportToExcel(_curveChart);
      }

      [Observation]
      public void should_export_x_data_and_y_data()
      {
         _dataColumns.ShouldOnlyContain(_curve.yData, _curve.xData);
      }

      [Observation]
      public void should_use_the_name_of_the_curve_as_column_name_for_the_excel_table()
      {
         _namingFunc(_curve.yData).ShouldBeEqualTo(_curve.Name);
      }

      [Observation]
      public void should_use_the_name_of_the_column_for_the_x_data()
      {
         _namingFunc(_curve.xData).ShouldBeEqualTo(_curve.xData.Name);
      }

      [Observation]
      public void should_use_the_merge_dimension_for_column_dimension()
      {
         _dimensionFunc(_curve.yData).ShouldBeEqualTo(_mergedDimensionDataColumn);
         _dimensionFunc(_curve.xData).ShouldBeEqualTo(_mergedDimensionDataColumn);
      }

      [Observation]
      public void should_ask_the_user_for_the_location_of_the_file_to_export()
      {
         A.CallTo(() => _dialogCreator.AskForFileToSave(Captions.ExportChartToExcel, Constants.Filter.EXCEL_SAVE_FILE_FILTER, Constants.DirectoryKey.REPORT, _curveChart.Name, null)).MustHaveHappened();
      }
   }
}