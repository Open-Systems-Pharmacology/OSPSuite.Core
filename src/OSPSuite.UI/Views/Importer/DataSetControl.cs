using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraTab;
using DevExpress.XtraTab.Buttons;
using DevExpress.XtraTab.ViewInfo;
using OSPSuite.Assets;
using OSPSuite.Core.Importer;
using OSPSuite.Core.Importer.Mappers;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Services.Importer;
using OSPSuite.Utility.Container;

namespace OSPSuite.UI.Views.Importer
{
   /// <summary>
   /// This control displays all tables of a given data set on a separate tab page.
   /// </summary>
   /// <remarks>
   /// If the table is an import data table the import data table control will be used.
   /// For standard tables the data table control is used.
   /// </remarks>
   public partial class DataSetControl : XtraUserControl
   {
      private readonly XtraTabControl _tabControl;
      private readonly PopupMenu _popupMenu;
      private readonly BarItem _deleteItem;
      private readonly BarItem _deleteAllItem;
      private readonly DataSet _dataSet;
      private XtraTabPage _contextPage;
      private readonly IReadOnlyList<ColumnInfo> _columnInfos;

      /// <summary>
      /// This is the constructor of the user control.
      /// </summary>
      public DataSetControl(DataSet data, IReadOnlyList<ColumnInfo> columnInfos = null, bool deleteAllButton = false)
      {
         InitializeComponent();
         _columnInfos = columnInfos;
         _dataSet = data;

         _tabControl = new XtraTabControl
         {
            Name = "TabControl", 
            Dock = DockStyle.Fill, 
            ClosePageButtonShowMode = ClosePageButtonShowMode.InActiveTabPageHeaderAndOnMouseHover,
            ToolTipController = new ToolTipController {ToolTipType = ToolTipType.SuperTip}
         };
         var customButton = new CustomHeaderButton(ButtonPredefines.Combo) {ToolTip = "Select Page"};
         _tabControl.CustomHeaderButtons.Add(customButton);
         _tabControl.CustomHeaderButtonClick += onTabControlCustomHeaderButtonClick;
         _tabControl.SelectedPageChanged += onTabControlSelectedPageChanged;
         _tabControl.MouseUp += onTabControlMouseUp;
         _tabControl.CloseButtonClick += onTabCloseButtonClick;
         _tabControl.MouseWheel += onTabControlMouseWheel;
         _tabControl.HeaderButtonClick += onTabControlOnHeaderButtonClick;

         Controls.Add(_tabControl);

         _popupMenu = new PopupMenu {Manager = barManager};
         _deleteItem = new BarButtonItem(barManager, "Delete");
         _deleteItem.ItemClick += onDeleteItemClick;
         _popupMenu.AddItem(_deleteItem);
         if (deleteAllButton)
         {
            _deleteAllItem = new BarButtonItem(barManager, "Delete All");
            _deleteAllItem.ItemClick += onDeleteAllItemClick;
            _popupMenu.AddItem(_deleteAllItem);
         }

         buildPages();
      }

      /// <summary>
      /// When using the navigator buttons the next or previous page should directly be selected.
      /// </summary>
      private void onTabControlOnHeaderButtonClick(object sender, HeaderButtonEventArgs headerButtonEventArgs)
      {
         switch (headerButtonEventArgs.Button)
         {
            case TabButtons.Next:
               _tabControl.SelectedTabPageIndex++;
               break;
            case TabButtons.Prev:
               _tabControl.SelectedTabPageIndex--;
               break;
         }
         headerButtonEventArgs.Handled = true;
      }

      /// <summary>
      /// On CTRL-Tab move foreward and on SHIFT-CTRL-Tab move backward.
      /// </summary>
      protected override bool ProcessDialogKey(Keys keyData)
      {
         var keyCode = keyData & Keys.KeyCode;
         if (keyCode == Keys.Tab)
            if ((keyData & Keys.Control) != Keys.None)
            {
               if ((keyData & Keys.Shift) == Keys.None)
                  _tabControl.SelectedTabPageIndex++;
               else
                  _tabControl.SelectedTabPageIndex--;
               return true;
            }
         return base.ProcessDialogKey(keyData);
      }

      /// <summary>
      /// Allow the user to move through pages by mouse wheel control.
      /// </summary>
      void onTabControlMouseWheel(object sender, MouseEventArgs e)
      {
         if (e.Delta < 0)
            _tabControl.SelectedTabPageIndex++;
         else
            _tabControl.SelectedTabPageIndex--;
      }

