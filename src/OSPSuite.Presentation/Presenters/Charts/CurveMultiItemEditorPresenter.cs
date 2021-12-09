using System.Drawing;
using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Views.Charts;

namespace OSPSuite.Presentation.Presenters.Charts
{
   public enum BooleanComboBox
   {
      Default,
      Yes,
      No
   }

   public enum MultiEditLineStyles
   {
      None,
      Solid,
      Dash,
      Dot,
      DashDot
   }

   public enum MultiEditSymbols
   {
      None,
      Circle,
      Diamond,
      Triangle,
      Square
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
      public LineStyles? Style { get; set; }
      public Symbols? Symbol { get; set; }
      public bool? Visible { get; set; }
      public bool? VisibleInLegend { get; set; }
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