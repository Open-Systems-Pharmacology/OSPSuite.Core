using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters.SensitivityAnalyses;

namespace OSPSuite.Presentation.Views.SensitivityAnalyses
{
   public interface ISensitivityAnalysisSetValueView : IView<ISensitivityAnalysisSetValuePresenter>
   {
      void BindTo(IParameterDTO parameterDTO);
      void ConfigureForUInt();
   }
}