      /// <summary>
      /// Present a popup menu with all pages to select a page to move to.
      /// </summary>
      private void onTabControlCustomHeaderButtonClick(object sender, CustomHeaderButtonEventArgs e)
      {
         var popupMenu = new DXPopupMenu {MenuViewType = MenuViewType.Menu};
         foreach (XtraTabPage page in _tabControl.TabPages)
         {
            var menuitem = new DXMenuItem(page.Text);
            menuitem.Click += onPageListMenuItemClick;
            menuitem.Tag = popupMenu;
            if (page.Image != null)
               menuitem.Image = page.Image;
            popupMenu.Items.Add(menuitem);
         }
         var menuPos = _tabControl.PointToClient(MousePosition);
         MenuManagerHelper.ShowMenu(popupMenu, _tabControl.LookAndFeel, barManager, _tabControl, menuPos);
      }

      /// <summary>
      /// Handle page selection in page list popup menu.
      /// </summary>
      private void onPageListMenuItemClick(object sender, EventArgs e)
      {
         var menuItem = sender as DXMenuItem;
         if (menuItem == null) return;
         foreach(XtraTabPage page in _tabControl.TabPages)
         {
            if (page.Text != menuItem.Caption) continue;
            _tabControl.SelectedTabPage = page;
            _tabControl.MakePageVisible(page);
            break;
         }
         //dispose dynamically created objects.
         var popupMenu = menuItem.Tag as DXPopupMenu;
         if (popupMenu == null) return;
         foreach (DXMenuItem item in popupMenu.Items)
         {
            item.Click -= onPageListMenuItemClick;
            item.Dispose();
         }
         popupMenu.Items.Clear();
         popupMenu.Dispose();
      }

      /// <summary>
      /// This method actualizes the data of the data set control by rebuilding the pages.
      /// </summary>
      public void ActualizeData()
      {  
         buildPages();
      }

      /// <summary>
      /// Handler for event MissingRequiredData and RequiredDataCompleted.
      /// </summary>
      public delegate void RequiredDataHandler(object sender, EventArgs e);

      /// <summary>
      /// Event raised when all required data are completly entered.
      /// </summary>
      public event RequiredDataHandler RequiredDataCompleted;

      /// <summary>
      /// Event raised when there are required data not entered yet.
      /// </summary>
      public event RequiredDataHandler MissingRequiredData;

      /// <summary>
      /// Method for building pages for each table of the dataset.
      /// </summary>
      private void buildPages()
      {
         try
         {
            Cursor = Cursors.WaitCursor;

            _tabControl.BeginUpdate();

            // remove previously added user controls from page and free memory
            foreach (XtraTabPage page in _tabControl.TabPages)
               cleanPage(page);
            _tabControl.TabPages.Clear();

            foreach (DataTable table in _dataSet.Tables)
            {
               var tabPage = new XtraTabPage {Name = table.TableName, Text = table.TableName};
               _tabControl.TabPages.Add(tabPage);

               FillTabPageFromTable(table, tabPage);
            }
            _tabControl.EndUpdate();
            PerformLayout();
            checkForMissingData();
         }
         finally
         {
            Cursor = Cursors.Default;
         }
      }

      /// <summary>
      /// Fills controls on the tabPage from the data table. The tabpage will be cleared first
      /// </summary>
      /// <param name="table">The datatable used to build the tabpage</param>
      /// <param name="tabPage">The tabpage to insert the controls</param>
      public void FillTabPageFromTable(DataTable table, XtraTabPage tabPage)
      {
         tabPage.Controls.Clear();
         var importTable = table as ImportDataTable;

         XtraUserControl dataTableControl;
         if (importTable != null)
         {
            var idtc = new ImportDataTableControl(IoC.Resolve<ISimpleChartPresenter>(), IoC.Resolve<IImportDataTableToDataRepositoryMapper>(), IoC.Resolve<IImportDataTableGridPresenter>(), IoC.Resolve<IImporterTask>());
            idtc.Build(importTable, _columnInfos);
            dataTableControl = idtc;
         }
         else
            dataTableControl = new DataTableControl(table);

         Controls.Add(dataTableControl);

         var importDataTableControl = dataTableControl as ImportDataTableControl;
         if (importDataTableControl != null)
         {
            importDataTableControl.OnCopyMetaDataOnTable += onCopyMetaDataOnTable;
            importDataTableControl.OnBroadcastMetaDataOnTable += onBroadcastMetaDataOnTable;
            importDataTableControl.OnCopyMetaDataOnColumn += onCopyMetaDataOnColumn;
            importDataTableControl.UnitChanged += onImportDataTableControlDataChangedForEvents;
            importDataTableControl.UnitChanged += onImportDataTableControlDataChangedForPageIcon;
            importDataTableControl.OnCopyUnitInputParametersOnColumn += onCopyUnitInputParametersOnColumn;
            importDataTableControl.OnCopyUnitInfoOnColumn += onCopyUnitInfoOnColumn;
            importDataTableControl.MetaDataChanged += onImportDataTableControlDataChangedForEvents;
            importDataTableControl.MetaDataChanged += onImportDataTableControlDataChangedForPageIcon;
            tabPage.Image = importTable.HasMissingData
               ? ApplicationIcons.MissingData.ToImage()
               : ApplicationIcons.EmptyIcon.ToImage();
         }
         tabPage.Controls.Add(dataTableControl);
         if (table.ExtendedProperties.ContainsKey("Protocol"))
         {
            var importProtocol = (string) table.ExtendedProperties["Protocol"];
            tabPage.SuperTip = new SuperToolTip();
            tabPage.SuperTip.Items.AddTitle(table.TableName);
            tabPage.SuperTip.Items.Add(importProtocol);
         }
         else
         {
            tabPage.SuperTip = null;
         }
         dataTableControl.Dock = DockStyle.Fill;
      }

