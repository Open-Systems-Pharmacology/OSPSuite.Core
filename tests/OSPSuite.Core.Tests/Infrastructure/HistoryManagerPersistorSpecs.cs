using System;
using System.Collections.Generic;
using OSPSuite.BDDHelper;
using FakeItEasy;
using NHibernate;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Infrastructure.Serialization.ORM.History;
using OSPSuite.Infrastructure.Serialization.ORM.Mappers;
using OSPSuite.Infrastructure.Serialization.ORM.MetaData;

namespace OSPSuite.Infrastructure
{
   public abstract class concern_for_BaseHistoryManagerPersistor : ContextSpecification<HistoryManagerPersistor>
   {
      protected IHistoryItemMetaDataRepository _historyItemMetaDataRepository;
      protected IHistoryManagerFactory _historyManagerFactory;
      protected IHistoryItemMetaDataToHistoryItemMapper _historyItemMetaDataToHistoryItemMapper;
      protected IHistoryItemToHistoryItemMetaDataMapper _historyItemToHistoryItemMetaDataMapper;

      protected IHistoryManager _historyManager;
      protected IEnumerable<IHistoryItem> _history;
      protected HistoryItem _existingItem;
      protected HistoryItemMetaData _existingMetaDataItem;
      protected IEnumerable<HistoryItemMetaData> _existingHistoryItemMetaData;

      protected override void Context()
      {
         _historyItemMetaDataRepository = A.Fake<IHistoryItemMetaDataRepository>();
         _historyManagerFactory = A.Fake<IHistoryManagerFactory>();
         _historyItemMetaDataToHistoryItemMapper = A.Fake<IHistoryItemMetaDataToHistoryItemMapper>();
         _historyItemToHistoryItemMetaDataMapper = A.Fake<IHistoryItemToHistoryItemMetaDataMapper>();

         sut = new HistoryManagerPersistor(_historyItemToHistoryItemMetaDataMapper, _historyItemMetaDataToHistoryItemMapper, _historyManagerFactory, _historyItemMetaDataRepository);
      }

      protected void CreateExistingItemAndMetaDataItem()
      {
         _existingItem = new HistoryItem("user", new DateTime(), A.Fake<ICommand>()) {Id = "id"};
         _existingMetaDataItem = new HistoryItemMetaData {User = _existingItem.User, Id = "id", Command = new CommandMetaData {Comment = "there"}};
         _existingItem.Command.Comment = "hi";
         _history = new List<IHistoryItem>
         {
            _existingItem
         };

         _existingHistoryItemMetaData = new List<HistoryItemMetaData>
         {
            _existingMetaDataItem
         };

         _historyManager = A.Fake<IHistoryManager>();

         A.CallTo(() => _historyManager.History).Returns(_history);
         A.CallTo(() => _historyItemMetaDataRepository.All(A<ISession>._)).Returns(_existingHistoryItemMetaData);
         A.CallTo(() => _historyItemToHistoryItemMetaDataMapper.MapFrom(_existingItem)).Returns(_existingMetaDataItem);
      }
   }

   public class When_updating_a_comment_on_a_command_in_the_history : concern_for_BaseHistoryManagerPersistor
   {
      protected override void Context()
      {
         base.Context();
         CreateExistingItemAndMetaDataItem();
      }

      protected override void Because()
      {
         sut.Save(_historyManager, A.Fake<ISession>());
      }

      [Observation]
      public void the_command_should_have_been_saved()
      {
         A.CallTo(() => _historyItemMetaDataRepository.SaveCommand(_existingMetaDataItem, A<ISession>._)).MustHaveHappened();
      }
   }

   public class When_saving_new_history_the_new_item : concern_for_BaseHistoryManagerPersistor
   {
      protected override void Context()
      {
         base.Context();
         _existingItem = new HistoryItem("user", new DateTime(), A.Fake<ICommand>());
         _existingMetaDataItem = new HistoryItemMetaData {User = _existingItem.User};
         _history = new List<IHistoryItem>
         {
            _existingItem
         };
         _historyManager = A.Fake<IHistoryManager>();

         A.CallTo(() => _historyManager.History).Returns(_history);
         A.CallTo(() => _historyItemToHistoryItemMetaDataMapper.MapFrom(_existingItem)).Returns(_existingMetaDataItem);
      }

      protected override void Because()
      {
         sut.Save(_historyManager, A.Fake<ISession>());
      }

      [Observation]
      public void should_save_the_new_history_item()
      {
         A.CallTo(() => _historyItemMetaDataRepository.Save(_existingMetaDataItem, A<ISession>.Ignored)).MustHaveHappened();
      }
   }
}