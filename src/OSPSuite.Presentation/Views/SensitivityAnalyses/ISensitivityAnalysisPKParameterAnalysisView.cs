using System.Collections.Generic;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters.SensitivityAnalyses;

namespace OSPSuite.Presentation.Views.SensitivityAnalyses
{
   public interface ISensitivityAnalysisPKParameterAnalysisView : 
      IView<ISensitivityAnalysisPKParameterAnalysisPresenter>,
      ICanCopyToClipboardWithWatermark
   {
      void BindTo(SensitivityAnalysisPKParameterAnalysisPresenter sensitivityAnalysisPKParameterAnalysisPresenter);
      void UpdateChart(IReadOnlyList<PKParameterSensitivity> allPKParameterSensitivities, string pkParameterSelection);
   }
}
