using System;
using System.Collections.Generic;
using OSPSuite.Utility.Events;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presenters
{
   public abstract class SingleStartContainerPresenter<TView, TPresenter, TSubject, TSubPresenter> : SubjectPresenter<TView, TPresenter, TSubject>,
      IContainerPresenter
      where TView : IView<TPresenter>, IMdiChildView, IContainerView
      where TPresenter : IPresenter
      where TSubPresenter : ISubPresenter
   {
      protected readonly ISubPresenterItemManager<TSubPresenter> _subPresenterItemManager;
      private readonly IReadOnlyList<ISubPresenterItem> _subPresenterItems;

      protected SingleStartContainerPresenter(TView view, ISubPresenterItemManager<TSubPresenter> subPresenterItemManager, IReadOnlyList<ISubPresenterItem> subPresenterItems)
         : base(view)
      {
         _subPresenterItemManager = subPresenterItemManager;
         _subPresenterItems = subPresenterItems;
      }

      public virtual void AddSubItemView(ISubPresenterItem subPresenterItem, IView view)
      {
         View.AddSubItemView(subPresenterItem, view);
      }

      public override void InitializeWith(ICommandCollector commandRegister)
      {
         base.InitializeWith(commandRegister);
         _subPresenterItemManager.InitializeWith(this, _subPresenterItems);
      }

      public override void ReleaseFrom(IEventPublisher eventPublisher)
      {
         base.ReleaseFrom(eventPublisher);
         _subPresenterItemManager.ReleaseFrom(eventPublisher);
      }

      protected T PresenterAt<T>(ISubPresenterItem<T> subPresenterItem) where T : TSubPresenter
      {
         return _subPresenterItemManager.PresenterAt(subPresenterItem);
      }

      public override void OnFormClosed()
      {
         base.OnFormClosed();
         Cleanup();
      }

      public override void Close()
      {
         base.Close();
         Cleanup();
      }

      public override bool CanClose
      {
         get { return _subPresenterItemManager.CanClose && base.CanClose; }
      }

      protected virtual void Cleanup()
      {
         ReleaseFrom(_subPresenterItemManager.EventPublisher);
      }

      public virtual bool ShouldClose
      {
         get { return true; }
      }

      #region Disposable properties

      private bool _disposed;

      public void Dispose()
      {
         if (_disposed) return;

         Cleanup();
         GC.SuppressFinalize(this);
         _disposed = true;
      }

      ~SingleStartContainerPresenter()
      {
         Cleanup();
      }

      #endregion
   }
}