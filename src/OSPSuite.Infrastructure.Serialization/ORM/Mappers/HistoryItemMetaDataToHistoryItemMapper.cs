using OSPSuite.Core.Commands.Core;
using OSPSuite.Infrastructure.ORM.History;
using OSPSuite.Utility;

namespace OSPSuite.Infrastructure.ORM.Mappers
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
