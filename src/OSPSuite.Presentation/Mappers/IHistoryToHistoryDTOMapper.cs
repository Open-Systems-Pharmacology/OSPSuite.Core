using OSPSuite.Core.Commands.Core;
using OSPSuite.Presentation.DTO.Commands;
using OSPSuite.Utility;

namespace OSPSuite.Presentation.Mappers
{
   public interface IHistoryToHistoryDTOMapper : IMapper<IHistoryItem, IHistoryItemDTO>
   {
      bool EnableHistoryPruning { set; }
   }
}