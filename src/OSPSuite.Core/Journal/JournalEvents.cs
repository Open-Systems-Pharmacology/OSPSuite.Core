using System.Collections.Generic;

namespace OSPSuite.Core.Journal
{
   public abstract class JournalPageEvent
   {
      public JournalPage JournalPage { get; private set; }

      protected JournalPageEvent(JournalPage journalPage)
      {
         JournalPage = journalPage;
      }
   }

   public class JournalPageAddedEvent : JournalPageEvent
   {
      public JournalPageAddedEvent(JournalPage journalPage) : base(journalPage)
      {
      }
   }

   public class JournalPageDeletedEvent : JournalPageEvent
   {
      public JournalPageDeletedEvent(JournalPage journalPage) : base(journalPage)
      {
      }
   }

   public class EditJournalPageStartedEvent : JournalPageEvent
   {
      public bool ShowEditor { get; }

      public EditJournalPageStartedEvent(JournalPage journalPage, bool showEditor) : base(journalPage)
      {
         ShowEditor = showEditor;
      }
   }

   public class JournalPageUpdatedEvent : JournalPageEvent
   {
      public JournalPageUpdatedEvent(JournalPage journalPage)
         : base(journalPage)
      {
      }
   }

   public class JournalLoadedEvent
   {
      public Journal Journal { get; private set; }

      public JournalLoadedEvent(Journal journal)
      {
         Journal = journal;
      }
   }

   public class JournalDiagramUpdatedEvent
   {
      public JournalDiagram Diagram { get; private set; }

      public JournalDiagramUpdatedEvent(JournalDiagram diagram)
      {
         Diagram = diagram;
      }
   }

   public class JournalClosedEvent
   {
   }

   public class JournalSearchPerformedEvent
   {
      public IEnumerable<JournalSearchItem> SearchResults { get; private set; }
      public JournalSearch JournalSearch { get; private set; }

      public JournalSearchPerformedEvent(IEnumerable<JournalSearchItem> searchResults, JournalSearch journalSearch)
      {
         SearchResults = searchResults;
         JournalSearch = journalSearch;
      }
   }

   public class JournalClearSearchEvent
   {
   }

   public class JournalCloseSearchEvent
   {
   }

   public class ShowJournalSearchEvent
   {
      public JournalSearch JournalSearch { get; private set; }

      public ShowJournalSearchEvent(JournalSearch journalSearch)
      {
         JournalSearch = journalSearch;
      }
   }

   public class ShowJournalEvent
   {
   }
}