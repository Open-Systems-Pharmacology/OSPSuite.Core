using System.Collections.Generic;
using System.Linq;
using OSPSuite.Presentation.Views.Importer;

namespace OSPSuite.UI.Views.Importer
{
   public partial class LloqEditorView : ILloqEditorView
   {
      public LloqEditorView()
      {
         InitializeComponent();
         ColumnsComboBox.EditValueChanged += (s, e) => _presenter.SetLloqColumn(ColumnsComboBox.EditValue.ToString());
      }
      public void FillComboBox(IEnumerable<string> columns, string defaultValue)
      {
         ColumnsComboBox.Properties.Items.Clear();
         ColumnsComboBox.Properties.Items.AddRange(columns.ToArray());
         ColumnsComboBox.EditValue = defaultValue;
      }
   }
}