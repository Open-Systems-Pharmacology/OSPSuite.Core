using OSPSuite.Presentation.Presenters.Charts;

namespace OSPSuite.Presentation.Views.Charts
{
   public interface ICurveMultiItemEditorView : IModalView<ICurveMultiItemEditorPresenter>
   {
      SelectedCurveValues GetSelectedValues();
   }
}
