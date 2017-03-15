using System.Collections.Generic;
using System.Linq;
using OSPSuite.Presentation.DTO.Commands;
using OSPSuite.Presentation.Mappers;
using OSPSuite.UI.DTO.Commands;

namespace OSPSuite.UI.Mappers
{
   public class HistoryItemDTOEnumerableToHistoryItemDTOList : IHistoryItemDTOEnumerableToHistoryItemDTOList
   {
      public IHistoryItemDTOList MapFrom(IEnumerable<IHistoryItemDTO> historyItemsDtos)
      {
         return new HistoryItemDTOList(historyItemsDtos.ToList());
      }
   }
}