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
      void ExportToExcel(CurveChart chart);
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

      public void ExportToExcel(CurveChart chart)
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
         var otherColumnsToExport = visibleCurves.Select(x => x.xData).Where(x => !x.IsBaseGrid());

         var exportOptions = new DataColumnExportOptions
         {
            ColumnNameRetriever = col => dataColumnCache[col]?.Name ?? col.Name,
            DimensionRetriever = _dimensionFactory.MergedDimensionFor
         };
         _dataRepositoryExportTask.ExportToExcel(dataColumnCache.Keys.Union(otherColumnsToExport), fileName, exportOptions: exportOptions);
      }
   }
}