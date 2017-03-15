using OSPSuite.Presentation.Presenters.ParameterIdentifications;

namespace OSPSuite.Presentation.Views.ParameterIdentifications
{
   public interface IParameterIdentificationWeightedObservedDataView : IView<IParameterIdentificationWeightedObservedDataPresenter>
   {
      void AddDataView(IView view);
      void AddChartView(IView view);
   }
}