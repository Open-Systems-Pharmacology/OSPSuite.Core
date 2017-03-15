using System.Drawing;
using OSPSuite.Utility.Extensions;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Extensions
{
   public static class PresenterWithContextMenuExtensions
   {
      public static PresenterWithContextMenuExpression<TObject> CreatePopupMenuFor<TObject>(this IPresenterWithContextMenu<TObject> presenter, TObject objectRequestingPopup)
      {
         return new PresenterWithContextMenuExpression<TObject>(presenter, objectRequestingPopup);
      }
   }

   public class PresenterWithContextMenuExpression<TObject>
   {
      private readonly IPresenterWithContextMenu<TObject> _presenter;
      private readonly TObject _objectRequestingPopup;

      public PresenterWithContextMenuExpression(IPresenterWithContextMenu<TObject> presenter, TObject objectRequestingPopup)
      {
         _presenter = presenter;
         _objectRequestingPopup = objectRequestingPopup;
      }

      public void At(Point point)
      {
         this.DoWithinExceptionHandler(() => _presenter.ShowContextMenu(_objectRequestingPopup, point));
      }

      public void At(int x, int y)
      {
         At(new Point(x, y));
      }
   }
}