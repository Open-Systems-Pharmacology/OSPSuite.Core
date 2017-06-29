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
      private readonly IDataRepositoryTask _dataRepositoryTask;
      private readonly IDimensionFactory _dimensionFactory;

      public CurveChartExportTask(IDialogCreator dialogCreator, IDataRepositoryTask dataRepositoryTask, IDimensionFactory dimensionFactory)
      {
         _dialogCreator = dialogCreator;
         _dataRepositoryTask = dataRepositoryTask;
         _dimensionFactory = dimensionFactory;
      }

      public void ExportToExcel(CurveChart chart)
      {
         if (chart == null) return;
         var visibleCurves = chart.Curves.Where(x => x.Visible).ToList();
         if (!visibleCurves.Any()) return;

         var fileName = _dialogCreator.AskForFileToSave(Captions.ExportChartToExcel, Constants.Filter.EXCEL_SAVE_FILE_FILTER, Constants.DirectoryKey.REPORT, chart.Name);
         if (string.IsNullOrEmpty(fileName)) return;

         var dataColumnCache = new Cache<DataColumn, Curve>(onMissingKey: x => null);
         visibleCurves.Each(curve => dataColumnCache[curve.yData] = curve);
         _dataRepositoryTask.ExportToExcel(dataColumnCache.Keys, fileName, col => dataColumnCache[col]?.Name ?? col.Name, _dimensionFactory.MergedDimensionFor);
      }
   }
}