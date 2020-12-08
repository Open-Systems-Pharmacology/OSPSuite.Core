using DevExpress.DataProcessing;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.UI.Views.Importer
{
   public partial class OptionsEditorView : BaseUserControl, IOptionsEditorView
   {
      public OptionsEditorView()
      {
         InitializeComponent();
      }

      public void AttachPresenter(IOptionsEditorPresenter presenter)
      {
      }

      public void SetOptions(IReadOnlyDictionary<string, IEnumerable<string>> options, string selected = null)
      {
         _comboBoxEdit.Properties.Items.Clear();
         var list = options.SelectMany(o => o.Value).ToArray();
         _comboBoxEdit.Properties.Items.AddRange(list);
         _comboBoxEdit.SelectedIndexChanged += (s, a) => OnEvent(() => OnOptionChanged.Invoke(s, new OptionChangedEventArgs() { Index = _comboBoxEdit.SelectedIndex, Text = _comboBoxEdit.Text }));
         if (string.IsNullOrEmpty(selected))
            _comboBoxEdit.SelectedIndex = 0;
         else
            _comboBoxEdit.SelectedIndex = list.IndexOf(list.FirstOrDefault(o => o == selected));
      }

      public void Clear()
      {
         _comboBoxEdit.Properties.Items.Clear();
         _comboBoxEdit.SelectedText = "";
      }

      public event EventHandler<OptionChangedEventArgs> OnOptionChanged = delegate { };
   }
}
