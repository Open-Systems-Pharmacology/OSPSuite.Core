using OSPSuite.UI.Controls;
using OSPSuite.Presentation.Importer.Presenters;
using OSPSuite.UI.Extensions;
using System.Collections.Generic;
using System;

namespace OSPSuite.Presentation.Importer.Views
{
   public partial class ImporterView : BaseUserControl , IImporterView
   {
      private IImporterPresenter _presenter;
      public ImporterView()
      {
         InitializeComponent();
      }

      public void AddColumnMappingControl(IColumnMappingControl columnMappingControl)
      {
         columnMappingPanelControl.FillWith(columnMappingControl);
      }

      public void AddDataViewingControl(IDataViewingControl dataViewingControl)
      {
         dataViewingPanelControl.FillWith(dataViewingControl);
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

      public void AttachPresenter(IImporterPresenter presenter)
      {
         _presenter = presenter;
      }
   }
}