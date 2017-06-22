using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Views.Charts;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.RepositoryItems;

namespace OSPSuite.UI.Views.Charts
{
   internal partial class DataBrowserView : BaseUserControlWithColumnSettings, IDataBrowserView
   {
      private GridHitInfo _downHitInfo;
      private IDataBrowserPresenter _presenter;
      private readonly GridViewBinder<DataColumnDTO> _gridViewBinder;
      private readonly RepositoryItem _usedRepository;

      public DataBrowserView()
      {
         InitializeComponent();

         gridView.AllowsFiltering = true;
         gridView.ShowColumnChooser = true;
         gridView.ShouldUseColorForDisabledCell = false;
         gridView.OptionsView.ShowGroupPanel = true;
         gridView.MultiSelect = true;
         gridView.MouseDown += (o, e) => OnEvent(viewMouseDown, e);
         gridView.MouseMove += (o, e) => OnEvent(viewMouseMove, e);
         gridView.PopupMenuShowing += (o, e) => OnEvent(viewPopupMenuShowing, e);
         gridView.SelectionChanged += (o, e) => OnEvent(onGridViewSelectionChanged);
         InitializeWith(gridView);
         _gridViewBinder = new GridViewBinder<DataColumnDTO>(gridView);
         _usedRepository = new UxRepositoryItemCheckEdit(gridView);
      }

      private void onGridViewSelectionChanged()
      {
         _presenter.SelectedDataColumnsChanged();
      }

      public void AttachPresenter(IDataBrowserPresenter presenter)
      {
         _presenter = presenter;
         base.AttachPresenter(presenter);
      }

      public void BindTo(IEnumerable<DataColumnDTO> dataColumnDTOs)
      {
         DoWithoutColumnSettingsUpdateNotification(() =>
         {
            _gridViewBinder.BindToSource(dataColumnDTOs);
         });
      }

      public override void InitializeBinding()
      {
         bind(x => x.RepositoryName, BrowserColumns.RepositoryName);
         bind(x => x.Simulation, BrowserColumns.Simulation);
         bind(x => x.TopContainer, BrowserColumns.TopContainer);
         bind(x => x.Container, BrowserColumns.Container);
         bind(x => x.BottomCompartment, BrowserColumns.BottomCompartment);
         bind(x => x.Molecule, BrowserColumns.Molecule);
         bind(x => x.Name, BrowserColumns.Name);
         bind(x => x.BaseGridName, BrowserColumns.BaseGridName);
         bind(x => x.OrderIndex, BrowserColumns.OrderIndex);
         bind(x => x.DimensionName, BrowserColumns.DimensionName);
         bind(x => x.QuantityType, BrowserColumns.QuantityType);
         bind(x => x.QuantityName, BrowserColumns.QuantityName);
         bind(x => x.Origin, BrowserColumns.Origin);
         bind(x => x.Date, BrowserColumns.Date);
         bind(x => x.Category, BrowserColumns.Category);
         bind(x => x.Source, BrowserColumns.Source);

         var usedColumn = _gridViewBinder.Bind(x => x.Used)
            .WithRepository(dto => _usedRepository)
            .WithOnValueUpdating((dto, e) => _presenter.UsedChangedFor(dto, e.NewValue));

         usedColumn.XtraColumn.Tag = BrowserColumns.Used.ToString();
      }

      private IGridViewColumn bind<T>(Expression<Func<DataColumnDTO, T>> propertyToBindTo, BrowserColumns browserColumn)
      {
         var column = _gridViewBinder.Bind(propertyToBindTo)
            .AsReadOnly();

         column.XtraColumn.Tag = browserColumn.ToString();
         return column;
      }

      public IReadOnlyList<DataColumnDTO> SelectedColumns => dtoListFrom(gridView.GetSelectedRows());

      public IReadOnlyList<DataColumnDTO> SelectedDescendantColumns => selectDescendentDataRows(gridView.GetSelectedRows());

      private IReadOnlyList<DataColumnDTO> dtoListFrom(IEnumerable<int> rowHandles) => rowHandles.Select(_gridViewBinder.ElementAt).ToList();

      private IReadOnlyList<DataColumnDTO> selectDescendentDataRows(IEnumerable<int> selectedRowHandles)
      {
         return selectedRowHandles.SelectMany(rowHandle => _gridViewBinder.SelectedItems(rowHandle)).ToList();
      }

      private void viewPopupMenuShowing(PopupMenuShowingEventArgs e)
      {
         if (!shouldShowPopup(e)) return;
         var popupMenu = new DataBrowserPopupMenu(_gridViewBinder, _presenter, e.HitInfo.RowHandle);
         popupMenu.Show(e.HitInfo.HitPoint);
      }

      private bool shouldShowPopup(PopupMenuShowingEventArgs e)
      {
         return e.MenuType == GridMenuType.Row && e.HitInfo.InRow && !gridView.IsDataRow(e.HitInfo.RowHandle);
      }

      private void viewMouseDown(MouseEventArgs e)
      {
         _downHitInfo = null;
         GridHitInfo hitInfo = gridView.CalcHitInfo(new Point(e.X, e.Y));
         if (ModifierKeys != Keys.None) return;
         if (e.Button == MouseButtons.Left && hitInfo.RowHandle >= 0)
            _downHitInfo = hitInfo;
      }

      private void viewMouseMove(MouseEventArgs e)
      {
         if (e.Button == MouseButtons.Left && _downHitInfo != null)
         {
            Size dragSize = SystemInformation.DragSize;
            var dragRect = new Rectangle(new Point(_downHitInfo.HitPoint.X - dragSize.Width / 2,
               _downHitInfo.HitPoint.Y - dragSize.Height / 2), dragSize);

            if (!dragRect.Contains(new Point(e.X, e.Y)))
            {
               var data = _presenter.SelectedDataColumns;
               gridControl.DoDragDrop(data, DragDropEffects.Move);
               _downHitInfo = null;
               DXMouseEventArgs.GetMouseArgs(e).Handled = true;
            }
         }
      }
   }
}