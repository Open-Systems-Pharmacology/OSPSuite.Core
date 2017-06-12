using System.Linq;
using DevExpress.Utils.Menu;
using DevExpress.XtraGrid.Menu;
using OSPSuite.Assets;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views.Charts
{
   internal class DataBrowserPopupMenu : GridViewMenu
   {
      private readonly IDataBrowserPresenter _presenter;
      private readonly int _groupRowHandle;
      private readonly GridViewBinder<DataColumnDTO> _gridViewBinder;

      public DataBrowserPopupMenu(GridViewBinder<DataColumnDTO> gridViewBinder, IDataBrowserPresenter presenter, int groupRowHandle)
         : base(gridViewBinder.GridView)
      {
         _presenter = presenter;
         _groupRowHandle = groupRowHandle;
         _gridViewBinder = gridViewBinder;
         Items.Add(new DXMenuItem("Select all", (o, e) => updateSelection(selected: true), ApplicationIcons.CheckAll));
         Items.Add(new DXMenuItem("Deselect all", (o, e) => updateSelection(selected: false), ApplicationIcons.UncheckAll));
      }

      private void updateSelection(bool selected)
      {
         var selectedItems = _gridViewBinder.SelectedItems(_groupRowHandle);
         _presenter.SetUsedState(selectedItems.ToList(), selected);
      }
   }
}