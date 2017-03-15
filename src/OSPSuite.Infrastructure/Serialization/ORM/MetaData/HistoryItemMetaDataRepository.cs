using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Transform;
using OSPSuite.Infrastructure.Serialization.ORM.History;

namespace OSPSuite.Infrastructure.Serialization.ORM.MetaData
{
   public interface IHistoryItemMetaDataRepository
   {
      void Save(HistoryItemMetaData historyItemMetaData, ISession session);
      IEnumerable<HistoryItemMetaData> All(ISession session);
      void SaveCommand(HistoryItemMetaData historyItemMetaData, ISession session);
   }

   public class HistoryItemMetaDataRepository : IHistoryItemMetaDataRepository
   {
      private readonly ICommandMetaDataRepository _commandMetaDataRepository;

      public HistoryItemMetaDataRepository(ICommandMetaDataRepository commandMetaDataRepository)
      {
         _commandMetaDataRepository = commandMetaDataRepository;
      }

      public void Save(HistoryItemMetaData historyItemMetaData, ISession session)
      {
         var existingItem = session.Get<HistoryItemMetaData>(historyItemMetaData.Id);
         //already exists nothng to do to
         if (existingItem != null) return;

         session.Save(historyItemMetaData);
         SaveCommand(historyItemMetaData, session);
      }

      public IEnumerable<HistoryItemMetaData> All(ISession session)
      {
         _commandMetaDataRepository.LoadFromSession(session);
         //http://nhforge.org/wikis/howtonh/get-unique-results-from-joined-queries.aspx
         return session.CreateCriteria<HistoryItemMetaData>()
            .SetResultTransformer(Transformers.DistinctRootEntity).List<HistoryItemMetaData>().OrderBy(x => x.Sequence);
      }

      public void SaveCommand(HistoryItemMetaData historyItemMetaData, ISession session)
      {
         _commandMetaDataRepository.Save(historyItemMetaData.Command, session);
      }
   }
}