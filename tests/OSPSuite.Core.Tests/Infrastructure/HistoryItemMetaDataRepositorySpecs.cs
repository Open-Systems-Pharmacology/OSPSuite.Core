using OSPSuite.BDDHelper;
using FakeItEasy;
using NHibernate;
using OSPSuite.Infrastructure.Serialization.ORM.History;
using OSPSuite.Infrastructure.Serialization.ORM.MetaData;

namespace OSPSuite.Infrastructure
{
   public abstract class concern_for_HistoryItemMetaDataRepository : ContextSpecification<HistoryItemMetaDataRepository>
   {
      protected ICommandMetaDataRepository _commandMetaDataRepository;

      protected override void Context()
      {
         _commandMetaDataRepository = A.Fake<ICommandMetaDataRepository>();
         sut = new HistoryItemMetaDataRepository(_commandMetaDataRepository);
      }
   }

   public class When_retrieving_all_history_items_from_the_session : concern_for_HistoryItemMetaDataRepository
   {
      private ISession _session;

      protected override void Context()
      {
         base.Context();
         _session = A.Fake<ISession>();
      }

      protected override void Because()
      {
         sut.All(_session);
      }

      [Observation]
      public void should_fill_the_command_repository_before_the_history_items_are_retrieved()
      {
         A.CallTo(() => _commandMetaDataRepository.LoadFromSession(_session)).MustHaveHappened();
         A.CallTo(() => _session.CreateCriteria<HistoryItemMetaData>()).MustHaveHappened();
      }
   }

   public class When_saving_a_command_only : concern_for_HistoryItemMetaDataRepository
   {
      private ISession _session;
      private HistoryItemMetaData _historyItemMetaData;

      protected override void Context()
      {
         base.Context();
         _session = A.Fake<ISession>();
         _historyItemMetaData = new HistoryItemMetaData {Command = new CommandMetaData()};
      }

      protected override void Because()
      {
         sut.SaveCommand(_historyItemMetaData, _session);
      }

      [Observation]
      public void should_not_save_the_history_item()
      {
         A.CallTo(() => _session.Save(_historyItemMetaData)).MustNotHaveHappened();
      }

      [Observation]
      public void should_save_the_command()
      {
         A.CallTo(() => _commandMetaDataRepository.Save(_historyItemMetaData.Command, _session)).MustHaveHappened();
      }
   }

   public class When_saving_a_history_item_that_does_not_already_exists : concern_for_HistoryItemMetaDataRepository
   {
      private ISession _session;
      private HistoryItemMetaData _historyItemMetaData;

      protected override void Context()
      {
         base.Context();
         _session = A.Fake<ISession>();

         _historyItemMetaData = new HistoryItemMetaData {Id = "historyItemId"};

         A.CallTo(() => _session.Get<HistoryItemMetaData>("historyItemId")).Returns(null);
      }

      protected override void Because()
      {
         sut.Save(_historyItemMetaData, _session);
      }

      [Observation]
      public void should_save_the_history_item()
      {
         A.CallTo(() => _session.Save(A<HistoryItemMetaData>._)).MustHaveHappened();
      }

      [Observation]
      public void should_save_the_command()
      {
         A.CallTo(() => _commandMetaDataRepository.Save(A<CommandMetaData>._, _session)).MustHaveHappened();
      }
   }

   public class When_saving_a_history_item_that_already_exists : concern_for_HistoryItemMetaDataRepository
   {
      private ISession _session;
      private HistoryItemMetaData _historyItemMetaData;

      protected override void Context()
      {
         base.Context();
         _session = A.Fake<ISession>();

         _historyItemMetaData = new HistoryItemMetaData {Id = "historyItemId"};

         A.CallTo(() => _session.Get<HistoryItemMetaData>("historyItemId")).Returns(new HistoryItemMetaData {Id = "historyItemId"});
      }

      protected override void Because()
      {
         sut.Save(_historyItemMetaData, _session);
      }

      [Observation]
      public void should_not_save_the_history_item()
      {
         A.CallTo(() => _session.Save(A<HistoryItemMetaData>._)).MustNotHaveHappened();
      }

      [Observation]
      public void should_not_save_the_command()
      {
         A.CallTo(() => _commandMetaDataRepository.Save(A<CommandMetaData>._, _session)).MustNotHaveHappened();
      }
   }
}
