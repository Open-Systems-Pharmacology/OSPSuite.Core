using OSPSuite.UI.Controls;
using OSPSuite.Presentation.Importer.Presenters;
using OSPSuite.UI.Extensions;
using System.Collections.Generic;
using System;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.Utils.Menu;
using DevExpress.XtraTab;
using DevExpress.XtraTab.ViewInfo;
using OSPSuite.Assets;

namespace OSPSuite.Presentation.Importer.Views
{
   public partial class ImporterView : BaseUserControl , IImporterView
   {
      private IImporterPresenter _presenter; //TODO - have to keep in mind if I need this at all the architecture of these classes should actually be restructured

      private string _contextMenuSelectedTab;

      public ImporterView()
      {
         InitializeComponent();
         btnImport.Click += onButtonImportClicked;
         btnImportAll.Click += onButtonImportAllClicked;
         TabControl.SelectedPageChanged += onSelectedPageChanged;
         TabControl.CloseButtonClick += onCloseTab;
         TabControl.MouseDown += onTabControlMouseDown;
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
         OnImportAllSheets.Invoke();
      }

      //still, we should not the context menu if we click at smthing that is not a page
      private void onTabControlMouseDown(object sender, MouseEventArgs e)
      {
         if (e.Button == MouseButtons.Right)
         {
            //this HERE DOES NOT REALLY WORK, hitTest returns none
            //XtraTabControl tabCtrl = sender as XtraTabControl;
            Point pt = MousePosition;
            //XtraTabHitInfo info = tabCtrl.CalcHitInfo(tabCtrl.PointToClient(pt));
            //if (info.HitTest == XtraTabHitTest.PageHeader)
            //{
            XtraTabHitInfo hi = TabControl.CalcHitInfo(e.Location);
            if (hi.HitTest == XtraTabHitTest.PageHeader)
            {

               var contextMenu = new DXPopupMenu();
               contextMenu.Items.Clear();
               contextMenu.Items.Add(new DXMenuItem("close all tabs but this", onCloseAllButThisTab));
               contextMenu.Items.Add(new DXMenuItem("close all tabs to the right", onCloseAllTabsToTheRight));
               contextMenu.ShowPopup(TabControl, e.Location);
               _contextMenuSelectedTab = hi.Page.Text;
            }

            //}
         }
      }
      private void onCloseTab(object sender, EventArgs e)
      {
         //from DataSetControl.cs
         var eventArgs = e as ClosePageButtonEventArgs;
         if (eventArgs == null) return;
         var page = eventArgs.Page as XtraTabPage;
         if (page == null) return;
         //deleteTable(page);
         //TabControl.TabPages.Remove(page);
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
         foreach (XtraTabPage tabName in TabControl.TabPages)
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
         OnImportSingleSheet.Invoke(TabControl.SelectedTabPage.Text);
      }

      private void onSelectedPageChanged(object sender, TabPageChangedEventArgs e) //actually do we need the event arguments here?
      {
         if (TabControl.SelectedTabPage != null) //not the best solution in the world this check here....
            OnTabChanged.Invoke(TabControl.SelectedTabPage.Text);
      }

      public event TabChangedHandler OnTabChanged = delegate {};
      public event ImportSingleSheetHandler OnImportSingleSheet = delegate { };
      public event ImportAllSheetsHandler OnImportAllSheets = delegate { };
      public event FormatChangedHandler OnFormatChanged = delegate {};

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
         TabControl.FillWith(dataViewingControl);
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
         OnFormatChanged.Invoke(formatComboBoxEdit.EditValue as string);
      }

      public void AddTabs(List<string> sheetNames)
      {
         //we should seek an alternative
         foreach (var sheetName in sheetNames)
         {
            TabControl.TabPages.Add(sheetName);
         }
      }

      public void ClearTabs()
      {
         for (var i= TabControl.TabPages.Count -1; i >= 0; i--)
         {
            TabControl.TabPages.RemoveAt(i);
         }
      }
   }
}