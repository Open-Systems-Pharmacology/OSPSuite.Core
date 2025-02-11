using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Services
{
   public interface ICurveChartExportTask
   {
      /// <summary>
      /// Export the chart to excel
      /// </summary>
      /// <param name="chart">Chart to export</param>
      /// <param name="preExportHook">Hook that can be executed on the dataColumn to modify them, reorder them etc..</param>
      void ExportToExcel(CurveChart chart, Func<IEnumerable<DataColumn>, IEnumerable<DataColumn>> preExportHook = null);
   }

   public class CurveChartExportTask : ICurveChartExportTask
   {
      private readonly IDialogCreator _dialogCreator;
      private readonly IDataRepositoryExportTask _dataRepositoryExportTask;
      private readonly IDimensionFactory _dimensionFactory;

      public CurveChartExportTask(IDialogCreator dialogCreator, IDataRepositoryExportTask dataRepositoryExportTask, IDimensionFactory dimensionFactory)
      {
         _dialogCreator = dialogCreator;
         _dataRepositoryExportTask = dataRepositoryExportTask;
         _dimensionFactory = dimensionFactory;
      }

      public void ExportToExcel(CurveChart chart, Func<IEnumerable<DataColumn>, IEnumerable<DataColumn>> preExportHook = null)
      {
         if (chart == null)
            return;

         var visibleCurves = chart.Curves.Where(x => x.Visible).ToList();
         if (!visibleCurves.Any())
            return;

         var fileName = _dialogCreator.AskForFileToSave(Captions.ExportChartToExcel, Constants.Filter.EXCEL_SAVE_FILE_FILTER, Constants.DirectoryKey.REPORT, chart.Name);
         if (string.IsNullOrEmpty(fileName))
            return;

         // Goal is to use the curve name if it's defined instead of the data column name
         var dataColumnCache = new Cache<DataColumn, Curve>(onMissingKey: x => null);
         visibleCurves.Each(curve => dataColumnCache[curve.yData] = curve);

         //Base grid are added by default to the export unless the data represents an amount vs obs data. In that case, the base grid might be another column
         var otherColumnsToExport = visibleCurves.Select(x => x.xData).Where(x => !x.IsBaseGrid()).ToList();

         var exportOptions = new DataColumnExportOptions
         {
            ColumnNameRetriever = col => dataColumnCache[col]?.Name ?? col.Name,
            DimensionRetriever = _dimensionFactory.MergedDimensionFor
         };

         var allColumns = dataColumnCache.Keys.Union(otherColumnsToExport);
         var allColumnsToExport = preExportHook?.Invoke(allColumns) ?? allColumns;
         _dataRepositoryExportTask.ExportToExcel(allColumnsToExport, fileName, exportOptions: exportOptions);
      }
   }
}