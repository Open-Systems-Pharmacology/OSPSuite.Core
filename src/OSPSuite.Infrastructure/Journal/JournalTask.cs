using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Journal;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Journal.Commands;
using OSPSuite.Infrastructure.Journal.Queries;

namespace OSPSuite.Infrastructure.Journal
{
   public class JournalTask : IJournalTask
   {
      private readonly IJournalPageFactory _journalPageFactory;
      private readonly IDatabaseMediator _databaseMediator;
      private readonly IJournalRetriever _journalRetriever;
      private readonly IEventPublisher _eventPublisher;
      private readonly IJournalSessionConnector _journalSessionConnector;
      private readonly IRelatedItemFactory _relatedItemFactory;
      private readonly IJournalPageTask _journalPageTask;
      private readonly IDialogCreator _dialogCreator;
      private const int BYTE_COUNT_FOR_DESCRIPTION = 50;

      public JournalTask(IJournalPageFactory journalPageFactory,
         IDatabaseMediator databaseMediator, IJournalRetriever journalRetriever,
         IEventPublisher eventPublisher, IJournalSessionConnector journalSessionConnector,
         IRelatedItemFactory relatedItemFactory, IJournalPageTask journalPageTask, IDialogCreator dialogCreator)
      {
         _journalPageFactory = journalPageFactory;
         _databaseMediator = databaseMediator;
         _journalRetriever = journalRetriever;
         _eventPublisher = eventPublisher;
         _journalSessionConnector = journalSessionConnector;
         _relatedItemFactory = relatedItemFactory;
         _journalPageTask = journalPageTask;
         _dialogCreator = dialogCreator;
      }

      public JournalPage CreateJournalPage(bool showEditor = false)
      {
         if (!isConnectedToJournal())
            return null;

         var lastCreatedPage = journal.LastCreatedJournalPage;
         var journalPage = _journalPageFactory.Create();
         SaveJournalPage(journalPage);

         setDefaultParentJournalPageFor(journalPage, lastCreatedPage);
         Edit(journalPage, showEditor);

         return journalPage;
      }

      private void setDefaultParentJournalPageFor(JournalPage journalPage, JournalPage lastCreatedPage)
      {
         _journalPageTask.AddAsParentTo(journalPage, lastCreatedPage);
      }

      private bool isConnectedToJournal()
      {
         if (_journalSessionConnector.IsConnected)
            return true;

         reloadCurrentProjectJournal();
         return _journalSessionConnector.IsConnected;
      }

      public void ReloadJournal()
      {
         if (!_journalSessionConnector.IsConnected)
            return;

         reloadCurrentProjectJournal();
      }

      public void ShowJournal()
      {
         _eventPublisher.PublishEvent(new ShowJournalEvent());
      }

      private void reloadCurrentProjectJournal()
      {
         LoadJournal(_journalRetriever.JournalFullPath, showJournal: true);
      }

      public void SelectJournal()
      {
         connectTo(_journalSessionConnector.SelectJournal(), showJournal: true);
      }

      public void LoadJournal(string journalFilePath, bool showJournal)
      {
         connectTo(_journalSessionConnector.ConnectToJournal(journalFilePath), showJournal);
      }

      public void SaveJournalPage(JournalPage journalPage)
      {
         if (journalPage.IsTransient)
            createWorkingJournalItem(journalPage);
         else
            updateWorkingJournalItem(journalPage);
      }

      public void AddAsRelatedItemsToJournal(IReadOnlyList<IObjectBase> relatedItems)
      {
         if (!relatedItems.Any())
            return;

         if (!isConnectedToJournal())
            return;

         var currentJournalPage = JournalPageCurrentlyEdited ?? CreateJournalPage(showEditor: true);

         relatedItems.Each(x => _journalPageTask.AddRelatedItemTo(currentJournalPage, _relatedItemFactory.Create(x)));
      }

      public void DeleteJournalPage(JournalPage journalPage)
      {
         DeleteJournalPages(new[] {journalPage});
      }

      private void deleteJournalPage(JournalPage journalPage)
      {
         _databaseMediator.ExecuteCommand(new DeleteJournalPage {JournalPage = journalPage});
         journal.Remove(journalPage);
         _eventPublisher.PublishEvent(new JournalPageDeletedEvent(journalPage));
      }

      public void DeleteJournalPages(IReadOnlyList<JournalPage> journalPages)
      {
         if (!journalPages.Any())
            return;
         var viewResult = _dialogCreator.MessageBoxYesNo(promptForDelete(journalPages));
         if (viewResult != ViewResult.Yes) return;

         journalPages.Each(deleteJournalPage);
      }

      private string promptForDelete(IReadOnlyList<JournalPage> journalPages)
      {
         return journalPages.Count == 1 ? promptForSinglePageDelete(journalPages.First()) : Captions.Journal.ReallyDeleteMultipleJournalPages;
      }

      private string promptForSinglePageDelete(JournalPage journalPage)
      {
         return Captions.Journal.ReallyDeleteJournalPage(journalPage.Title);
      }

      public JournalPage JournalPageCurrentlyEdited => journal.Edited;

      private Core.Journal.Journal journal => _journalRetriever.Current;

      public void Edit(JournalPage journalPage, bool showEditor, JournalSearch journalSearch = null)
      {
         _eventPublisher.PublishEvent(new EditJournalPageStartedEvent(journalPage, showEditor));
         if (journalSearch == null) return;
         _eventPublisher.PublishEvent(new ShowJournalSearchEvent(journalSearch));
      }

      public IEnumerable<string> AllKnownTags => _databaseMediator.ExecuteQuery(new AllKnownTags());

      private void createWorkingJournalItem(JournalPage journalPage)
      {
         _databaseMediator.ExecuteCommand(new CreateJournalPage {JournalPage = journalPage});
         journal.AddJournalPage(journalPage);
         _eventPublisher.PublishEvent(new JournalPageAddedEvent(journalPage));
      }

      private void updateWorkingJournalItem(JournalPage journalPage)
      {
         _journalPageTask.UpdateJournalPage(journalPage);
      }

      private void connectTo(Core.Journal.Journal journalToConnectTo, bool showJournal)
      {
         if (journalToConnectTo == null)
            return;

         _journalRetriever.Current = journalToConnectTo;

         if (showJournal)
            ShowJournal();
      }

      public void SaveJournalDiagram(JournalDiagram journalDiagram)
      {
         _databaseMediator.ExecuteCommand(new UpdateJournalDiagram {Diagram = journalDiagram});
         _eventPublisher.PublishEvent(new JournalDiagramUpdatedEvent(journalDiagram));
      }

      public string CreateItemDescriptionFromContent(string documentContent)
      {
         var itemDescriptionFromContent = string.Concat(documentContent.Trim().Take(BYTE_COUNT_FOR_DESCRIPTION));
         var indexOfNewLine = itemDescriptionFromContent.IndexOf(Environment.NewLine, StringComparison.Ordinal);
         if (indexOfNewLine > 0)
            return itemDescriptionFromContent.Substring(0, indexOfNewLine);

         return itemDescriptionFromContent;
      }
   }
}