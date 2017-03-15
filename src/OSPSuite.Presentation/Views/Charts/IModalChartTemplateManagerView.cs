using OSPSuite.Presentation.Presenters.Charts;

namespace OSPSuite.Presentation.Views.Charts
{
   public interface IModalChartTemplateManagerView : IModalView<IModalChartTemplateManagerPresenter>
   {
      void SetBaseView(IView baseView);
   }
}
