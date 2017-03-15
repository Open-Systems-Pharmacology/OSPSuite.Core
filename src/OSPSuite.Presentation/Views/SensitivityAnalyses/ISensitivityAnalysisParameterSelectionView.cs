using OSPSuite.Presentation.Presenters.SensitivityAnalyses;

namespace OSPSuite.Presentation.Views.SensitivityAnalyses
{
   public interface ISensitivityAnalysisParameterSelectionView : IView<ISensitivityAnalysisParameterSelectionPresenter>
   {
      void AddSimulationParametersView(IView view);
      void AddSensitivityParametersView(IView view);
      void BindTo(SensitivityAnalysisParameterSelectionPresenter sensitivityAnalysisParameterSelectionPresenter);
   }
}