using OSPSuite.Presentation.Presenters.Charts;

namespace OSPSuite.Presentation.Views.Charts
{
   public interface ICurveMultiItemEditorView : IModalView<ICurveMultiItemEditorPresenter>
   {
      void BindTo(SelectedCurveValues selectedCurveValues);
   }
}
