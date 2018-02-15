using System.Collections.Generic;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Core.Journal;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.UICommands
{
   public class AddParameterAnalysableToActiveJournalPageUICommand : ActiveObjectUICommand<IParameterAnalysable>
   {
      private readonly IJournalTask _journalTask;
      private readonly IObservedDataRepository _observedDataRepository;
      private readonly IOSPSuiteExecutionContext _executionContext;

      public AddParameterAnalysableToActiveJournalPageUICommand(
         IJournalTask journalTask,
         IActiveSubjectRetriever activeSubjectRetriever,
         IObservedDataRepository observedDataRepository,
         IOSPSuiteExecutionContext executionContext) : base(activeSubjectRetriever)
      {
         _journalTask = journalTask;
         _observedDataRepository = observedDataRepository;
         _executionContext = executionContext;
      }

      protected override void PerformExecute()
      {
         var items = new List<IObjectBase>();

         //Load all used simulation
         Subject.AllSimulations.Each(_executionContext.Load);

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