using OSPSuite.Utility;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Infrastructure.Serialization.ORM.History;

namespace OSPSuite.Infrastructure.Serialization.ORM.Mappers
{

   public interface IHistoryItemMetaDataToHistoryItemMapper : IMapper<HistoryItemMetaData, IHistoryItem>
   {
   }

   public class HistoryItemMetaDataToHistoryItemMapper : IHistoryItemMetaDataToHistoryItemMapper
   {
      private readonly ICommandMetaDataToCommandMapper _commandMapper;

      public HistoryItemMetaDataToHistoryItemMapper(ICommandMetaDataToCommandMapper commandMapper)
      {
         _commandMapper = commandMapper;
      }

      public IHistoryItem MapFrom(HistoryItemMetaData historyItemMetaData)
      {
         IHistoryItem historyItem = new HistoryItem(
            historyItemMetaData.User,
            historyItemMetaData.DateTime,
            _commandMapper.MapFrom(historyItemMetaData.Command));

         historyItem.State = historyItemMetaData.State;
         historyItem.Id = historyItemMetaData.Id;
         return historyItem;
      }
   }
}
