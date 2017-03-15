using OSPSuite.Presentation.DTO.Commands;
using OSPSuite.Presentation.Presenters.Commands;

namespace OSPSuite.Presentation.Views.Commands
{
   public interface IHistoryBrowserView
   {
      void AttachPresenter(IHistoryBrowserPresenter presenter);
      void BindTo(IHistoryItemDTOList historyItems);
      void Unbind();
      void AddColumn(ColumnProperties columnToAdd);
      void RefreshView();
      void Clear();
      bool EnableFiltering { set; }
      bool EnableAutoFilterRow { set; }
      void UpdateColumnPosition();
      void BestFitColumns();
   }
}