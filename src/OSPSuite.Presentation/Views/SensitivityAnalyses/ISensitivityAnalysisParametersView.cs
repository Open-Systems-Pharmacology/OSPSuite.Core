using System.Collections.Generic;
using OSPSuite.Presentation.DTO.SensitivityAnalyses;
using OSPSuite.Presentation.Presenters.SensitivityAnalyses;

namespace OSPSuite.Presentation.Views.SensitivityAnalyses
{
   public interface ISensitivityAnalysisParametersView : IView<ISensitivityAnalysisParametersPresenter>
   {
      void BindTo(IReadOnlyList<SensitivityParameterDTO> allSensitivityParameterDtos);
      IEnumerable<SensitivityParameterDTO> SelectedParameters();
      void SetNMaxView(IView view);
      void SetRangeView(IView view);
   }
}