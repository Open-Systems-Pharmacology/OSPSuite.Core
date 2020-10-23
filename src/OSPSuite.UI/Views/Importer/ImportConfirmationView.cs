using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraEditors;
using OSPSuite.Assets;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Views.Importer;
using OSPSuite.UI.Controls;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.UI.Views.Importer
{
   public partial class ImportConfirmationView : BaseUserControl, IImportConfirmationView
   {
      private IImportConfirmationPresenter _presenter;

      public ImportConfirmationView()
      {
         InitializeComponent();
         namingConventionLayout.Text = Captions.Importer.NamingPattern;
         buttonAdd.Click += (s, a) => this.DoWithinExceptionHandler(() =>
            namingConventionComboBoxEdit.EditValue += String.Join(",", keysListBox.SelectedItems.Select(i => $"{{{i.ToString()}}}")));
         importButton.Click += onButtonImportClick;
         namesListBox.SelectedIndexChanged += onDataSetNameSelected;
         keysListBox.SelectedIndexChanged += (s, a) => buttonAdd.Enabled = keysListBox.SelectedItems.Any();
         buttonAdd.Enabled = false;
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
         namesListBox.Items.Clear();
         namesListBox.Items.AddRange(names.ToArray());
      }

      private void onButtonImportClick(object sender, EventArgs e)
      {
         _presenter.ImportData();
      }

      public void SetDataValues()
      { }

      private void onNamingConventionChanged(object sender, EventArgs e)
      {
         this.DoWithinExceptionHandler( () => _presenter.TriggerNamingConventionChanged(namingConventionComboBoxEdit.EditValue as string));
      }

      public void SetNamingConventionKeys(IEnumerable<string> keys)
      {
         keysListBox.Items.Clear();
         keysListBox.Items.AddRange(keys.ToArray());
      }

      private void onDataSetNameSelected(object sender, EventArgs eventArgs)
      {
         var listBox = sender as ListBoxControl;
         _presenter.DataSetToDataRepository(listBox.SelectedValue.ToString(), listBox.SelectedIndex );
      }
   }
}
