using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Presentation.Importer.Presenters;
using OSPSuite.Assets;
using OSPSuite.UI.Controls;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Importer.Views
{
   public partial class ImportConfirmationView : BaseUserControl, IImportConfirmationView
   {
      private IImportConfirmationPresenter _presenter;
      public ImportConfirmationView()
      {
         InitializeComponent();
         layoutControlItem1.Text = Captions.Importer.NamingPattern;
      }

      public void AttachPresenter(IImportConfirmationPresenter presenter)
      {
         _presenter = presenter;
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

      private void onNamingConventionChanged(object sender, EventArgs e)
      {
         this.DoWithinExceptionHandler( () => _presenter.TriggerNamingConventionChanged(namingConventionComboBoxEdit.EditValue as string));
      }
   }
}
