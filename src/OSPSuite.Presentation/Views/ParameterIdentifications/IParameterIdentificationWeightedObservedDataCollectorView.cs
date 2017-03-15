using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;

namespace OSPSuite.Presentation.Views.ParameterIdentifications
{
   public interface IParameterIdentificationWeightedObservedDataCollectorView : IView<IParameterIdentificationWeightedObservedDataCollectorPresenter>, IBatchUpdatable
   {
      void AddObservedDataView(IView view);
      void RemoveObservedDataView(IView view);
      void SelectObservedDataView(IView view);
      void Clear();
   }
}