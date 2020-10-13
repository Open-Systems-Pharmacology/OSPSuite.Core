using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OSPSuite.Presentation.Importer.Presenters;
using DevExpress.XtraEditors;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.UnitSystem;
using DevExpress.Utils;
using DevExpress.XtraLayout;
using DevExpress.XtraEditors.Controls;
using System.Collections.Generic;
using OSPSuite.UI.Views;

namespace OSPSuite.Presentation.Importer.Views
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