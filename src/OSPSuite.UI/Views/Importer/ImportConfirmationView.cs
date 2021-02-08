using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.Charts;
using OSPSuite.Presentation.Views.Importer;
using OSPSuite.Presentation.Views.ObservedData;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.UI.Views.Importer
{
   public partial class ImportConfirmationView : BaseUserControl, IImportConfirmationView
   {
      private IImportConfirmationPresenter _presenter;
      private List<string> _namingConventionOptions = new List<string>();

      public ImportConfirmationView()
      {
         InitializeComponent();

         namingConventionLayout.Text = Captions.Importer.NamingPattern;
         buttonAdd.Click += (s, a) => OnEvent(() =>
            namingConventionComboBoxEdit.EditValue += String.Join(separatorComboBoxEdit.SelectedItem.ToString(), keysListBox.SelectedItems.Select(i => $"{{{i.ToString()}}}"))
         );
         importButton.Click += (s, a) => OnEvent(onButtonImportClick, s, a);
         namesListBox.SelectedIndexChanged += (s, a) => OnEvent(onDataSetNameSelected, s, a);
         keysListBox.SelectedIndexChanged += (s, a) => OnEvent(() => buttonAdd.Enabled = keysListBox.SelectedItems.Any());
         buttonAdd.Enabled = false;
         separatorComboBoxEdit.Properties.Items.Clear();
         separatorComboBoxEdit.Properties.Items.AddRange(new [] {".", ",", "-", "_"}); //TODO: Bring the values from some configuration??
         separatorComboBoxEdit.SelectedIndex = 0;
         separatorComboBoxEdit.TextChanged += (s, a) => OnEvent(() =>
         {
            setNamingConventions(
               _namingConventionOptions.Select(nc => Regex.Replace(nc, @"}[,.-_]*{", $"}}{separatorComboBoxEdit.SelectedText}{{")),
                  namingConventionComboBoxEdit.EditValue.ToString()
            );
         });
         separatorControlItem.Text = Captions.Importer.Separator;
         buttonAddLayoutControlItem.AdjustButtonSize();
         importButtonLayoutControlItem.AdjustButtonSize();
      }

      public void AttachPresenter(IImportConfirmationPresenter presenter)
      {
         _presenter = presenter;
      }



      public void SetNamingConventions(IEnumerable<string> options, string selected = null)
      {
         _namingConventionOptions = options.ToList();
         setNamingConventions(_namingConventionOptions, selected);
         namingConventionComboBoxEdit.TextChanged += (s, a) => OnEvent(onNamingConventionChanged, s, a);
      }

      private void setNamingConventions(IEnumerable<string> options, string selected)
      {
         namingConventionComboBoxEdit.Properties.Items.Clear();
         namingConventionComboBoxEdit.Properties.Items.AddRange(options.ToArray());
         if (selected != null)
         {
            namingConventionComboBoxEdit.EditValue = selected;
         }
         else
         {
            namingConventionComboBoxEdit.EditValue = _namingConventionOptions.First();
         }
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

      public void ShowSelectedDataSet(DataRepository dataRepository)
      {
      }

      public void AddChartView(IView chartView)
      {
         chartPanelControl.FillWith(chartView);
      }

      public void AddDataView(IDataRepositoryDataView dataView)
      {
         dataPanelControl.FillWith(dataView);
      }

      private void onDataSetNameSelected(object sender, EventArgs eventArgs)
      {
         var listBox = sender as ListBoxControl;

         if (listBox.SelectedValue == null) return;  //here if null we have to empty the GUI

         _presenter.DataSetSelected(listBox.SelectedValue.ToString(), listBox.SelectedIndex);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         Caption = Captions.Importer.Confirmation;
         ApplicationIcon = ApplicationIcons.Parameter;
      }
   }
}
