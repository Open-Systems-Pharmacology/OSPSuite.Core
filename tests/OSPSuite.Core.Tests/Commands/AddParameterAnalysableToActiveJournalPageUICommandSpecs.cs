using System.Collections.Generic;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Core.Journal;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.UICommands;

namespace OSPSuite.Commands
{
   public abstract class concern_for_AddParameterAnalysableToActiveJournalPageUICommand : ContextSpecification<AddParameterAnalysableToActiveJournalPageUICommand>
   {
      protected IJournalTask _journalTask;
      protected IActiveSubjectRetriever _activeSubjectRetriever;
      protected IObservedDataRepository _observedDataRepository;
      protected IOSPSuiteExecutionContext _executionContext;
      protected ParameterIdentification _parameterIdentification;
      protected ISimulation _simulation1;
      protected ISimulation _simulation2;
      protected DataRepository _obs1;
      protected DataRepository _obs2;

      protected override void Context()
      {
         _journalTask = A.Fake<IJournalTask>();
         _activeSubjectRetriever = A.Fake<IActiveSubjectRetriever>();
         _observedDataRepository = A.Fake<IObservedDataRepository>();
         _executionContext = A.Fake<IOSPSuiteExecutionContext>();

         sut = new AddParameterAnalysableToActiveJournalPageUICommand(_journalTask, _observedDataRepository, _executionContext, _activeSubjectRetriever);

         _parameterIdentification = new ParameterIdentification();

         sut.Subject = _parameterIdentification;

         _simulation1 = A.Fake<ISimulation>();
         _simulation2 = A.Fake<ISimulation>();
         _parameterIdentification.AddSimulation(_simulation1);
         _parameterIdentification.AddSimulation(_simulation2);

         _obs1 = new DataRepository("OBS1");
         _obs2 = new DataRepository("OBS2");
         A.CallTo(() => _observedDataRepository.AllObservedDataUsedBy(_parameterIdentification)).Returns(new[] {_obs1, _obs2});
      }
   }

   public class When_executing_the_add_parmaeter_analysable_to_journal_commmand : concern_for_AddParameterAnalysableToActiveJournalPageUICommand
   {
      private IReadOnlyList<IObjectBase> _allRelatedItems;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _journalTask.AddAsRelatedItemsToJournal(A<IReadOnlyList<IObjectBase>>._))
            .Invokes(x => _allRelatedItems = x.GetArgument<IReadOnlyList<IObjectBase>>(0));
      }

      protected override void Because()
      {
         sut.Execute();
      }

      [Observation]
      public void shoould_load_the_parameter_analyzable()
      {
         A.CallTo(() => _executionContext.Load(_parameterIdentification)).MustHaveHappened();
      }

      [Observation]
      public void should_add_first_all_observed_data_used_by_the_analysable_if_any_as_related_item()
      {
         _allRelatedItems[0].ShouldBeEqualTo(_obs1);
         _allRelatedItems[1].ShouldBeEqualTo(_obs2);
      }

      [Observation]
      public void should_then_add_the_simulation_used_by_the_parameter_analysable_as_related_item()
      {
         _allRelatedItems[2].ShouldBeEqualTo(_simulation1);
         _allRelatedItems[3].ShouldBeEqualTo(_simulation2);
      }

      [Observation]
      public void should_add_last_the_parameter_analysable_as_related_item()
      {
         _allRelatedItems[4].ShouldBeEqualTo(_parameterIdentification);
      }
   }
}