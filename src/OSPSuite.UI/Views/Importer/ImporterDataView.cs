using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.Utils.Menu;
using DevExpress.XtraTab;
using DevExpress.XtraTab.ViewInfo;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Views.Importer;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.UI.Views.Importer
{
   public partial class ImporterDataView : BaseUserControl, IImporterDataView
   {
      private IImporterDataPresenter _dataPresenter;

      private string _contextMenuSelectedTab;

      public ImporterDataView()
      {
         InitializeComponent();
         btnImport.Click += (s, a) => OnEvent(onButtonImportClicked, s, a);
         btnImportAll.Click += (s, a) => OnEvent(onButtonImportAllClicked, s, a);
         importerTabControl.SelectedPageChanged += (s, a) => OnEvent(onSelectedPageChanged, s, a);
         importerTabControl.CloseButtonClick += (s, a) => OnEvent(onCloseTab, s, a);
         importerTabControl.MouseDown += (s, a) => OnEvent(onTabControlMouseDown, s, a);
         _contextMenuSelectedTab = "";
         btnImport.Enabled = false;
         btnImportAll.Enabled = false;
         //nanLayoutControlItem.AdjustControlHeight(80);
      }

      public void AttachPresenter(IImporterDataPresenter dataPresenter)
      {
         _dataPresenter = dataPresenter;
      }

      public void EnableImportButtons()
      {
         btnImport.Enabled = true;
         btnImportAll.Enabled = true;
      }

      public void DisableImportButtons()
      {
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
         contextMenu.Items.Add(new DXMenuItem("close all tabs but this", onCloseAllButThisTab));
         contextMenu.Items.Add(new DXMenuItem("close all tabs to the right", onCloseAllTabsToTheRight));
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

      private void onSelectedPageChanged(object sender, TabPageChangedEventArgs e) //actually do we need the event arguments here?
      {
         if (importerTabControl.SelectedTabPage != null) //not the best solution in the world this check here....
            _dataPresenter.SelectTab(e.Page.Text);
      }

      public void AddSourceFileControl(ISourceFileControl sourceFileControl)
      {
         sourceFilePanelControl.FillWith(sourceFileControl);
      }

      public void AddDataViewingControl(IDataViewingControl dataViewingControl)
      {
         importerTabControl.FillWith(dataViewingControl);
      }

      public void AddNanView(INanView nanView)
      {
        // nanPanelControl.FillWith(nanView);
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
   }
}
