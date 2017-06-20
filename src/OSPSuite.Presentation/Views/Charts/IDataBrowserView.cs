using System.Collections.Generic;
using System.Windows.Forms;
using OSPSuite.Presentation.Presenters.Charts;

namespace OSPSuite.Presentation.Views.Charts
{
   public interface IDataBrowserView : IView<IDataBrowserPresenter>, IViewWithColumnSettings
   {
      void BindTo(IEnumerable<DataColumnDTO> dataColumnDTOs);
      IReadOnlyList<DataColumnDTO> SelectedDataColumns { get; }
      event DragEventHandler DragOver;
      event DragEventHandler DragDrop;
      IReadOnlyList<DataColumnDTO> SelectedDescendentDataRepositoryColumns { get; }
   }
}