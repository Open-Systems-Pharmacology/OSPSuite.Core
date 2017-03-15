using OSPSuite.Presentation.Presenters.Charts;

namespace OSPSuite.Presentation.Views.Charts
{
   public interface IChartTemplateDetailsView : IView<IChartTemplateDetailsPresenter>
   {
      void SetChartSettingsView(IView view);
      void SetCurveTemplateView(IView view);
      void SetAxisSettingsView(IView view);
      void SetChartExportSettingsView(IView view);
   }
}
