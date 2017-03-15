namespace OSPSuite.Presentation.DTO.Commands
{
   public interface IHistoryItemDTOList
   {
      IHistoryItemDTO ItemById(string historyItemId);
      void AddAtFront(IHistoryItemDTO historyItemDTO);
   }
}