using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presenters
{
   public interface IWizardPresenter : IContainerPresenter
   {
      /// <summary>
      ///    Set the next available item after previous index
      /// </summary>
      /// <param name="currentIndex">the index selected when next was called</param>
      void WizardNext(int currentIndex);

      /// <summary>
      ///    Set the previous available item after previous index
      /// </summary>
      /// <param name="currentIndex">the index selected when previous was called</param>
      void WizardPrevious(int currentIndex);

      /// <summary>
      ///    Set the current item
      /// </summary>
      /// <param name="currentIndex">index selected when current was called</param>
      /// <param name="newIndex">index that ought to be selected</param>
      void WizardCurrent(int currentIndex, int newIndex);

      /// <summary>
      ///    Determines if the button "OK" or "Finish" can be displayed at the same time as the button "Next".
      ///    This can be useful, if for instance some steps in the workflow are not mandatory and can be skipped/
      ///    Default value is false.
      /// </summary>
      bool AllowQuickFinish { get; set; }
   }

   public abstract class WizardPresenter<TView, TPresenter, TSubPresenter> : AbstractDisposableSubPresenterContainerPresenter<TView, TPresenter, TSubPresenter>, IWizardPresenter
      where TSubPresenter : ISubPresenter
      where TView : IWizardView, IModalView<TPresenter>
      where TPresenter : IDisposablePresenter, IContainerPresenter
   {
      public bool AllowQuickFinish { get; set; }

      protected WizardPresenter(TView view, ISubPresenterItemManager<TSubPresenter> subPresenterItemManager, IReadOnlyList<ISubPresenterItem> subPresenterItems, IDialogCreator dialogCreator)
         : base(view, subPresenterItemManager, subPresenterItems, dialogCreator)
      {
         AllowQuickFinish = false;
      }

      public override void InitializeWith(ICommandCollector commandCollector)
      {
         base.InitializeWith(commandCollector);
         _subPresenterItemManager.AllSubPresenters.Each(presenter => presenter.StatusChanged += subPresenterChanged);
         _view.InitializeWizard();
      }

      private void subPresenterChanged(object sender, EventArgs e)
      {
         UpdateControls();
      }

      public override void ReleaseFrom(IEventPublisher eventPublisher)
      {
         _subPresenterItemManager.AllSubPresenters.Each(presenter => presenter.StatusChanged -= subPresenterChanged);
         base.ReleaseFrom(eventPublisher);
      }

      protected virtual void UpdateControls()
      {
         SetWizardButtonEnabled(selectedIndex);
      }

      /// <summary>
      ///    Entry point for derived presenter who can decided how the buttons and tabs should be activated
      ///    depending the the index receiving the focus
      /// </summary>
      /// <param name="currentIndex">selected index</param>
      protected abstract void UpdateControls(int currentIndex);

      protected void UpdateViewStatus()
      {
         _subPresenterItemManager.AllSubPresenters.Each(updateViewStatus);
      }

      private void updateViewStatus(TSubPresenter subPresenter)
      {
         _view.SetControlIcon(_subPresenterItemManager.SubPresenterItemFor(subPresenter), subPresenter.Icon);
      }

      public virtual void WizardNext(int currentIndex)
      {
         var nextAvailableIndex = retrieveIndexAfter(currentIndex);
         WizardCurrent(currentIndex, nextAvailableIndex);
         ActivateControl(nextAvailableIndex);
      }

      public virtual void WizardPrevious(int currentIndex)
      {
         var previousAvailableIndex = retrieveIndexBefore(currentIndex);
         WizardCurrent(currentIndex, previousAvailableIndex);
         ActivateControl(previousAvailableIndex);
      }

      private int retrieveIndexAfter(int previousIndex)
      {
         int indexAfter = previousIndex + 1;
         while (!_view.IsControlVisible(ItemAtPosition(indexAfter)) && indexAfter < LastIndex)
         {
            indexAfter++;
         }

         return indexAfter;
      }

      private int retrieveIndexBefore(int previousIndex)
      {
         int indexBefore = previousIndex - 1;
         while (!_view.IsControlVisible(ItemAtPosition(indexBefore)) && indexBefore > FirstIndex)
         {
            indexBefore--;
         }

         return indexBefore;
      }

      protected TSubPresenter PresenterAtPosition(int position)
      {
         return _subPresenterItemManager.PresenterAtPosition(position);
      }

      protected ISubPresenterItem ItemAtPosition(int position)
      {
         return _subPresenterItemManager.ItemAtPosition(position);
      }
      
      protected void ActivateControl(ISubPresenterItem subPresenterItem)
      {
         ActivateControl(subPresenterItem.Index);
      }

      protected void ActivateControl(int position)
      {
         _view.ActivateControl(ItemAtPosition(position));
      }

      protected int FirstIndex
      {
         get { return 0; }
      }

      protected int LastIndex
      {
         get { return _subPresenterItemManager.AllSubPresenters.Count() - 1; }
      }

      protected void SetWizardButtonEnabled(ISubPresenterItem subPresenter)
      {
         SetWizardButtonEnabled(subPresenter.Index);
      }

      protected void SetWizardButtonEnabled(int currentIndex)
      {
         _view.SetButtonsVisible(nextVisible(currentIndex), okVisible(currentIndex));

         //Previous is by default always enabled if the index gettint the focus is not the first one
         _view.PreviousEnabled = (currentIndex != FirstIndex);

         UpdateControls(currentIndex);

         //in any case, next button is disabled if the index is the last 
         updateNextEnabled(currentIndex);
      }

      private void updateNextEnabled(int indexThatWillHaveFocus)
      {
         if (indexThatWillHaveFocus == LastIndex)
            _view.NextEnabled = false;
      }

      private bool okVisible(int indexThatWillHaveFocus)
      {
         return (indexThatWillHaveFocus == LastIndex) || AllowQuickFinish;
      }

      private bool nextVisible(int indexThatWillHaveFocus)
      {
         return (indexThatWillHaveFocus != LastIndex) || AllowQuickFinish;
      }

      public virtual void WizardCurrent(int currentIndex, int newIndex)
      {
         SetWizardButtonEnabled(newIndex);
      }

      public override void ViewChanged()
      {
         UpdateControls(selectedIndex);
         updateNextEnabled(selectedIndex);
      }

      private int selectedIndex
      {
         get { return _view.SelectedPageIndex; }
      }
   }
}