using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraTab;
using DevExpress.XtraTab.Buttons;
using DevExpress.XtraTab.ViewInfo;
using OSPSuite.Assets;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Views.Importer;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Services;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.UI.Views.Importer
{
   public partial class ImporterDataView : BaseUserControl, IImporterDataView
   {
      private IImporterDataPresenter _dataPresenter;
      private readonly IImageListRetriever _imageListRetriever;
      private Cache<string, TabMarkInfo> _tabMarks = new Cache<string, TabMarkInfo>(onMissingKey: _ => new TabMarkInfo(null, false));
      private string _contextMenuSelectedTab;
      private bool sheetImportedFlag;
      private bool allSheetsImportedFlag;
      private bool allImportButtonsDisabledFlag;

      public ImporterDataView(IImageListRetriever imageListRetriever)
      {
         InitializeComponent();
         btnImport.Click += (s, a) => OnEvent(onButtonImportClicked, s, a);
         btnImportAll.Click += (s, a) => OnEvent(onButtonImportAllClicked, s, a);
         importerTabControl.FillWith(dataViewingGridControl);
         importerTabControl.SelectedPageChanged += (s, a) => OnEvent(onSelectedPageChanged, s, a);
         importerTabControl.CloseButtonClick += (s, a) => OnEvent(onCloseTab, s, a);
         importerTabControl.MouseDown += (s, a) => OnEvent(onTabControlMouseDown, s, a);
         importerTabControl.ClosePageButtonShowMode = ClosePageButtonShowMode.InAllTabPageHeaders;
         _contextMenuSelectedTab = "";
         sheetImportedFlag = false;
         allSheetsImportedFlag = false;
         btnImport.Enabled = false;
         btnImportAll.Enabled = false;
         btnImportAll.Text = Captions.Importer.LoadAllSheets;
         btnImport.Text = Captions.Importer.LoadCurrentSheet;
         allImportButtonsDisabledFlag = false;
         dataViewingGridView.OptionsBehavior.Editable = false;
         _imageListRetriever = imageListRetriever;
      }
      
      public override void InitializeResources()
      {
         base.InitializeResources();
         Caption = Captions.Importer.SourceTab;
         btnImport.InitWithImage(ApplicationIcons.Import, Captions.Importer.LoadCurrentSheet);
         btnImportAll.InitWithImage(ApplicationIcons.Import, Captions.Importer.LoadAllSheets);
         layoutItemImportAll.AdjustLargeButtonSize(rootLayoutControl);
         layoutItemImportCurrent.AdjustLargeButtonSize(rootLayoutControl);
         ApplicationIcon = ApplicationIcons.Excel;
         useForImportCheckEdit.ToolTip = Captions.Importer.UseFiltersForImportTooltip;
         useForImportCheckEdit.CheckedChanged += (s, a) => OnEvent(() => _dataPresenter.TriggerOnDataChanged());
         dataViewingGridView.ColumnFilterChanged += (s, a) => OnEvent(() => _dataPresenter.TriggerOnDataChanged());

         var customButton = new CustomHeaderButton(ButtonPredefines.Combo) { ToolTip = "Select Page" };
         importerTabControl.CustomHeaderButtons.Add(customButton);
         importerTabControl.CustomHeaderButtonClick += (s,a) => OnEvent(onTabControlCustomHeaderButtonClick);
         dataViewingGridView.OptionsView.ShowIndicator = false;
      }

      private void onTabControlCustomHeaderButtonClick()
      {
         var popupMenu = new DXPopupMenu { MenuViewType = MenuViewType.Menu };
         foreach (XtraTabPage page in importerTabControl.TabPages)
         {
            var menuitem = new DXMenuItem(page.Text);
            menuitem.Click += (s, a) => OnEvent(onPageListMenuItemClick, s, a);
            menuitem.Tag = popupMenu;
            if (page.Image != null)
               menuitem.Image = page.Image;
            popupMenu.Items.Add(menuitem);
         }
         var menuPos = importerTabControl.PointToClient(MousePosition);
         MenuManagerHelper.ShowMenu(popupMenu, importerTabControl.LookAndFeel, null, importerTabControl, menuPos);
      }


      private void onPageListMenuItemClick(object sender, EventArgs e)
      {
         var menuItem = sender as DXMenuItem;
         if (menuItem == null) return;
         foreach (XtraTabPage page in importerTabControl.TabPages)
         {
            if (page.Text != menuItem.Caption) continue;
            importerTabControl.SelectedTabPage = page;
            importerTabControl.MakePageVisible(page);
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

      public void AttachPresenter(IImporterDataPresenter dataPresenter)
      {
         _dataPresenter = dataPresenter;
      }

      public void EnableImportButtons()
      {
         allImportButtonsDisabledFlag = false;
         if (!sheetImportedFlag)
            btnImport.Enabled = true;
         if (!allSheetsImportedFlag)
            btnImportAll.Enabled = true;
      }

      public void DisableImportButtons()
      {
         allImportButtonsDisabledFlag = true;
         btnImport.Enabled = false;
         btnImportAll.Enabled = false;
      }

      private void onButtonImportAllClicked(object sender, EventArgs e)
      {
         _dataPresenter.ImportDataForConfirmation();
      }

      private void onTabControlMouseDown(object sender, MouseEventArgs e)
      {
         if (e.Button != MouseButtons.Right)
            return;
         XtraTabHitInfo hi = importerTabControl.CalcHitInfo(e.Location);
         if (hi.HitTest != XtraTabHitTest.PageHeader)
            return;

         var contextMenu = new DXPopupMenu();
         contextMenu.Items.Clear();
         contextMenu.Items.Add(new DXMenuItem(Captions.Importer.CloseAllTabsButThis, onCloseAllButThisTab));
         contextMenu.Items.Add(new DXMenuItem(Captions.Importer.CloseAllTabsToTheRight, onCloseAllTabsToTheRight));
         contextMenu.Items.Add(new DXMenuItem(Captions.Importer.ResetAllTabs, onReopenAllSheets));
         contextMenu.ShowPopup(importerTabControl, e.Location);
         _contextMenuSelectedTab = hi.Page.Text;

      }
      private void onCloseTab(object sender, EventArgs e)
      {
         //from DataSetControl.cs
         var eventArgs = e as ClosePageButtonEventArgs;
         if (eventArgs == null) return;
         var page = eventArgs.Page as XtraTabPage;
         if (page == null) return;
         //deleteTable(page);
         //importerTabControl.TabPages.Remove(page);
         _dataPresenter.RemoveTab(page.Text);
         _dataPresenter.RefreshTabs();
         hideCloseButtonForSingleTab();
      }

      private void onCloseAllButThisTab(object sender, EventArgs e)
      {
         _dataPresenter.RemoveAllButThisTab(_contextMenuSelectedTab);
      }

      private void onCloseAllTabsToTheRight(object sender, EventArgs e)
      {
         var removePage = false;
         foreach (XtraTabPage tabName in importerTabControl.TabPages)
         {
            if (removePage)
               _dataPresenter.RemoveTab(tabName.Text);

            if (tabName.Text == _contextMenuSelectedTab)
               removePage = true;
         }
         _dataPresenter.RefreshTabs();
      }

      private void onReopenAllSheets(object sender, EventArgs e)
      {
         _dataPresenter.ReopenAllSheets();
      }

      private void onButtonImportClicked(object sender, EventArgs e)
      {
         _dataPresenter.ImportDataForConfirmation(importerTabControl.SelectedTabPage.Text);
      }

      public void SelectTab(string tabName)
      {
         var tab = importerTabControl.TabPages.FirstOrDefault(x => x.Text == tabName);
         if (tab == null)
            return;

         var oldTab = importerTabControl.SelectedTabPage;
         importerTabControl.SelectedTabPage = tab;
         onSelectedPageChanged(this, new TabPageChangedEventArgs(oldTab, tab));
      }

      private void onSelectedPageChanged(object sender, TabPageChangedEventArgs e)
      {
         if (importerTabControl.SelectedTabPage == null) return;

         if (!_dataPresenter.SelectTab(e.Page.Text)) return;

         if (_dataPresenter.ImportedSheets.GetDataSheetNames().Contains(e.Page.Text))
            DisableImportCurrentSheet();
         else
            enableImportCurrentSheet();

         SelectedTab = e.Page.Text;
      }

      public void SetGridSource(string tabName = null)
      {
         dataViewingGridControl.DataSource = null;
         dataViewingGridView.Columns.Clear();

         if (tabName == null)
            tabName = _dataPresenter.GetSheetNames().ElementAt(0);

         dataViewingGridControl.DataSource = _dataPresenter.GetSheet(tabName);
      }

      public string GetActiveFilterCriteria()
      {
         return useForImportCheckEdit.Checked ? DevExpress.Data.Filtering.CriteriaToWhereClauseHelper.GetDataSetWhere(dataViewingGridView.ActiveFilterCriteria) : "";
      }

      public void AddTabs(List<string> sheetNames)
      {
         importerTabControl.Images = _imageListRetriever.AllImages16x16;
         foreach (var sheetName in sheetNames)
            importerTabControl.TabPages.Add(sheetName);

         hideCloseButtonForSingleTab();
         refreshErrorMarks();
      }

      private void hideCloseButtonForSingleTab()
      {
         if (importerTabControl.TabPages.Count == 1)
            importerTabControl.TabPages.FirstOrDefault().ShowCloseButton = DefaultBoolean.False;
      }

      public void ClearTabs()
      {
         importerTabControl.TabPages.Clear();
         _tabMarks.Clear();
      }

      public void DisableImportCurrentSheet()
      {
         btnImport.Enabled = false;
         btnImport.Text = Captions.Importer.SheetsAlreadyImported;
         sheetImportedFlag = true;
      }

      private void enableImportCurrentSheet()
      {
         if (allImportButtonsDisabledFlag) return;

         btnImport.Text = Captions.Importer.LoadCurrentSheet;
         btnImport.Enabled = true;
         sheetImportedFlag = false;
      }
      private void enableImportAllSheets()
      {
         if (allImportButtonsDisabledFlag) return;

         btnImportAll.Enabled = true;
         btnImportAll.Text = Captions.Importer.LoadAllSheets;
         allSheetsImportedFlag = false;
      }
      public void DisableImportAllSheets()
      {
         btnImportAll.Enabled = false;
         btnImportAll.Text = Captions.Importer.AllSheetsAlreadyImported;
         allSheetsImportedFlag = true;
      }

      public void ResetImportButtons()
      {
         enableImportCurrentSheet();
         enableImportAllSheets();
      }

      public void SetFilter(string filter)
      {
         if (string.IsNullOrEmpty(filter)) return;

         dataViewingGridView.ActiveFilterString = filter;
         useForImportCheckEdit.Checked = true;
      }

      private string _selectedTab;
      public string SelectedTab {
         get => _selectedTab;
         set
         {
            _selectedTab = value;
            refreshErrorMessage();
         }
      }
      public string GetFilter()
      {
         return DevExpress.Data.Filtering.CriteriaToWhereClauseHelper.GetDataSetWhere(dataViewingGridView.ActiveFilterCriteria);
      }

      private void refreshErrorMessage()
      {
         var tabMark = _tabMarks[SelectedTab];
         if (tabMark.ContainsError)
         {
            labelControlError.Text = tabMark.ErrorMessage;
            layoutControlItemError.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            return;
         }

         layoutControlItemError.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
      }

      private void refreshErrorMarks()
      {
         importerTabControl.TabPages.Each(x =>
         {
            var tabMark = _tabMarks[x.Text];
            if (!tabMark.IsLoaded)
            {
               x.ImageIndex = -1;
               return;
            }

            x.ImageIndex = tabMark.ContainsError ? _imageListRetriever.ImageIndex(ApplicationIcons.Cancel) : _imageListRetriever.ImageIndex(ApplicationIcons.OK);         
         });
      }

      public void SetTabMarks(Cache<string, TabMarkInfo> tabMarks)
      {
         _tabMarks = tabMarks;
         refreshErrorMessage();
         refreshErrorMarks();
      }
   }
}
