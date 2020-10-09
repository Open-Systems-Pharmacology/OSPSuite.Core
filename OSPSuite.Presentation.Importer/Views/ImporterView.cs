using OSPSuite.UI.Controls;
using OSPSuite.Presentation.Importer.Presenters;
using OSPSuite.UI.Extensions;
using System.Collections.Generic;
using System;
using System.Windows.Forms;
using DevExpress.Utils.Menu;
using DevExpress.XtraTab;
using DevExpress.XtraTab.ViewInfo;


namespace OSPSuite.Presentation.Importer.Views
{
   public partial class ImporterView : BaseUserControl , IImporterView
   {
      private IImporterPresenter _presenter;

      private string _contextMenuSelectedTab;

      public ImporterView()
      {
         InitializeComponent();
         btnImport.Click += onButtonImportClicked;
         btnImportAll.Click += onButtonImportAllClicked;
         ImporterTabControl.SelectedPageChanged += onSelectedPageChanged;
         ImporterTabControl.CloseButtonClick += onCloseTab;
         ImporterTabControl.MouseDown += onTabControlMouseDown;
         _contextMenuSelectedTab = "";
      }


      public void AttachPresenter(IImporterPresenter presenter)
      {
         _presenter = presenter;
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
         _presenter.ImportDataForConfirmation();
      }

      private void onTabControlMouseDown(object sender, MouseEventArgs e)
      {
         if (e.Button != MouseButtons.Right)
            return;
         XtraTabHitInfo hi = ImporterTabControl.CalcHitInfo(e.Location);
         if (hi.HitTest != XtraTabHitTest.PageHeader)
            return;
         
         var contextMenu = new DXPopupMenu();
         contextMenu.Items.Clear();
         contextMenu.Items.Add(new DXMenuItem("close all tabs but this", onCloseAllButThisTab));
         contextMenu.Items.Add(new DXMenuItem("close all tabs to the right", onCloseAllTabsToTheRight));
         contextMenu.ShowPopup(ImporterTabControl, e.Location);
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
         //ImporterTabControl.TabPages.Remove(page);
         _presenter.RemoveTab(page.Text);
         _presenter.RefreshTabs();
      }

      private void onCloseAllButThisTab(object sender, EventArgs e)
      {
         _presenter.RemoveAllButThisTab(_contextMenuSelectedTab);
      }

      private void onCloseAllTabsToTheRight(object sender, EventArgs e)
      {
         var removePage = false;
         foreach (XtraTabPage tabName in ImporterTabControl.TabPages)
         {
            if (removePage)
               _presenter.RemoveTab(tabName.Text);

            if (tabName.Text == _contextMenuSelectedTab)
               removePage = true;
         }
         _presenter.RefreshTabs();
      }

      private void onButtonImportClicked(object sender, EventArgs e)
      {
         _presenter.ImportDataForConfirmation(ImporterTabControl.SelectedTabPage.Text);
      }

      private void onSelectedPageChanged(object sender, TabPageChangedEventArgs e) //actually do we need the event arguments here?
      {
         if (ImporterTabControl.SelectedTabPage != null) //not the best solution in the world this check here....
            _presenter.SelectTab(e.Page.Text);
      }

      public void AddColumnMappingControl(IColumnMappingControl columnMappingControl)
      {
         columnMappingPanelControl.FillWith(columnMappingControl);
      }

      public void AddSourceFileControl(ISourceFileControl sourceFileControl)
      {
         sourceFilePanelControl.FillWith(sourceFileControl);
      }

      public void AddDataViewingControl(IDataViewingControl dataViewingControl)
      {
         ImporterTabControl.FillWith(dataViewingControl);
      }

      public void SetFormats(IEnumerable<string> options, string selected)
      {
         formatComboBoxEdit.Properties.Items.Clear();
         foreach (var option in options)
         {
            formatComboBoxEdit.Properties.Items.Add(option);
         }
         formatComboBoxEdit.EditValue = selected;
         formatComboBoxEdit.TextChanged += onFormatChanged;
      }

      private void onFormatChanged(object sender, EventArgs e)
      {
         _presenter.SetNewFormat(formatComboBoxEdit.EditValue as string);
      }

      public void AddTabs(List<string> sheetNames)
      {
         //we should seek an alternative
         foreach (var sheetName in sheetNames)
         {
            ImporterTabControl.TabPages.Add(sheetName);
         }
      }

      public void ClearTabs()
      {
         ImporterTabControl.TabPages.Clear();
      }
   }
}