using OSPSuite.Presentation.Presenters.ParameterIdentifications;

namespace OSPSuite.Presentation.Views.ParameterIdentifications
{
   public interface IParameterIdentificationDataSelectionView : IView<IParameterIdentificationDataSelectionPresenter>
   {
      void AddSimulationSelectionView(IView view);
      void AddOutputMappingView(IView view);
      void AddWeightedObservedDataCollectorView(IView view);
   }
}