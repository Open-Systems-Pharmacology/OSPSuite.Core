using OSPSuite.Presentation.Views;
using OSPSuite.Starter.Presenters;

namespace OSPSuite.Starter.Views
{
   public interface IDataRepositoryTestView : IView<IDataRepositoryTestPresenter>
   {
      void AddChartView(IView baseView);
      void AddDataView(IView baseView);
      void AddMetaDataView(IView baseView);
   }
}
