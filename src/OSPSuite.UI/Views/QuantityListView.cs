using System.Collections.Generic;
using System.Linq;
using DevExpress.Utils.Menu;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Menu;
using DevExpress.XtraGrid.Views.Grid;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Binders;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.RepositoryItems;
using OSPSuite.Utility.Collections;

namespace OSPSuite.UI.Views
{
   public partial class QuantityListView : BaseUserControl, IQuantityListView
   {
      private readonly PathElementsBinder<QuantitySelectionDTO> _pathElementsBinder;
      private readonly GridViewBinder<QuantitySelectionDTO> _gridViewBinder;
      private readonly ICache<QuantityColumn, IGridViewColumn> _otherColumns;
      private readonly RepositoryItem _selectionRepository;
      private IGridViewColumn _colSequence;
      private IQuantityListPresenter _presenter;
      private PathElement _groupPathElement;
      private PathElement _sortedPathElement;
      private bool _groupByDefined;
      private bool _sortByDefined;

      public QuantityListView(PathElementsBinder<QuantitySelectionDTO> pathElementsBinder)
      {
         InitializeComponent();
         gridView.GroupFormat = "[#image]{1}";
         _gridViewBinder = new GridViewBinder<QuantitySelectionDTO>(gridView);
         _pathElementsBinder = pathElementsBinder;
         gridView.ShouldUseColorForDisabledCell = false;
         gridView.OptionsSelection.EnableAppearanceFocusedRow = true;
         gridView.ShowColumnChooser = true;
         gridView.EndGrouping += (o, e) => OnEvent(gridViewEndGrouping);
         gridView.DoubleClick += (o, e) => OnEvent(gridViewDoubleClicked);

         //default not defined
         _groupByDefined = false;
         _sortByDefined = false;
         _otherColumns = new Cache<QuantityColumn, IGridViewColumn>();
         _selectionRepository = new UxRepositoryItemCheckEdit(gridView);
      }

      private void gridViewEndGrouping()
      {
         if (_presenter.ExpandAllGroups)
            gridView.ExpandAllGroups();
      }

      private void gridViewDoubleClicked()
      {
         var pt = gridView.GridControl.PointToClient(MousePosition);
         _presenter.QuantityDTODoubleClicked(_gridViewBinder.ElementAt(pt));
      }

      public void AttachPresenter(IQuantityListPresenter presenter)
      {
         _presenter = presenter;
      }

      public override void InitializeBinding()
      {
         _pathElementsBinder.InitializeBinding(_gridViewBinder);

         var colDimension = _gridViewBinder.AutoBind(x => x.DimensionDisplayName)
            .WithCaption(Captions.Dimension)
            .AsReadOnly();

         _otherColumns.Add(QuantityColumn.Dimension, colDimension);

         var colQuantityType = _gridViewBinder.AutoBind(x => x.QuantityType)
            .AsReadOnly();

         _otherColumns.Add(QuantityColumn.QuantityType, colQuantityType);

         var colSelection = _gridViewBinder.AutoBind(x => x.Selected)
            .WithRepository(x => _selectionRepository)
            .WithOnChanged(dto => OnEvent(() => _presenter.UpdateSelection(dto)));

         _otherColumns.Add(QuantityColumn.Selection, colSelection);

         _colSequence = _gridViewBinder.Bind(x => x.Sequence).AsHidden();
         gridView.PopupMenuShowing += (o, e) => OnEvent(showPopupMenu, e);
      }

      public void BindTo(IEnumerable<QuantitySelectionDTO> quantitySelectionDTOs)
      {
         groupByColumn();
         sortByColumn();
         _gridViewBinder.BindToSource(quantitySelectionDTOs);
      }

      public void SetCaption(PathElement pathElement, string caption)
      {
         _pathElementsBinder.SetCaption(pathElement, caption);
      }

      private void sortByColumn()
      {
         if (!_sortByDefined) return;
         xtraColumnBy(SortedPathElement).FieldNameSortGroup = _colSequence.XtraColumn.FieldName;
      }

      private void groupByColumn()
      {
         if (!_groupByDefined) return;
         xtraColumnBy(GroupPathElement).GroupIndex = 0;
      }

      private GridColumn xtraColumnBy(PathElement pathElement)
      {
         return _pathElementsBinder.ColumnAt(pathElement).XtraColumn;
      }

      public IEnumerable<QuantitySelectionDTO> SelectedQuantities
      {
         get
         {
            return gridView.GetSelectedRows()
               .Select(rowHandle => _gridViewBinder.ElementAt(rowHandle));
         }
      }

      public void SetVisibility(QuantityColumn column, bool visible)
      {
         _otherColumns[column].Visible = visible;
         if (column != QuantityColumn.Selection) return;
         if (visible)
         {
            gridView.OptionsSelection.MultiSelect = false;
         }
         else
         {
            gridView.OptionsSelection.MultiSelect = true;
            gridView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect;
         }
      }

      public void SetVisibility(PathElement pathElement, bool visible)
      {
         _pathElementsBinder.SetVisibility(pathElement, visible);
      }

      public override bool HasError => _gridViewBinder.HasError;

      private void showPopupMenu(PopupMenuShowingEventArgs e)
      {
         if (!shouldShowPopupMenu(e)) return;

         var popupMenu = new DataBrowserPopupMenu(_gridViewBinder, _presenter, e.HitInfo.RowHandle);
         popupMenu.Show(e.HitInfo.HitPoint);
      }

      private bool shouldShowPopupMenu(PopupMenuShowingEventArgs e)
      {
         return e.MenuType == GridMenuType.Row && e.HitInfo.InRow && !gridView.IsDataRow(e.HitInfo.RowHandle);
      }

      public PathElement GroupPathElement
      {
         get => _groupPathElement;
         set
         {
            _groupPathElement = value;
            _groupByDefined = true;
         }
      }

      public PathElement SortedPathElement
      {
         get => _sortedPathElement;
         set
         {
            _sortedPathElement = value;
            _sortByDefined = true;
         }
      }

      internal class DataBrowserPopupMenu : GridViewMenu
      {
         private readonly GridViewBinder<QuantitySelectionDTO> _gridViewBinder;
         private readonly IQuantityListPresenter _presenter;
         private readonly int _groupRowHandle;

         public DataBrowserPopupMenu(GridViewBinder<QuantitySelectionDTO> gridViewBinder, IQuantityListPresenter presenter, int groupRowHandle)
            : base(gridViewBinder.GridView)
         {
            _gridViewBinder = gridViewBinder;
            _presenter = presenter;
            _groupRowHandle = groupRowHandle;
            Items.Add(new DXMenuItem("Select all", (o, e) => updateSelection(selected: true), ApplicationIcons.CheckAll));
            Items.Add(new DXMenuItem("Deselect all", (o, e) => updateSelection(selected: false), ApplicationIcons.UncheckAll));
         }

         private void updateSelection(bool selected)
         {
            var selectedItems = _gridViewBinder.SelectedItems(_groupRowHandle);
            _presenter.UpdateSelection(selectedItems.ToList(), selected);
         }
      }
   }
}