using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.UICommands;
using OSPSuite.Utility.Events;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_RenameSimulationAnalysisUICommand : ContextSpecification<RenameSimulationAnalysisUICommand>
   {
      protected IOSPSuiteExecutionContext _executionContext;
      protected IEventPublisher _eventPublisher;
      protected IApplicationController _applicationController;
      protected IRenameObjectPresenter _renameObjectPresenter;
      protected ISimulationAnalysis _analysis;
      protected ISimulationAnalysis _siblingAnalysis;
      protected IAnalysable _analysable;

      protected override void Context()
      {
         _executionContext = A.Fake<IOSPSuiteExecutionContext>();
         _eventPublisher = A.Fake<IEventPublisher>();
         _applicationController = A.Fake<IApplicationController>();
         _renameObjectPresenter = A.Fake<IRenameObjectPresenter>();
         A.CallTo(() => _applicationController.Start<IRenameObjectPresenter>()).Returns(_renameObjectPresenter);

         _analysis = A.Fake<ISimulationAnalysis>();
         _analysis.Name = "Time Profile";
         _siblingAnalysis = A.Fake<ISimulationAnalysis>();
         _siblingAnalysis.Name = "Sibling Analysis";
         _analysable = A.Fake<IAnalysable>();
         A.CallTo(() => _analysable.Analyses).Returns(new[] {_analysis, _siblingAnalysis});
         A.CallTo(() => _analysis.Analysable).Returns(_analysable);

         sut = new RenameSimulationAnalysisUICommand(_executionContext, _eventPublisher, _applicationController);
         sut.For(_analysis);
      }
   }

   public class When_renaming_a_simulation_analysis : concern_for_RenameSimulationAnalysisUICommand
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _renameObjectPresenter.NewNameFrom(_analysis, A<IEnumerable<string>>._, null)).Returns("New Name");
      }

      protected override void Because()
      {
         sut.Execute();
      }

      [Observation]
      public void should_forbid_the_names_of_all_analyses_of_the_analysable()
      {
         A.CallTo(() => _renameObjectPresenter.NewNameFrom(_analysis, A<IEnumerable<string>>.That.Matches(x => x.Contains(_siblingAnalysis.Name)), null)).MustHaveHappened();
      }

      [Observation]
      public void should_rename_the_analysis()
      {
         _analysis.Name.ShouldBeEqualTo("New Name");
      }

      [Observation]
      public void should_publish_a_renamed_event_for_the_analysis()
      {
         A.CallTo(() => _eventPublisher.PublishEvent(A<RenamedEvent>.That.Matches(x => Equals(x.RenamedObject, _analysis)))).MustHaveHappened();
      }

      [Observation]
      public void should_notify_a_project_change()
      {
         A.CallTo(() => _executionContext.ProjectChanged()).MustHaveHappened();
      }
   }

   public class When_the_rename_of_a_simulation_analysis_is_canceled : concern_for_RenameSimulationAnalysisUICommand
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _renameObjectPresenter.NewNameFrom(_analysis, A<IEnumerable<string>>._, null)).Returns(string.Empty);
      }

      protected override void Because()
      {
         sut.Execute();
      }

      [Observation]
      public void should_not_rename_the_analysis()
      {
         _analysis.Name.ShouldBeEqualTo("Time Profile");
      }

      [Observation]
      public void should_not_publish_any_renamed_event()
      {
         A.CallTo(() => _eventPublisher.PublishEvent(A<RenamedEvent>._)).MustNotHaveHappened();
      }
   }

   public class When_renaming_a_simulation_analysis_that_does_not_belong_to_an_analysable : concern_for_RenameSimulationAnalysisUICommand
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _analysis.Analysable).Returns(null);
         A.CallTo(() => _renameObjectPresenter.NewNameFrom(_analysis, A<IEnumerable<string>>._, null)).Returns("New Name");
      }

      protected override void Because()
      {
         sut.Execute();
      }

      [Observation]
      public void should_not_forbid_any_name()
      {
         A.CallTo(() => _renameObjectPresenter.NewNameFrom(_analysis, A<IEnumerable<string>>.That.Matches(x => !x.Any()), null)).MustHaveHappened();
      }
   }
}
