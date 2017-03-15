using OSPSuite.Presentation.Presenters.Charts;

namespace OSPSuite.Presentation.Views.Charts
{
   public interface ISingleCurveSettingsModalView : IModalView<ISingleCurveSettingsModalPresenter>
   {
      void SetSummaryView(IView baseView);
   }
}