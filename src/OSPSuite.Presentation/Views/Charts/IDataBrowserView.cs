using System.Collections.Generic;
using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Presenters.Charts;

namespace OSPSuite.Presentation.Views.Charts
{
   public interface IDataBrowserView : IView<IDataBrowserPresenter>, IViewWithColumnSettings
   {
      void BindTo(IEnumerable<DataColumnDTO> dataColumnDTOs);

      /// <summary>
      ///    Returns all selected <see cref="DataColumnDTO" />.
      ///    All DTOs from a group are returned when the group is selected
      /// </summary>
      IReadOnlyList<DataColumnDTO> SelectedColumns { get; }

      /// <summary>
      ///    sets the group row format of the gridView to the specified string.
      /// </summary>
      void SetGroupRowFormat(GridGroupRowFormats format);
   }
}