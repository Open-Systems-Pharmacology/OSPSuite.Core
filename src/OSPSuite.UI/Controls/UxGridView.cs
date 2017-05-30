using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Localization;
using DevExpress.XtraGrid.Menu;
using DevExpress.XtraGrid.Scrolling;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using OSPSuite.Assets;
using OSPSuite.Core.Extensions;
using OSPSuite.UI.Mappers;
using OSPSuite.UI.Services;

namespace OSPSuite.UI.Controls
{
   public class UxGridView : GridView
   {
      private IClipboardTask _clipboardCopyTask;
      private IGridViewToDataTableMapper _gridViewToDataTableMapper;

      private DataTable rowSelectionOnlyTable => _gridViewToDataTableMapper.MapFrom(this, GetSelectedRows());
      private DataTable table => _gridViewToDataTableMapper.MapFrom(this);
      private DataTable rectangularSelectionOnlyTable => _gridViewToDataTableMapper.MapFrom(this, GetSelectedRows(), GetSelectedCells);

      protected override string ViewName => "UxGridView";

      /// <summary>
      ///    Color used for cell that are locked/disabled (End of gradient)
      /// </summary>
      protected Color _colorDisabled = Color.LightGray;

      public UxGridView(GridControl gridControl) : base(gridControl)
      {
         DoInit();
      }

      public UxGridView()
      {
         DoInit();
      }

      public void AddMessageInEmptyArea(CustomDrawEventArgs e, string message)
      {
         var font = new Font(AppearanceObject.DefaultFont, FontStyle.Bold);
         var r = new Rectangle(e.Bounds.Left + 5, e.Bounds.Top + 5, e.Bounds.Width - 5, e.Bounds.Height - 5);
         e.Graphics.DrawString(message, font, Brushes.Black, r);
      }

