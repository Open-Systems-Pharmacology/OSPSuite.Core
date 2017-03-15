using OSPSuite.Assets;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presenters
{
   public abstract class AbstractSubPresenter<TView, TPresenter> : AbstractCommandCollectorPresenter<TView, TPresenter>, ISubPresenter
      where TPresenter : IPresenter
      where TView : IView<TPresenter>
   {
      protected AbstractSubPresenter(TView view) : base(view)
      {
      }

      public virtual ApplicationIcon Icon
      {
         get { return CanClose ? View.ApplicationIcon : ErrorIcon; }
      }

      public virtual ApplicationIcon ErrorIcon
      {
         get { return ApplicationIcons.ErrorOverlayFor(View.ApplicationIcon); }
      }
   }
}