      void onTabCloseButtonClick(object sender, EventArgs e)
      {
         var eventArgs = e as ClosePageButtonEventArgs;
         if (eventArgs == null) return;
         var page = eventArgs.Page as XtraTabPage;
         if (page == null) return;
         deleteTable(page);
      }


      /// <summary>
      /// This method removes a single table from control and releases the objects.
      /// </summary>
      private void deleteTable(XtraTabPage page)
      {
         if (_dataSet.Tables.Contains(page.Name))
         {
            var table = _dataSet.Tables[page.Name];
            table.Dispose();
            _dataSet.Tables.Remove(table);
         }

         cleanPage(page);
         var selectedIndex = _tabControl.SelectedTabPageIndex;
         _tabControl.TabPages.Remove(page);
         if (selectedIndex >= 0 && selectedIndex < _tabControl.TabPages.Count)
            _tabControl.SelectedTabPageIndex = selectedIndex;
         else
            _tabControl.SelectedTabPageIndex = _tabControl.TabPages.Count - 1;
         _tabControl.MakePageVisible(_tabControl.SelectedTabPage);

         if (TableDeleted != null) 
            TableDeleted(this, new TableDeletedEventArgs { TableCount = _dataSet.Tables.Count });
         checkForMissingData();
      }

      /// <summary>
      /// This methods removes all table from control and releases the objects.
      /// </summary>
      private void deleteAllTables()
      {
         _tabControl.BeginUpdate();

         foreach (DataTable table in _dataSet.Tables)
            table.Dispose();

         foreach (XtraTabPage page in _tabControl.TabPages)
            cleanPage(page);

         _tabControl.TabPages.Clear();
         _dataSet.Tables.Clear();
         _tabControl.EndUpdate();

         if (TableDeleted != null)
            TableDeleted(this, new TableDeletedEventArgs { TableCount = _dataSet.Tables.Count });
      }

      /// <summary>
      /// This methods checks all import tables in the data set for missing data.
      /// <remarks>It throws the corresponding events if requested.</remarks>
      /// </summary>
      private void checkForMissingData()
      {
         foreach (var table in _dataSet.Tables)
         {
            var importTable = table as ImportDataTable;
            if (importTable == null) return;
            if (!importTable.HasMissingData) continue;
            if (MissingRequiredData != null)
               MissingRequiredData(this, new EventArgs());
            return;
         }
         if (_dataSet.Tables.Count == 0)
         {
            if (MissingRequiredData != null)
               MissingRequiredData(this, new EventArgs());
            return;
         }
         if (RequiredDataCompleted != null)
            RequiredDataCompleted(this, new EventArgs());
      }

      /// <summary>
      /// Method for copying unit input parameters of one column to columns of the other tables.
      /// </summary>
      void onCopyUnitInputParametersOnColumn(object sender, ImportDataTableControl.CopyUnitInputParametersOnColumnEventArgs e)
      {
         var importDataControl = sender as ImportDataTableControl;
         if (importDataControl == null) return;

         foreach (XtraTabPage page in _tabControl.TabPages)
            foreach (ImportDataTableControl importDataTableControl in page.Controls)
            {
               if (importDataTableControl == null) continue;
               if (importDataTableControl != importDataControl)
                  importDataTableControl.SetInputParametersForColumn(e.InputParameters, e.Dimension, e.ColumnName);
            }
      }

