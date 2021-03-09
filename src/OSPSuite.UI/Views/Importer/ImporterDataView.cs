using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.Utils.Menu;
using DevExpress.XtraTab;
using DevExpress.XtraTab.ViewInfo;
using OSPSuite.Assets;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Views.Importer;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views.Importer
{
   public partial class ImporterDataView : BaseUserControl, IImporterDataView
   {
      private IImporterDataPresenter _dataPresenter;

      private string _contextMenuSelectedTab;
      private bool sheetImportedFlag;
      private bool allSheetsImportedFlag;
      private bool allImportButtonsDisabledFlag;

      public ImporterDataView()
      {
         InitializeComponent();
         btnImport.Click += (s, a) => OnEvent(onButtonImportClicked, s, a);
         btnImportAll.Click += (s, a) => OnEvent(onButtonImportAllClicked, s, a);
         importerTabControl.FillWith(dataViewingGridControl);
         importerTabControl.SelectedPageChanged += (s, a) => OnEvent(onSelectedPageChanged, s, a);
         importerTabControl.CloseButtonClick += (s, a) => OnEvent(onCloseTab, s, a);
         importerTabControl.MouseDown += (s, a) => OnEvent(onTabControlMouseDown, s, a);
         _contextMenuSelectedTab = "";
         sheetImportedFlag = false;
         allSheetsImportedFlag = false;
         btnImport.Enabled = false;
         btnImportAll.Enabled = false;
         btnImportAll.Text = Captions.Importer.LoadAllSheets;
         btnImport.Text = Captions.Importer.LoadCurrentSheet;
         allImportButtonsDisabledFlag = false;
         dataViewingGridView.OptionsBehavior.Editable = false;
      }
      
      public override void InitializeResources()
      {
         base.InitializeResources();
         Caption = Captions.Importer.SourceTab;
         btnImport.InitWithImage(ApplicationIcons.Import, Captions.Importer.LoadCurrentSheet);
         btnImportAll.InitWithImage(ApplicationIcons.Import, Captions.Importer.LoadAllSheets);
         layoutItemImportAll.AdjustLargeButtonSize();
         layoutItemImportCurrent.AdjustLargeButtonSize();
         ApplicationIcon = ApplicationIcons.Excel;
         useForImportCheckEdit.CheckedChanged += (s, a) => OnEvent(() => _dataPresenter.TriggerOnDataChanged());
         dataViewingGridView.ColumnFilterChanged += (s, a) => OnEvent(() => _dataPresenter.TriggerOnDataChanged());

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

      private void onButtonImportClicked(object sender, EventArgs e)
      {
         _dataPresenter.ImportDataForConfirmation(importerTabControl.SelectedTabPage.Text);
      }

      private void onSelectedPageChanged(object sender, TabPageChangedEventArgs e)
      {
         if (importerTabControl.SelectedTabPage == null) return;

         if (_dataPresenter.Sheets.Keys.Contains(e.Page.Text))
            DisableImportCurrentSheet();
         else
            enableImportCurrentSheet();

         _dataPresenter.SelectTab(e.Page.Text);
         SelectedTab = e.Page.Text;
      }

      public void SetGridSource(string tabName = null)
      {
         allImportButtonsDisabledFlag = false;

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
         //we should seek an alternative
         foreach (var sheetName in sheetNames)
         {
            importerTabControl.TabPages.Add(sheetName);
         }
      }

      public void ClearTabs()
      {
         importerTabControl.TabPages.Clear();
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
         dataViewingGridView.ActiveFilterString = filter;
      }

      public string SelectedTab { get; set; }
   }
}
