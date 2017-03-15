using System;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presenters
{
   public abstract class SingleStartPresenter<TView, TPresenter> : AbstractCommandCollectorPresenter<TView, TPresenter>, ISingleStartPresenter
      where TView : IView<TPresenter>, IMdiChildView
      where TPresenter : IPresenter
   {
      public event EventHandler Closing = delegate { };

      protected SingleStartPresenter(TView view) : base(view)
      {
      }

      public void Handle(RenamedEvent eventToHandle)
      {
         if (!Equals(eventToHandle.RenamedObject, Subject)) return;

         UpdateCaption();
      }

      public virtual void OnFormClosed()
      {
         Closing(this, EventArgs.Empty);
      }

      public virtual void Close()
      {
         _view.CloseView();
      }

      public abstract object Subject { get; }

      public virtual IPresentationSettings GetSettings()
      {
         //per default return an empty setting.
         return new DefaultPresentationSettings();
      }

      public virtual void RestoreSettings(IPresentationSettings settings)
      {
         //per default do nothing
      }

      public abstract void Edit(object subject);

      public virtual void SaveChanges()
      {
         _view.SaveChanges();
      }

      public virtual void Activated()
      {
         //nothing to do here
      }

      protected abstract void UpdateCaption();
   }
}