      /// <summary>
      /// Method for copying unit information of one column to columns of the other tables.
      /// </summary>
      void onCopyUnitInfoOnColumn(object sender, SetUnitView.CopyUnitInfoEventArgs e)
      {
         var importDataControl = sender as ImportDataTableControl;
         if (importDataControl == null) return;

         foreach (XtraTabPage page in _tabControl.TabPages)
            foreach (ImportDataTableControl importDataTableControl in page.Controls)
            {
               if (importDataTableControl == null) continue;
               if (importDataTableControl != importDataControl)
                  importDataTableControl.SetUnitInformationForColumn(e.Dimension, e.Unit, e.ColumnName);
            }
      }

      /// <summary>
      /// Method for actualizing the icon of the page according to current data states of the tables.
      /// </summary>
      void onImportDataTableControlDataChangedForPageIcon(object sender, EventArgs e)
      {
         var tableControl = sender as ImportDataTableControl;
         var dataMissing = false;
         XtraTabPage currentPage = null;
         foreach (XtraTabPage page in _tabControl.TabPages)
         {
            if (!page.Controls.Contains(tableControl)) continue;
            currentPage = page;
            var table = _dataSet.Tables[page.Name] as ImportDataTable;
            if (table == null) continue;
            dataMissing = table.HasMissingData;
            if (dataMissing) break;
         }

         if (currentPage == null) return;
         currentPage.Image = dataMissing ? ApplicationIcons.MissingData.ToImage() : ApplicationIcons.EmptyIcon.ToImage();
      }

      /// <summary>
      /// Method for sending events according to current data states of the tables.
      /// </summary>
      void onImportDataTableControlDataChangedForEvents(object sender, EventArgs e)
      {
         var dataMissing = (_tabControl.TabPages.Count == 0);
         foreach(XtraTabPage page in _tabControl.TabPages)
         {
            if (!_dataSet.Tables.Contains(page.Name)) continue;
            var table = _dataSet.Tables[page.Name] as ImportDataTable;
            if (table == null) continue;
            dataMissing = table.HasMissingData;
            if (dataMissing) break;
         }

         if (dataMissing)
         {
            if (MissingRequiredData != null)
               MissingRequiredData(this, new EventArgs());
         }
         else
         {
            if (RequiredDataCompleted != null)
               RequiredDataCompleted(this, new EventArgs());
         }
      }

      /// <summary>
      /// Event arguments for event TableDeleted.
      /// </summary>
      public class TableDeletedEventArgs : EventArgs
      {
         /// <summary>
         /// Number of left tables.
         /// </summary>
         public int TableCount;
      }

      /// <summary>
      /// Handler for event TableDeleted.
      /// </summary>
      public delegate void DeleteTableHandler(object sender, TableDeletedEventArgs e);

      /// <summary>
      /// Event raised when table is deleted.
      /// </summary>
      public event DeleteTableHandler TableDeleted;

      /// <summary>
      /// Method for removing a table from the dataset and for removing the page from the control.
      /// </summary>
      void onDeleteItemClick(object sender, ItemClickEventArgs e)
      {
         if (e.Item != _deleteItem) return;
         deleteTable(_contextPage);
      }

      /// <summary>
      /// Method for removing a table from the dataset and for removing the page from the control.
      /// </summary>
      void onDeleteAllItemClick(object sender, ItemClickEventArgs e)
      {
         if (e.Item != _deleteAllItem) return;
         deleteAllTables();
      }

      /// <summary>
      /// Method for generating the context menu on the page header.
      /// </summary>
      private void onTabControlMouseUp(object sender, MouseEventArgs e)
      {
         if (e.Button != MouseButtons.Right) return;
         var tabCtrl = sender as XtraTabControl;
         if (tabCtrl == null) return;

         var pt = MousePosition;
         var info = tabCtrl.CalcHitInfo(tabCtrl.PointToClient(pt));
         if (info.HitTest != XtraTabHitTest.PageHeader) return;
         if (!_dataSet.Tables.Contains(info.Page.Name)) return;

         _contextPage = info.Page;
         _popupMenu.ShowPopup(pt);
      }

      /// <summary>
      /// Property retrieving the currently selected table.
      /// </summary>
      public DataTable SelectedTable
      {
         get { return _dataSet.Tables[_tabControl.SelectedTabPageIndex]; }
      }

      /// <summary>
      /// To set the shown table from outside.
      /// </summary>
      public void SetSelectedTabPageIndex(int index)
      {
         _tabControl.SelectedTabPageIndex = index;
      }

