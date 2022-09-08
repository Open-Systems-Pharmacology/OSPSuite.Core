using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Views
{ 
   public interface ISimulationVsObservedDataView : IView<ISimulationRunAnalysisPresenter>, ISimulationAnalysisView
   {
      void SetTotalError(double totalError);
   }
}
