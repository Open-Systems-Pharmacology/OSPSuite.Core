using System.Collections.Generic;
using System.Linq;
using NHibernate;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Infrastructure.ORM.Mappers;
using OSPSuite.Infrastructure.ORM.MetaData;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Infrastructure.ORM.History
{
   public interface IHistoryManagerPersistor : ISessionPersistor<IHistoryManager>
   {
   }

   public class HistoryManagerPersistor : IHistoryManagerPersistor
   {
      private readonly IHistoryItemToHistoryItemMetaDataMapper _historyItemMetaDataMapper;
      private readonly IHistoryItemMetaDataToHistoryItemMapper _historyItemMapper;
      private readonly IHistoryManagerFactory _historyManagerFactory;
      private readonly IHistoryItemMetaDataRepository _historyItemMetaDataRepository;

      public HistoryManagerPersistor(IHistoryItemToHistoryItemMetaDataMapper historyItemMetaDataMapper, IHistoryItemMetaDataToHistoryItemMapper historyItemMapper, IHistoryManagerFactory historyManagerFactory, IHistoryItemMetaDataRepository historyItemMetaDataRepository)
      {
         _historyItemMetaDataMapper = historyItemMetaDataMapper;
         _historyItemMapper = historyItemMapper;
         _historyManagerFactory = historyManagerFactory;
         _historyItemMetaDataRepository = historyItemMetaDataRepository;
      }

      public void Save(IHistoryManager historyManager, ISession session)
      {
         //only save history that were not saved yet
         var savedHistory = new Cache<string, HistoryItemMetaData>(x => x.Id, x => null);
         savedHistory.AddRange(allHistoryItemMetaData(session));
         int sequence = 0;

         foreach (var historyItem in historyManager.History)
         {
            var savedHistoryItem = savedHistory[historyItem.Id];
            if (savedHistoryItem == null)
            {
               saveNewHistoryItem(historyItem, sequence, session);
            }
            else if (commentHasChanged(savedHistoryItem, historyItem))
            {
               updateComment(savedHistoryItem, historyItem, session);
            }

            sequence++;
         }
      }

      public IHistoryManager Load(ISession session)
      {
         var historyManager = _historyManagerFactory.Create();
         allHistoryItemMetaData(session).Each(x => historyManager.AddToHistory(_historyItemMapper.MapFrom(x)));

         if (historyManager.History != null)
         {
            //last command should alwyas be loaded to enable undo
            var lastItem = historyManager.History.LastOrDefault();
            if (lastItem != null)
               lastItem.Command.Loaded = true;
         }

         return historyManager;
      }

      private bool commentHasChanged(HistoryItemMetaData savedHistoryItem, IHistoryItem historyItem)
      {
         return !string.Equals(savedHistoryItem.Command.Comment, historyItem.Command.Comment);
      }

      private IEnumerable<HistoryItemMetaData> allHistoryItemMetaData(ISession session)
      {
         return _historyItemMetaDataRepository.All(session);
      }

      private void updateComment(HistoryItemMetaData savedHistoryItemMetaData, IHistoryItem historyItem, ISession session)
      {
         savedHistoryItemMetaData.Command.Comment = historyItem.Command.Comment;
         _historyItemMetaDataRepository.SaveCommand(savedHistoryItemMetaData, session);
      }

      private void saveNewHistoryItem(IHistoryItem historyItem, int sequence, ISession session)
      {
         var metaData = _historyItemMetaDataMapper.MapFrom(historyItem);
         metaData.Sequence = sequence;
         _historyItemMetaDataRepository.Save(metaData, session);
      }
   }
}