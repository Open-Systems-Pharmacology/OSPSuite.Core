using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout.Utils;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.Importer;
using OSPSuite.Presentation.Views.ObservedData;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Extensions;

namespace OSPSuite.UI.Views.Importer
{
   public partial class ImportPreviewView : BaseUserControl, IImportPreviewView
   {
      private IImportPreviewPresenter _presenter;
      private bool _selectingDataSetsEnabled = true;
      private List<string> _namingConventionOptions = new List<string>();
      public string SelectedSeparator
      {
         get => separatorComboBoxEdit.SelectedText;
      }

      public bool SelectingDataSetsEnabled
      {
         set
         {
            _selectingDataSetsEnabled = value;
            importButton.Enabled = _selectingDataSetsEnabled;
            namesListBox.SelectionMode = _selectingDataSetsEnabled ? SelectionMode.One : SelectionMode.None;
            layoutControlItemError.Visibility = _selectingDataSetsEnabled ? LayoutVisibility.Never : LayoutVisibility.Always;
         }
         get => _selectingDataSetsEnabled;
      }

      public void SetErrorMessage(string errorMessage)
      {
         labelControlError.Text = errorMessage;
      }

      public ImportPreviewView()
      {
         InitializeComponent();

         namingConventionLayout.Text = Captions.Importer.NamingPattern.FormatForLabel();
         buttonAdd.Text = Captions.Importer.AddKeys;
         buttonAdd.Click += (s, a) => OnEvent(() =>
            namingConventionComboBoxEdit.EditValue += 
               (string.IsNullOrEmpty(namingConventionComboBoxEdit.EditValue.ToString()) ? "" : separatorComboBoxEdit.SelectedItem.ToString()) +
               string.Join(separatorComboBoxEdit.SelectedItem.ToString(), keysListBox.SelectedItems.Select(i => $"{{{i.ToString()}}}"))
         );
         importButton.Click += (s, a) => OnEvent(onButtonImportClick, s, a);
         namesListBox.SelectedIndexChanged += (s, a) => OnEvent(onDataSetNameSelected, s, a);
         keysListBox.SelectedIndexChanged += (s, a) => OnEvent(() => buttonAdd.Enabled = keysListBox.SelectedItems.Any());
         buttonAdd.Enabled = false;
         separatorComboBoxEdit.Properties.Items.Clear();
         
         separatorComboBoxEdit.Properties.Items.AddRange(Constants.ImporterConstants.NAMING_PATTERN_SEPARATORS);
         separatorComboBoxEdit.SelectedIndex = 0;
         separatorComboBoxEdit.TextChanged += (s, a) => OnEvent(() =>
         {
            setNamingConventions(
               _namingConventionOptions.Select(nc => Regex.Replace(nc, @"}[,.-_]*{", $"}}{separatorComboBoxEdit.SelectedText}{{")),
                  namingConventionComboBoxEdit.EditValue.ToString()
            );
         });
         namingConventionComboBoxEdit.TextChanged += (s, a) => OnEvent(onNamingConventionChanged, s, a);
         separatorControlItem.Text = Captions.Importer.Separator.FormatForLabel();
         namingElementLayoutControlItem.Text = Captions.Importer.NamingElement.FormatForLabel();
         namingPatternLayoutControlGroup.Text = Captions.Importer.CreateNamingPattern;
         dataSetsLayoutControlItem.Text =Captions.Importer.DataSets;
         //TODO WHY DO WE HAVE TWO LAYOUT CONTROLS IN ONE VIEW?
         buttonAddLayoutControlItem.AdjustButtonSize(layoutControl1);
         importButtonLayoutControlItem.AdjustButtonSize(layoutControl);
         namingPatternDropDownLabelControl.AsDescription();
         namingPatternDropDownLabelControl.Text = Captions.Importer.NamingPatternDescription.FormatForDescription();
         namingPatternDropDownLabelControl.AutoSizeMode = LabelAutoSizeMode.Vertical;
         namingPatternPanelLabelControl.AsDescription();
         namingPatternPanelLabelControl.Text = Captions.Importer.NamingPatternPanelDescription.FormatForDescription();
         namingPatternPanelLabelControl.AutoSizeMode = LabelAutoSizeMode.Vertical;
      }

      public void AttachPresenter(IImportPreviewPresenter presenter)
      {
         _presenter = presenter;
      }

      public void SetNamingConventions(IEnumerable<string> options, string selected = null)
      {
         _namingConventionOptions = options.ToList();
         setNamingConventions(_namingConventionOptions, selected);
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
         namesListBox.SetSelected(0, true);
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

         _presenter.DataSetSelected(listBox.SelectedIndex);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         Caption = Captions.Importer.ImportPreview;
         ApplicationIcon = ApplicationIcons.Parameter;
      }
   }
   
}
