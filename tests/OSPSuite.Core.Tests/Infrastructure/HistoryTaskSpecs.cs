using System;
using System.Data.Common;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Events;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Serialization.ORM.MetaData;
using OSPSuite.Infrastructure.Services;
using OSPSuite.Utility.Events;

namespace OSPSuite.Infrastructure
{
   public abstract class concern_for_HistoryTask : ContextSpecification<IHistoryTask>
   {
      protected IProjectRetriever _projectRetriever;
      protected IHistoryManagerRetriever _historyManagerRetriever;
      protected IDialogCreator _dialogCreator;
      protected SQLiteProjectCommandExecuter _commandExecuter;
      protected IHistoryManager _historyManager;
      protected IEventPublisher _eventPublisher;
      protected ICommandMetaDataRepository _commandMetaDataRepository;

      protected override void Context()
      {
         _projectRetriever = A.Fake<IProjectRetriever>();
         _historyManagerRetriever = A.Fake<IHistoryManagerRetriever>();
         _dialogCreator = A.Fake<IDialogCreator>();
         _commandExecuter = A.Fake<SQLiteProjectCommandExecuter>();
         _eventPublisher = A.Fake<IEventPublisher>();
         _historyManager = A.Fake<IHistoryManager>();
         _commandMetaDataRepository= A.Fake<ICommandMetaDataRepository>();
         sut = new HistoryTask(_projectRetriever, _historyManagerRetriever, _dialogCreator, _commandExecuter, _eventPublisher, _commandMetaDataRepository);
      }
   }

   public class When_executing_the_clear_history_command_and_the_user_cancels_the_action : concern_for_HistoryTask
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(_dialogCreator).WithReturnType<ViewResult>().Returns(ViewResult.No);
      }

      protected override void Because()
      {
         sut.ClearHistory();
      }

      [Observation]
      public void should_not_clear_the_history()
      {
         A.CallTo(_commandExecuter).MustNotHaveHappened();
      }
   }

   public class When_executing_the_clear_history_command_and_the_user_validates_the_action : concern_for_HistoryTask
   {
      private readonly string _fullPath = "FULL PATH";

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _projectRetriever.ProjectFullPath).Returns(_fullPath);

         A.CallTo(_dialogCreator).WithReturnType<ViewResult>().Returns(ViewResult.Yes);
         A.CallTo(() => _historyManagerRetriever.Current).Returns(_historyManager);
      }

      protected override void Because()
      {
         sut.ClearHistory();
      }

      [Observation]
      public void should_clear_the_project_history()
      {
         A.CallTo(() => _commandExecuter.ExecuteCommand(_fullPath, A<Action<DbConnection>>._)).MustHaveHappened();
      }

      [Observation]
      public void should_clear_the_current_history()
      {
         A.CallTo(() => _historyManager.Clear()).MustHaveHappened();
      }

      [Observation]
      public void should_clear_the_command_meta_data_repository()
      {
         A.CallTo(() => _commandMetaDataRepository.Clear()).MustHaveHappened();
      }

      [Observation]
      public void should_publish_the_history_cleared_event()
      {
         A.CallTo(() => _eventPublisher.PublishEvent(A<HistoryClearedEvent>._)).MustHaveHappened();
      }
   }

   public class When_executing_the_clear_history_command_and_the_current_project_was_not_saved_to_file : concern_for_HistoryTask
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _projectRetriever.ProjectFullPath).Returns(null);

         A.CallTo(_dialogCreator).WithReturnType<ViewResult>().Returns(ViewResult.Yes);
         A.CallTo(() => _historyManagerRetriever.Current).Returns(_historyManager);
      }

      protected override void Because()
      {
         sut.ClearHistory();
      }

      [Observation]
      public void should_not_clear_the_project_history()
      {
         A.CallTo(_commandExecuter).MustNotHaveHappened();
      }

      [Observation]
      public void should_clear_the_current_history()
      {
         A.CallTo(() => _historyManager.Clear()).MustHaveHappened();
      }
   }

   public class When_executing_the_clear_history_command_and_no_acitve_history_maanger_could_be_retrieved : concern_for_HistoryTask
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _projectRetriever.ProjectFullPath).Returns(null);

         A.CallTo(_dialogCreator).WithReturnType<ViewResult>().Returns(ViewResult.Yes);
         A.CallTo(() => _historyManagerRetriever.Current).Returns(null);
      }

      protected override void Because()
      {
         sut.ClearHistory();
      }

      [Observation]
      public void should_not_clear_the_project_history()
      {
         A.CallTo(_commandExecuter).MustNotHaveHappened();
      }

      [Observation]
      public void should_not_publish_the_history_cleared_event()
      {
         A.CallTo(() => _eventPublisher.PublishEvent(A<HistoryClearedEvent>._)).MustNotHaveHappened();
      }
   }
}