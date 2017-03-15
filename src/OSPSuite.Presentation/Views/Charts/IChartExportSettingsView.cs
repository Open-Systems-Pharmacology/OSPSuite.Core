using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Presenters.Charts;

namespace OSPSuite.Presentation.Views.Charts
{
   public interface IChartExportSettingsView : IView<IChartExportSettingsPresenter>
   {
      void BindToSource(IChartManagement chart);
      void Refresh();
      void DeleteBinding();
   }
}