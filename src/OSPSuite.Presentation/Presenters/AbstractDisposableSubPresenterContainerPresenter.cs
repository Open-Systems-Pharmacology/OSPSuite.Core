using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Events;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presenters
{
   public abstract class AbstractDisposableSubPresenterContainerPresenter<TView, TPresenter, TSubPresenter> :
      AbstractDisposableCommandCollectorPresenter<TView, TPresenter>, IContainerPresenter
      where TSubPresenter : ISubPresenter
      where TView : IModalView<TPresenter>, IContainerView
      where TPresenter : IDisposablePresenter, IContainerPresenter
   {
      protected readonly ISubPresenterItemManager<TSubPresenter> _subPresenterItemManager;
      protected readonly IReadOnlyList<ISubPresenterItem> _subPresenterItems;
      protected readonly IDialogCreator _dialogCreator;

      protected AbstractDisposableSubPresenterContainerPresenter(TView view, ISubPresenterItemManager<TSubPresenter> subPresenterItemManager, IReadOnlyList<ISubPresenterItem> subPresenterItems, IDialogCreator dialogCreator)
         : base(view)
      {
         _subPresenterItemManager = subPresenterItemManager;
         _subPresenterItems = subPresenterItems;
         _dialogCreator = dialogCreator;
      }

      public void AddSubItemView(ISubPresenterItem subPresenterItem, IView view)
      {
         View.AddSubItemView(subPresenterItem, view);
      }

      protected T PresenterAt<T>(ISubPresenterItem<T> subPresenterItem) where T : TSubPresenter
      {
         return _subPresenterItemManager.PresenterAt(subPresenterItem);
      }

      public override bool ShouldClose
      {
         get
         {
            if (!HasData())
               return true;

            var shouldCancel = _dialogCreator.MessageBoxYesNo(Captions.ReallyCancel);
            return shouldCancel == ViewResult.Yes;
         }
      }

      protected virtual bool HasData()
      {
         return CommandCollector.All().Any();
      }

      public override void InitializeWith(ICommandCollector commandCollector)
      {
         base.InitializeWith(commandCollector);
         _subPresenterItemManager.InitializeWith(this, _subPresenterItems);
      }

      public override bool CanClose
      {
         get { return _subPresenterItemManager.CanClose && base.CanClose; }
      }

      public override void ReleaseFrom(IEventPublisher eventPublisher)
      {
         base.ReleaseFrom(eventPublisher);
         _subPresenterItemManager.ReleaseFrom(eventPublisher);
      }

      protected override void Cleanup()
      {
         try
         {
            ReleaseFrom(_subPresenterItemManager.EventPublisher);
         }
         finally
         {
            base.Cleanup();
         }
      }
   }
}