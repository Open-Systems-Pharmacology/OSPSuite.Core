using OSPSuite.Core.Commands.Core;
using OSPSuite.Infrastructure.ORM.History;
using OSPSuite.Utility;

namespace OSPSuite.Infrastructure.ORM.Mappers
{
   public interface IHistoryItemToHistoryItemMetaDataMapper : IMapper<IHistoryItem, HistoryItemMetaData>
   {
   }

   public class HistoryItemToHistoryItemMetaDataMapper : IHistoryItemToHistoryItemMetaDataMapper
   {
      private readonly ICommandToCommandMetaDataMapper _commandMetaDataMapper;

      public HistoryItemToHistoryItemMetaDataMapper(ICommandToCommandMetaDataMapper commandMetaDataMapper)
      {
         _commandMetaDataMapper = commandMetaDataMapper;
      }

      public HistoryItemMetaData MapFrom(IHistoryItem historyItem)
      {
         return new HistoryItemMetaData
         {
            Id = historyItem.Id,
            DateTime = historyItem.DateTime,
            User = historyItem.User,
            State = historyItem.State,
            Command = _commandMetaDataMapper.MapFrom(historyItem.Command)
         };
      }
   }
}
