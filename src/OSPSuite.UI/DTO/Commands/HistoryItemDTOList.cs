using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraTreeList;
using OSPSuite.Presentation.DTO.Commands;

namespace OSPSuite.UI.DTO.Commands
{
   public class HistoryItemDTOList : TreeList.IVirtualTreeListData, IHistoryItemDTOList
   {
      private readonly IList<IHistoryItemDTO> _dtoList;

      public HistoryItemDTOList() : this(new List<IHistoryItemDTO>())
      {
      }

      public int Count => _dtoList.Count;

      public HistoryItemDTOList(IList<IHistoryItemDTO> dtoList)
      {
         _dtoList = dtoList;
      }

      public void Add(IHistoryItemDTO itemToAdd)
      {
         _dtoList.Add(itemToAdd);
      }

      public void AddAtFront(IHistoryItemDTO itemToAdd)
      {
         _dtoList.Insert(0,itemToAdd);
      }

      public IList<IHistoryItemDTO> All()
      {
         return _dtoList;
      }

      public void VirtualTreeGetChildNodes(VirtualTreeGetChildNodesInfo info)
      {
         info.Children = _dtoList.ToArray();
      }

      public void VirtualTreeGetCellValue(VirtualTreeGetCellValueInfo info)
      {
      }

      public void VirtualTreeSetCellValue(VirtualTreeSetCellValueInfo info)
      {
      }

      public IHistoryItemDTO ItemById(string id)
      {
         var historyItem = findHistoryItemById(_dtoList, id);
         return historyItem ?? new NullHistoryItemDTO();
      }

      private IHistoryItemDTO findHistoryItemById(IList<IHistoryItemDTO> historyItems, string historyItemId)
      {

         var historyItem = historyItems.FirstOrDefault(item => item.Id.Equals(historyItemId));
         if (historyItem != null) return historyItem;
         foreach (var history in historyItems)
         {
            var childHistory = findHistoryItemById(history.Children(), historyItemId);
            if (childHistory != null) return childHistory;
         }

         return null;
      }

      public void Remove(IHistoryItemDTO historyItemDto)
      {
         _dtoList.Remove(historyItemDto);
      }
   }
}