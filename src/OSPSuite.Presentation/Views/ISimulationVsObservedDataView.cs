using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Views
{ 
   public interface ISimulationVsObservedDataView : IView<ISimulationVsObservedDataPresenter>, ISimulationAnalysisView
   {
      void SetTotalError(double totalError);
   }
}
