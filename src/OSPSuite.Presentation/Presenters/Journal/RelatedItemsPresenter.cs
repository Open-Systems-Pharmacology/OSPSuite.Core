using OSPSuite.Core.Journal;
using OSPSuite.Presentation.Views.Journal;

namespace OSPSuite.Presentation.Presenters.Journal
{
   public interface IRelatedItemsPresenter : IPresenter<IRelatedItemsView>
   {
      void DeleteRelatedItem(RelatedItem relatedItem);
      void StartComparisonFor(RelatedItem relatedItem);
      void Edit(JournalPage journalPage);
      void DeleteBinding();
      void ReloadRelatedItem(RelatedItem relatedItem);
      void AddRelatedItemFromFile();
      void ReloadAllRelatedItems();
   }

   public class RelatedItemsPresenter : AbstractCommandCollectorPresenter<IRelatedItemsView, IRelatedItemsPresenter>, IRelatedItemsPresenter
   {
      private readonly IJournalRetriever _journalRetriever;
      private readonly IJournalPageTask _journalPageTask;
      private readonly IRelatedItemComparablePresenter _relatedItemComparablePresenter;
      private readonly IReloadRelatedItemTask _reloadRelatedItemTask;
      private JournalPage _journalPage;

      public RelatedItemsPresenter(
         IRelatedItemsView view, 
         IJournalRetriever journalRetriever,
         IJournalPageTask journalPageTask, 
         IRelatedItemComparablePresenter relatedItemComparablePresenter,
         IReloadRelatedItemTask reloadRelatedItemTask) : base(view)
      {
         _journalRetriever = journalRetriever;
         _journalPageTask = journalPageTask;
         _relatedItemComparablePresenter = relatedItemComparablePresenter;
         _reloadRelatedItemTask = reloadRelatedItemTask;
         _view.AddComparableView(_relatedItemComparablePresenter.View);
         AddSubPresenters(_relatedItemComparablePresenter);
      }

      public void DeleteRelatedItem(RelatedItem relatedItem)
      {
         _journalPageTask.DeleteRelatedItemFrom(_journalRetriever.Current, relatedItem);
         rebind();
      }

      public void StartComparisonFor(RelatedItem relatedItem)
      {
         _relatedItemComparablePresenter.StartComparisonFor(relatedItem);
      }

      public void Edit(JournalPage journalPage)
      {
         _journalPage = journalPage;
         rebind();
      }

      public void DeleteBinding()
      {
         _view.DeleteBinding();
         _journalPage = null;
      }

      public void ReloadRelatedItem(RelatedItem relatedItem)
      {
         _reloadRelatedItemTask.Load(relatedItem);
      }

      public void AddRelatedItemFromFile()
      {
         _journalPageTask.AddRelatedItemFromFile(_journalPage);
         rebind();
      }

      public void ReloadAllRelatedItems()
      {
         _reloadRelatedItemTask.ImportAllIntoApplication(_journalPage.RelatedItems);
      }

      private void rebind()
      {
         _view.BindTo(_journalPage.RelatedItems);
      }
   }
}