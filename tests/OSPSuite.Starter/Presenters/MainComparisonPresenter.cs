using OSPSuite.Core.Commands;
using OSPSuite.Core.Services;
using OSPSuite.Presentation;
using OSPSuite.Presentation.Presenters.Comparisons;
using OSPSuite.Presentation.Regions;
using OSPSuite.Presentation.Views.Comparisons;

namespace OSPSuite.Starter.Presenters
{
   public class MainComparisonPresenter : OSPSuite.Presentation.Presenters.Comparisons.MainComparisonPresenter
   {
      public MainComparisonPresenter(IMainComparisonView view, IRegionResolver regionResolver, IComparisonPresenter comparisonPresenter, IComparerSettingsPresenter comparerSettingsPresenter,
         IPresentationUserSettings presentationUserSettings, IDialogCreator dialogCreator, IExportDataTableToExcelTask exportToExcelTask, IOSPSuiteExecutionContext executionContext) :
            base(view, regionResolver, comparisonPresenter, comparerSettingsPresenter, presentationUserSettings, dialogCreator, exportToExcelTask, executionContext, RegionNames.Comparison)
      {
      }
   }
}