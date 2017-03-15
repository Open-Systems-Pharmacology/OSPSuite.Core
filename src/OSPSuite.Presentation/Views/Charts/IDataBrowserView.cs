using System.Collections.Generic;
using System.Windows.Forms;
using OSPSuite.Presentation.Presenters.Charts;

namespace OSPSuite.Presentation.Views.Charts
{
   public interface IDataBrowserView : IView<IDataBrowserPresenter>, IViewWithColumnSettings
   {
      void SetDataSource(DataColumnsDTO dataColumnsDTO);
      IReadOnlyList<string> SelectedDataColumnIds { get; }
      event DragEventHandler DragOver;
      event DragEventHandler DragDrop;
      IReadOnlyList<string> SelectedDescendentDataRepositoryColumnIds { get; }
   }
}