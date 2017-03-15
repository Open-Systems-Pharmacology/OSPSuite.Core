using System.Collections.Generic;
using OSPSuite.Presentation.DTO.Commands;
using OSPSuite.Utility;

namespace OSPSuite.Presentation.Mappers
{
   public interface IHistoryItemDTOEnumerableToHistoryItemDTOList : IMapper<IEnumerable<IHistoryItemDTO>, IHistoryItemDTOList>
   {
      
   }
}