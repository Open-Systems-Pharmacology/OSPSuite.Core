using OSPSuite.UI.Controls;
using OSPSuite.Presentation.Importer.Presenters;
using OSPSuite.UI.Extensions;
using System.Collections.Generic;
using System;
using System.Data;
using DevExpress.XtraGrid;
using DevExpress.XtraTab;
using NPOI.OpenXmlFormats.Dml.Diagram;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Presentation.Importer.Services;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Container;

namespace OSPSuite.Presentation.Importer.Views
{
   public partial class ImporterView : BaseUserControl , IImporterView
   {
      private IImporterPresenter _presenter;

      public ImporterView()
      {
         InitializeComponent();
      }


      public void AttachPresenter(IImporterPresenter presenter)
      {
         _presenter = presenter;
         TabControl.SelectedPageChanged += onSelectedPageChanged;
      }

      private void onSelectedPageChanged(object sender, TabPageChangedEventArgs e) //actually do we need the event arguments here?
      {
         if (TabControl.SelectedTabPage != null) //not the best solution in the world this check here....
            OnTabChanged?.Invoke(TabControl.SelectedTabPage.Text);
      }

      public event TabChangedHandler OnTabChanged;

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
         OnFormatChanged?.Invoke(formatComboBoxEdit.EditValue as string);
      }


      public event FormatChangedHandler OnFormatChanged;

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