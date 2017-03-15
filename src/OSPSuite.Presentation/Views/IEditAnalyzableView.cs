using OSPSuite.Assets;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Views
{
   public interface IEditAnalyzableView : IMdiChildView
   {
      void AddAnalysis(ISimulationAnalysisPresenter simulationAnalysisPresenter);
      void RemoveAnalysis(ISimulationAnalysisPresenter simulationAnalysisPresenter);
      void UpdateTrafficLightFor(ISimulationAnalysisPresenter simulationAnalysisPresenter, ApplicationIcon icon);
      void SelectTabByIndex(int tabIndex);
   }
}