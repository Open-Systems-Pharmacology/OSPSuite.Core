using System.Drawing;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.Events;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Events;

namespace OSPSuite.Presentation.Presenters.Main
{
   public abstract class AbstractMainViewPresenter<TView, TPresenter> : AbstractPresenter<TView, TPresenter>, IMainViewPresenter
      where TView : IMainView, IView<TPresenter>
      where TPresenter : IMainViewPresenter
   {
      protected readonly IEventPublisher _eventPublisher;
      private readonly ITabbedMdiChildViewContextMenuFactory _contextMenuFactory;

      protected AbstractMainViewPresenter(TView view, IEventPublisher eventPublisher, ITabbedMdiChildViewContextMenuFactory contextMenuFactory)
         : base(view)
      {
         _eventPublisher = eventPublisher;
         _contextMenuFactory = contextMenuFactory;
      }

      public virtual void ShowContextMenu(IMdiChildView childView, Point popupLocation)
      {
         if (childView == null) return;
         var contextMenu = _contextMenuFactory.CreateFor(childView, this);
         contextMenu.Show(_view, popupLocation);
      }

      public virtual void SaveChanges()
      {
         ActivePresenter?.SaveChanges();
      }

      public abstract void Run();
      public abstract void RemoveAlert();
      public abstract void OpenFile(string fileName);

      public virtual ISingleStartPresenter ActivePresenter
      {
         get
         {
            var activeView = View.ActiveView;
            return activeView?.Presenter;
         }
      }

      public virtual void Activate(IMdiChildView childView)
      {
         if (childView == null)
            _eventPublisher.PublishEvent(new NoActiveScreenEvent());
         else
            _eventPublisher.PublishEvent(new ScreenActivatedEvent(childView.Presenter));
      }

      public virtual void Handle(HeavyWorkStartedEvent eventToHandle)
      {
         View.InWaitCursor(true, eventToHandle.ForceCursorChange);
      }

      public virtual void Handle(HeavyWorkFinishedEvent eventToHandle)
      {
         View.InWaitCursor(false, eventToHandle.ForceCursorChange);
      }

      public override bool CanClose
      {
         get
         {
            var activePresenter = ActivePresenter;
            return activePresenter == null || activePresenter.CanClose;
         }
      }

      public virtual void Handle(RollBackStartedEvent eventToHandle)
      {
         View.InWaitCursor(true, true);
      }

      public virtual void Handle(RollBackFinishedEvent eventToHandle)
      {
         View.InWaitCursor(false, true);
      }
   }
}