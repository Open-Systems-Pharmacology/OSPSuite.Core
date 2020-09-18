using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OSPSuite.UI.Views;
using OSPSuite.Presentation.Importer.Presenters;
using OSPSuite.Assets;
using OSPSuite.UI.Controls;

namespace OSPSuite.Presentation.Importer.Views
{
   public partial class ImportConfirmationView : BaseUserControl, IImportConfirmationView
   {
      public ImportConfirmationView()
      {
         InitializeComponent();
         layoutControlItem1.Text = Captions.Importer.NamingPattern;
         listBoxControl1.TextChanged += (s, v) => OnSelectedDataSetChanged.Invoke(listBoxControl1.SelectedIndex);
      }

      public void AttachPresenter(IImportConfirmationPresenter presenter)
      {
      }

      public void SetNamingConventions(IEnumerable<string> options, string selected = null)
      {
         namingConventionComboBoxEdit.Properties.Items.Clear();
         namingConventionComboBoxEdit.Properties.Items.AddRange(options.ToArray());
         if (selected != null)
         {
            namingConventionComboBoxEdit.EditValue = selected;
         }
         else
         {
            namingConventionComboBoxEdit.EditValue = options.First();
         }
         namingConventionComboBoxEdit.TextChanged += onNamingConventionChanged;
      }

      public void SetDataSetNames(IEnumerable<string> names)
      {
         listBoxControl1.Items.Clear();
         listBoxControl1.Items.AddRange(names.ToArray());
      }

      public void SetDataValues()
      { }

      public event NamingConventionChangedHandler OnNamingConventionChanged = delegate { };
      public event SelectedDataSetChangedHandler OnSelectedDataSetChanged = delegate { };

      private void onNamingConventionChanged(object sender, EventArgs e)
      {
         OnNamingConventionChanged.Invoke(namingConventionComboBoxEdit.EditValue as string);
      }
   }
}
