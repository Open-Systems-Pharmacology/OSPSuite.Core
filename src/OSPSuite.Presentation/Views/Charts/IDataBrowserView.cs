using System.Collections.Generic;
using OSPSuite.Presentation.Presenters.Charts;

namespace OSPSuite.Presentation.Views.Charts
{
   public interface IDataBrowserView : IView<IDataBrowserPresenter>, IViewWithColumnSettings
   {
      void BindTo(IEnumerable<DataColumnDTO> dataColumnDTOs);

      /// <summary>
      /// Returns all selected <see cref="DataColumnDTO"/>
      /// </summary>
      IReadOnlyList<DataColumnDTO> SelectedColumns { get; }

      /// <summary>
      /// Returns all selected <see cref="DataColumnDTO"/> and their descendants.
      /// </summary>
      IReadOnlyList<DataColumnDTO> SelectedDescendantColumns { get; }
   }
}