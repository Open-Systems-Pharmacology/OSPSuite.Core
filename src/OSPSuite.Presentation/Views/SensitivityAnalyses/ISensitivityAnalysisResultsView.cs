using System.Data;
using OSPSuite.Presentation.Presenters.SensitivityAnalyses;

namespace OSPSuite.Presentation.Views.SensitivityAnalyses
{
   public interface ISensitivityAnalysisResultsView : IView<ISensitivityAnalysisResultsPresenter>
   {
      void HideResultsView();
      void BindTo(DataTable dataTable);
      DataTable GetSummaryData();
   }
}