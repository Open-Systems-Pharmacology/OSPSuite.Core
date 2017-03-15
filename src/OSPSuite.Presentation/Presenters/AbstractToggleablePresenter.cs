using System.Drawing;
using OSPSuite.Presentation.Presenters.Main;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presenters
{
   public interface IToogleablePresenter : IMainViewItemPresenter
   {
      void Display();

      void FormClosing(Point location, Size size);
   }

   public abstract class AbstractToggleablePresenter<TView, TPresenter> : AbstractCommandCollectorPresenter<TView, TPresenter>, IToogleablePresenter
      where TPresenter : IToogleablePresenter where TView : IView<TPresenter>, IToggleableView
   {
      protected AbstractToggleablePresenter(TView view) : base(view)
      {
      }

      public void ToggleVisibility()
      {
         _view.ToggleVisibility();
      }

      private void hideView()
      {
         _view.Hide();
      }

      public abstract void Display();

      protected void DisplayViewAt(Point location, Size size)
      {
         _view.SetFormLayout(location, size);
         _view.Display();
      }

      public virtual void FormClosing(Point location, Size size)
      {
         hideView();
         SaveFormLayout(location, size);
      }

      protected abstract void SaveFormLayout(Point location, Size size);
   }
}