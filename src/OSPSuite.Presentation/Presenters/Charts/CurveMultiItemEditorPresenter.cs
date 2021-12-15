using System.Drawing;
using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Views.Charts;

namespace OSPSuite.Presentation.Presenters.Charts
{
   public enum YesNoValues
   {
      Yes,
      No
   }

   public interface ICurveMultiItemEditorPresenter : IDisposablePresenter, IPresenter<ICurveMultiItemEditorView>
   {
      void Show();
      bool Canceled();
      SelectedCurveValues GetSelectedValues();
   }

   public class SelectedCurveValues
   {
      public Color? Color { get; set; }
      public string Style { get; set; }
      public string Symbol { get; set; }
      public string Visible { get; set; }
      public string VisibleInLegend { get; set; }
   }

   public class CurveMultiItemEditorPresenter : AbstractDisposablePresenter<ICurveMultiItemEditorView, ICurveMultiItemEditorPresenter>,
      ICurveMultiItemEditorPresenter
   {
      public CurveMultiItemEditorPresenter(ICurveMultiItemEditorView view) : base(view)
      {
      }

      public void Show()
      {
         _view.Display();
      }

      public bool Canceled()
      {
         return _view.Canceled;
      }
      public SelectedCurveValues GetSelectedValues()
      {
         return _view.GetSelectedValues();
      }
   }
}