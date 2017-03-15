using System.Collections.Generic;
using OSPSuite.Presentation.Core;

namespace OSPSuite.Presentation.Presenters.SensitivityAnalyses
{

   public static class SensitivityAnalysisItems
   {
      public static readonly SensitivityAnalysisItem<ISensitivityAnalysisParameterSelectionPresenter> Parameters = new SensitivityAnalysisItem<ISensitivityAnalysisParameterSelectionPresenter>();
      public static readonly SensitivityAnalysisItem<ISensitivityAnalysisResultsPresenter> Results = new SensitivityAnalysisItem<ISensitivityAnalysisResultsPresenter>();
      public static readonly IReadOnlyList<ISubPresenterItem> All = new List<ISubPresenterItem> { Parameters, Results };
   }

   public class SensitivityAnalysisItem<TSubPresenter> : SubPresenterItem<TSubPresenter> where TSubPresenter : ISensitivityAnalysisItemPresenter
   {
   }
}