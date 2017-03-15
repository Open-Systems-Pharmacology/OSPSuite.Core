using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Journal;
using OSPSuite.Core.Services;

namespace OSPSuite.Presentation.UICommands
{
   public class AddRelatedItemsToActiveJournalPageUICommand : ActiveObjectUICommand<IReadOnlyList<IObjectBase>>
   {
      private readonly IJournalTask _journalTask;

      public AddRelatedItemsToActiveJournalPageUICommand(IJournalTask journalTask, IActiveSubjectRetriever activeSubjectRetriever)
         : base(activeSubjectRetriever)
      {
         _journalTask = journalTask;
      }

      protected override void PerformExecute()
      {
         _journalTask.AddAsRelatedItemsToJournal(Subject);
      }
   }
}