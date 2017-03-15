using OSPSuite.Presentation.Presenters.Charts;

namespace OSPSuite.Presentation.Views.Charts
{
   public interface ISingleAxisSettingsModalView : IModalView<ISingleAxisSettingsModalPresenter>
   {
      /// <summary>
      /// Sets the view for the axis settings
      /// </summary>
      /// <param name="view">view shich displays axis settings</param>
      void SetSummaryView(IView view);
   }
}