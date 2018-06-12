using System.Collections.Generic;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Core.Journal;
using OSPSuite.Core.Services;

namespace OSPSuite.Presentation.UICommands
{
   public class AddParameterAnalysableToActiveJournalPageUICommand : ActiveObjectUICommand<IParameterAnalysable>
   {
      private readonly IJournalTask _journalTask;
      private readonly IObservedDataRepository _observedDataRepository;
      private readonly IOSPSuiteExecutionContext _executionContext;

      public AddParameterAnalysableToActiveJournalPageUICommand(
         IJournalTask journalTask,
         IObservedDataRepository observedDataRepository,
         IOSPSuiteExecutionContext executionContext,
         IActiveSubjectRetriever activeSubjectRetriever
         ) : base(activeSubjectRetriever)
      {
         _journalTask = journalTask;
         _observedDataRepository = observedDataRepository;
         _executionContext = executionContext;
      }

      protected override void PerformExecute()
      {
         var items = new List<IObjectBase>();

         //Load the subject first to ensure that we can retrieve the observed data used
         _executionContext.Load(Subject);

         //Add first observed data if available
         if (Subject is IUsesObservedData usesObservedData)
            items.AddRange(_observedDataRepository.AllObservedDataUsedBy(usesObservedData));

         //Then simulations
         items.AddRange(Subject.AllSimulations);

         //last subect
         items.Add(Subject);

         _journalTask.AddAsRelatedItemsToJournal(items);
      }
   }
}