      /// <summary>
      ///    True if the grid view should use the  color "_colorDisabled" to display the color in lock cells otherwise false
      ///    Default is true
      /// </summary>
      [Browsable(false)]
      [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
      public bool ShouldUseColorForDisabledCell { get; set; }

      /// <summary>
      ///    True if the grid view should show the column chooser
      ///    Default is false
      /// </summary>
      [Browsable(false)]
      [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
      public bool ShowColumnChooser { get; set; }

      /// <summary>
      ///    True if the grid view should show the row indicator
      ///    Default is false
      /// </summary>
      [Browsable(false)]
      [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
      public bool ShowArrowInRowIndicator { get; set; }

      /// <summary>
      ///    True if the grid view should show the row indicator (header)
      ///    Default is true
      /// </summary>
      [Browsable(false)]
      [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
      public bool ShowRowIndicator
      {
         set { OptionsView.ShowIndicator = value; }
         get { return OptionsView.ShowIndicator; }
      }

      protected virtual void DoInit()
      {
         ShouldUseColorForDisabledCell = true;
         ShowColumnChooser = false;
         ShowArrowInRowIndicator = false;
         KeyDown += onKeyDownDefaultBehavior;
         CustomDrawRowIndicator += removeArrowRowIndicator;
         OptionsView.EnableAppearanceOddRow = false;
         OptionsView.EnableAppearanceEvenRow = false;
         OptionsBehavior.EditorShowMode = EditorShowMode.MouseDown;
         OptionsSelection.EnableAppearanceFocusedCell = false;
         OptionsSelection.EnableAppearanceFocusedRow = false;
         OptionsNavigation.AutoFocusNewRow = true;
         RowCellStyle += onRowCellStyle;
         PopupMenuShowing += disableColumnChooser;
         _clipboardCopyTask = new ClipboardTask();
         _gridViewToDataTableMapper = new GridViewToDataTableMapper();

         KeyDown += onProcessGridKey;
         PopupMenuShowing += OnPopupMenuShowing;
         OptionsSelection.MultiSelect = true;
         OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect;
      }

      private void onRowCellStyle(object sender, RowCellStyleEventArgs e)
      {
         if (!ShouldUseColorForDisabledCell) return;
         if (e.Column == null) return;
         if (e.Column.OptionsColumn.AllowEdit) return;
         if (!e.Column.OptionsColumn.ReadOnly) return;

         //column is readonly
         AdjustAppearance(e, false);
      }

      /// <summary>
      ///    If the row is enabled, use the default color for enabled, otherwise set the back color to disabled
      /// </summary>
      public void AdjustAppearance(RowStyleEventArgs e, bool isEnabled)
      {
         if (isEnabled)
            e.CombineAppearance(Appearance.Row);
         else
         {
            AdjustAppearance(e, _colorDisabled);
         }
      }

      public void AdjustAppearance(RowCellStyleEventArgs e, bool isEnabled)
      {
         if (isEnabled)
            e.CombineAppearance(Appearance.Row);
         else
            AdjustAppearance(e, _colorDisabled);
      }

      public void AdjustAppearance(RowCellStyleEventArgs e, Color color)
      {
         if (rowHasFocus(e.RowHandle))
            e.CombineAppearance(Appearance.FocusedCell);
         else
            updateAppearanceBackColor(e.Appearance, color);
      }

      public void AdjustAppearance(RowStyleEventArgs e, Color color)
      {
         if (rowHasFocus(e.RowHandle))
            e.CombineAppearance(Appearance.FocusedRow);
         else
            updateAppearanceBackColor(e.Appearance, color);
      }

      private void updateAppearanceBackColor(AppearanceObject appearance, Color color)
      {
         appearance.BackColor = color;
         appearance.BackColor2 = color;
      }

      private bool rowHasFocus(int rowHandle)
      {
         return (FocusedRowHandle == rowHandle) && OptionsSelection.EnableAppearanceFocusedRow;
      }

      private void removeArrowRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
      {
         if (ShowArrowInRowIndicator) return;
         //remove row indicator with index 0;
         if (e.Info.ImageIndex == 0)
            e.Info.ImageIndex = -1;
         e.Painter.DrawObject(e.Info);
      }

      private void onKeyDownDefaultBehavior(object sender, KeyEventArgs e)
      {
         if (e.KeyCode == Keys.Escape)
            if (HasColumnErrors)
               e.Handled = true;
      }

      /// <summary>
      ///    set the appearance of the filtering panel
      ///    (AllowFilter,AllowGroup, AllowSort etc..)
      /// </summary>
      public bool AllowsFiltering
      {
         set
         {
            OptionsCustomization.AllowFilter = value;
            OptionsCustomization.AllowGroup = value;
            OptionsCustomization.AllowSort = value;
            OptionsFilter.AllowColumnMRUFilterList = value;
            OptionsFilter.AllowFilterEditor = value;
            OptionsFilter.AllowMRUFilterList = value;
            OptionsView.ShowGroupPanel = value;
            OptionsCustomization.AllowColumnMoving = value;
         }
         get { return OptionsCustomization.AllowFilter; }
      }

      [Browsable(false)]
      [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
      public bool ShowColumnHeaders
      {
         set { OptionsView.ShowColumnHeaders = value; }
         get { return OptionsView.ShowColumnHeaders; }
      }

      private void disableColumnChooser(object sender, PopupMenuShowingEventArgs e)
      {
         if (ShowColumnChooser)
            return;

         if (e.MenuType != GridMenuType.Column)
            return;

         // Customize

         var miCustomize = menuById(e.Menu, GridStringId.MenuColumnColumnCustomization);
         if (miCustomize != null)
            miCustomize.Visible = false;

         var remove = menuById(e.Menu, GridStringId.MenuColumnRemoveColumn);
         if (remove != null)
            remove.Visible = false;
      }

      private DXMenuItem menuById(DXPopupMenu menu, GridStringId id)
      {
         string caption = GridLocalizer.Active.GetLocalizedString(id);
         return menu.Items.Cast<DXMenuItem>().FirstOrDefault(item => item.Caption == caption);
      }

      [Browsable(false)]
      [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
      public EditorShowMode EditorShowMode
      {
         get { return OptionsBehavior.EditorShowMode; }
         set { OptionsBehavior.EditorShowMode = value; }
      }

      [Browsable(false)]
      [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
      public bool RowsInsertable
      {
         get { return OptionsView.NewItemRowPosition != NewItemRowPosition.None; }
         set { OptionsView.NewItemRowPosition = value ? NewItemRowPosition.Bottom : NewItemRowPosition.None; }
      }

      /// <summary>
      ///    Returns the optimal height for the actual view (so that no white space is visible)
      /// </summary>
      public int OptimalHeight
      {
         get
         {
            var viewInfo = (GridViewInfo) GetViewInfo();
            FieldInfo fi = typeof(GridView).GetField("scrollInfo", BindingFlags.Instance | BindingFlags.NonPublic);
            Rectangle oldBounds = viewInfo.Bounds;

            try
            {
               int height = viewInfo.CalcRealViewHeight(new Rectangle(0, 0, Int32.MaxValue, Int32.MaxValue));
               viewInfo.CalcRealViewHeight(oldBounds);

               var scrollInfo = (ScrollInfo) fi.GetValue(this);
               if (scrollInfo.HScrollVisible)
                  height += scrollInfo.HScrollSize;

               return height;
            }
            catch (Exception)
            {
               return oldBounds.Height;
            }
         }
      }

      /// <summary>
      ///    Returns a stream containing the layout of the grid view
      /// </summary>
      /// <returns></returns>
      public Stream LayoutToStream()
      {
         var stream = new MemoryStream();
         SaveLayoutToStream(stream);
         return stream;
      }

      /// <summary>
      ///    Returns the grid hit info for a valid hit, null otherwise
      /// </summary>
      /// <returns></returns>
      public GridHitInfo HitInfoAt(MouseEventArgs e)
      {
         return HitInfoAt(new Point(e.X, e.Y));
      }

      /// <summary>
      ///    Returns the grid hit info for a valid hit, null otherwise
      /// </summary>
      /// <returns></returns>
      public GridHitInfo HitInfoAt(Point point)
      {
         var hi = CalcHitInfo(point);
         if (hi == null || !hi.IsValid)
            return null;

         return hi;
      }

      /// <summary>
      ///    return the row handle located at the mouse position and -1 if not valid
      /// </summary>
      public int RowHandleAt(MouseEventArgs e)
      {
         var hi = HitInfoAt(e);
         if (hi == null) return -1;
         return hi.RowHandle;
      }

      /// <summary>
      ///    Returns the columns where the mouse was positioned. Null if no column was under the mouse
      /// </summary>
      public GridColumn ColumnAt(ToolTipControllerGetActiveObjectInfoEventArgs e)
      {
         return ColumnAt(e.ControlMousePosition);
      }

      /// <summary>
      ///    Returns the columns where the mouse was positioned. Null if no column was under the mouse
      /// </summary>
      public GridColumn ColumnAt(Point point)
      {
         return ColumnAt(HitInfoAt(point));
      }

      /// <summary>
      ///    Returns the columns where the mouse was positioned. Null if no column was under the mouse
      /// </summary>
      public GridColumn ColumnAt(MouseEventArgs e)
      {
         return ColumnAt(HitInfoAt(e));
      }

      /// <summary>
      ///    Returns the columns where the mouse was positioned. Null if no column was under the mouse
      /// </summary>
      public GridColumn ColumnAt(GridHitInfo hi)
      {
         return hi == null ? null : hi.Column;
      }

      /// <summary>
      ///    Load the grid view layout from the stream containing the serialized layout
      /// </summary>
      /// <param name="stream"></param>
      public void LoadLayoutFromStream(Stream stream)
      {
         // Set the position to the beginning of the stream.
         stream.Seek(0, SeekOrigin.Begin);
         RestoreLayoutFromStream(stream);
      }

      private bool gridIsCellSelectMode()
      {
         return OptionsSelection.MultiSelectMode == GridMultiSelectMode.CellSelect;
      }

      public void OnPopupMenuShowing(object sender, PopupMenuShowingEventArgs popupMenuShowingEventArgs)
      {
         var gridViewMenu = popupMenuShowingEventArgs.Menu;
         if (gridViewMenu == null)
            return;

         if (popupMenuShowingEventArgs.HitInfo.HitTest != GridHitTest.RowIndicator)
            return;

         // If this is a template detail view for master/detail pattern, it won't have any rows.
         // In that case, don't add any context menu items, they will be added by the template clone.
         if (GetSelectedRows().Length == 0)
            return;

         addCommonCopyMenuItems(gridViewMenu);

         if (gridIsCellSelectMode())
         {
            addCopyMenuItemsForCellSelect(gridViewMenu);
         }
         else
         {
            addCopyMenuItemsForRowSelect(gridViewMenu);
         }
      }

      private void addCopyMenuItemsForRowSelect(GridViewMenu gridViewMenu)
      {
         var copyRowMenu = new DXMenuItem(Captions.CopySelectedRows, (o, args) => copyRowSelectionToClipboard(), ApplicationIcons.CopySelection) {Shortcut = Shortcut.CtrlC};
         gridViewMenu.Items.Insert(0, copyRowMenu);
      }

      private void addCopyMenuItemsForCellSelect(GridViewMenu gridViewMenu)
      {
         var copyRowMenu = new DXMenuItem(Captions.CopySelectedRows, (o, args) => copyRowSelectionToClipboard(), ApplicationIcons.CopySelection);
         var copySelectionMenu = new DXMenuItem(Captions.CopySelection, (o, args) => processSelectiveCopyToClipboard(), ApplicationIcons.CopySelection) {Shortcut = Shortcut.CtrlC};
         gridViewMenu.Items.Insert(0, copyRowMenu);
         gridViewMenu.Items.Insert(0, copySelectionMenu);
      }

      private void addCommonCopyMenuItems(GridViewMenu gridViewMenu)
      {
         var copyAllMenu = new DXMenuItem(Captions.CopyTable, (o, args) => copyEntireGridToClipboard(), ApplicationIcons.Copy) {Shortcut = Shortcut.CtrlShiftC};
         gridViewMenu.Items.Insert(0, copyAllMenu);
      }

      private void copyRowSelectionToClipboard()
      {
         copyTableToClipboard(rowSelectionOnlyTable);
      }

      private void copyEntireGridToClipboard()
      {
         copyTableToClipboard(table);
      }

      private void processSelectiveCopyToClipboard()
      {
         if (rectangularAreaIsSelected())
            copyTableToClipboard(rectangularSelectionOnlyTable, includeHeaders: false);
         else
            copyRowSelectionToClipboard();
      }

      public bool EnableColumnContextMenu
      {
         set { OptionsMenu.EnableColumnMenu = value; }
         get { return OptionsMenu.EnableColumnMenu; }
      }

      public bool MultiSelect
      {
         get { return OptionsSelection.MultiSelect; }
         set
         {
            OptionsSelection.EnableAppearanceFocusedRow = value;
            OptionsSelection.MultiSelect = value;
            OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect;
         }
      }

      private void onProcessGridKey(object sender, KeyEventArgs keyEventArgs)
      {
         if (keyEventArgs.KeyCode != Keys.C || !keyEventArgs.Control) return;

         if (forceEntireGridCopy(keyEventArgs))
            copyEntireGridToClipboard();
         else
            processSelectiveCopyToClipboard();

         keyEventArgs.Handled = true;
      }

      private bool rectangularAreaIsSelected()
      {
         return gridIsCellSelectMode() && areaIsRectangular();
      }

      private bool areaIsRectangular()
      {
         var selectedRows = GetSelectedRows();
         if (!selectedRows.Any())
            return false;

         var firstRow = selectedRows.First();
         var firstRowColumnsSelected = GetSelectedCells(firstRow);

         return selectedRows.Except(new[] {firstRow}).Select(GetSelectedCells).All(currentRowSelectedColumns => allColumnsAreCommon(firstRowColumnsSelected, currentRowSelectedColumns));
      }

      private static bool allColumnsAreCommon(GridColumn[] firstRowColumnsSelected, GridColumn[] currentRowSelectedColumns)
      {
         return firstRowColumnsSelected.ContainsAll(currentRowSelectedColumns) && currentRowSelectedColumns.ContainsAll(firstRowColumnsSelected);
      }

      private static bool forceEntireGridCopy(KeyEventArgs keyEventArgs)
      {
         return keyEventArgs.Shift;
      }

      private void copyTableToClipboard(DataTable dataTable, bool includeHeaders = true)
      {
         if (dataTable.Rows.Count == 0)
            return;
         Clipboard.SetDataObject(_clipboardCopyTask.CreateDataObject(dataTable, includeHeaders));
      }
   }
}