      /// <summary>
      /// Event arguments for event TableSelected.
      /// </summary>
      public class TableSelectedEventArgs : EventArgs
      {
         /// <summary>
         /// Name of selected table.
         /// </summary>
         public string TableName { get; set; }
         /// <summary>
         /// Table object of the selected table.
         /// </summary>
         public DataTable Table { get; set; }
      }
      /// <summary>
      /// Handler for event TableSelected.
      /// </summary>
      public delegate void TableSelectedHandler(object sender, TableSelectedEventArgs e);

      /// <summary>
      /// Event raised when the selected page changes. 
      /// </summary>
      public event TableSelectedHandler TableSelected;

      /// <summary>
      /// Method raising the TableSelected event.
      /// </summary>
      void onTabControlSelectedPageChanged(object sender, TabPageChangedEventArgs e)
      {
         if (e.Page == null) return;
         if (!_tabControl.TabPages.Contains(e.Page)) return;
         if (_dataSet.Tables.Count == 0) return;

         if(e.Page != null)
            e.Page.Appearance.Header.Font = Fonts.SelectedTabHeaderFont;
         if(e.PrevPage != null)
            e.PrevPage.Appearance.Header.Font = Fonts.NonSelectedTabHeaderFont;

         DataTable table = _dataSet.Tables[_tabControl.TabPages.IndexOf(e.Page)];
         if (table == null) return;
         if (TableSelected != null) TableSelected(this, new TableSelectedEventArgs { TableName = table.TableName, Table = table });
      }

      /// <summary>
      /// Method which copies meta data from current table to the other ones.
      /// </summary>
      void onCopyMetaDataOnTable(object sender, ImportDataTableControl.CopyMetaDataOnTableEventArgs e)
      {
         var importDataControl = sender as ImportDataTableControl;
         if (importDataControl == null) return;

         foreach (XtraTabPage page in _tabControl.TabPages)
            foreach (ImportDataTableControl importDataTableControl in page.Controls)
            {
               if (importDataTableControl == null) continue;
               if (importDataTableControl != importDataControl)
                  importDataTableControl.SetMetaDataForTable(e.MetaData);
            }
      }


      private void onBroadcastMetaDataOnTable(object sender, BroadcastMetaDataOnTableEventArgs e)
      {
         var importDataControl = sender as ImportDataTableControl;
         if (importDataControl == null) return;

         foreach (XtraTabPage page in _tabControl.TabPages)
            foreach (ImportDataTableControl importDataTableControl in page.Controls)
            {
               if (importDataTableControl == null) continue;
               if (importDataTableControl != importDataControl)
                  importDataTableControl.SetMetaDataForTable(e.Name, e.Value);
            }
      }


      /// <summary>
      /// Method which copies meta data from current table column to the same columns of the other tables.
      /// </summary>
      void onCopyMetaDataOnColumn(object sender, ImportDataTableControl.CopyMetaDataOnColumnEventArgs e)
      {
         var importDataControl = sender as ImportDataTableControl;
         if (importDataControl == null) return;

         foreach (XtraTabPage page in _tabControl.TabPages)
            foreach (ImportDataTableControl importDataTableControl in page.Controls)
            {
               if (importDataTableControl == null) continue;
               if (importDataTableControl != importDataControl)
                  importDataTableControl.SetMetaDataForColumn(e.MetaData, e.ColumnName);
            }

      }

      private void cleanPage(XtraTabPage page)
      {
         for (var i = page.Controls.Count - 1; i > -1; --i)
         {
            var userControl = (XtraUserControl)page.Controls[i];
            if (userControl == null) continue;
            page.Controls.Remove(userControl);
            userControl.Dispose();
         }
      }

      private void cleanMemory()
      {
         if (_dataSet != null)
         {
            foreach (DataTable table in _dataSet.Tables)
               table.Dispose();
            _dataSet.Dispose();
         }

         if (_tabControl != null)
         {
            foreach (XtraTabPage page in _tabControl.TabPages)
            {
               CleanUpHelper.ReleaseControls(page.Controls);
               page.Controls.Clear();
            }
            _tabControl.Dispose();
         }

         CleanUpHelper.ReleaseControls(Controls);
         Controls.Clear();
      }

      public void UpdateTable(string tableName, DataTable table)
      {
         _dataSet.Tables[tableName].Clear();
         _dataSet.Tables[tableName].Columns.Clear();
         _dataSet.Tables[tableName].Merge(table);
         FillTabPageFromTable(_dataSet.Tables[tableName], _tabControl.SelectedTabPage);
      }
   }
}
