using OSPSuite.UI.Controls;
using OSPSuite.Presentation.Importer.Presenters;
using OSPSuite.UI.Extensions;
using System.Collections.Generic;
using System;
using DevExpress.XtraTab;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Importer.Views
{
   public partial class ImporterView : BaseUserControl , IImporterView
   {
      private IImporterPresenter _presenter; //TODO - have to keep in mind if I need this at all
                                             //the architecture of these classes should actually be restructured

      public ImporterView()
      {
         InitializeComponent();
         this.btnImportAll.Click += (e, a) => this.DoWithinExceptionHandler(_presenter.ShowImportConfirmation);
      }


      public void AttachPresenter(IImporterPresenter presenter)
      {
         _presenter = presenter;
         TabControl.SelectedPageChanged += onSelectedPageChanged;
         btnImport.Click += onButtonImportClicked;
         btnImportAll.Click += onButtonImportAllClicked;
      }

      private void onButtonImportAllClicked(object sender, EventArgs e)
      {
         OnImportAllSheets.Invoke();
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
      public event FormatChangedHandler OnFormatChanged = delegate {};
      public event ImportSingleSheetHandler OnImportSingleSheet = delegate { };
      public event ImportAllSheetsHandler OnImportAllSheets = delegate { };
      
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