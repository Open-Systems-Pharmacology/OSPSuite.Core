using OSPSuite.Presentation.Presenters.ObservedData;

namespace OSPSuite.Presentation.Views.ObservedData
{
   public interface IDataRepositoryChartView : IView<IDataRepositoryChartPresenter>
   {
      void AddChart(IView view);
   }
}