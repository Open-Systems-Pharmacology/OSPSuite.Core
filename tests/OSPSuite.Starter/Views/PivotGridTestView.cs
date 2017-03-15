using System;
using System.Data;
using DevExpress.Data.PivotGrid;
using DevExpress.Utils;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraPivotGrid;
using OSPSuite.Starter.Presenters;
using OSPSuite.UI.Controls;

namespace OSPSuite.Starter.Views
{
   public partial class PivotGridTestView : BaseUserControl, IPivotGridTestView
   {
      private PivotGridField _valueField;

      public PivotGridTestView()
      {
         InitializeComponent();
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         pivotGridControl.EditValueChanged += (sender, args) => OnEvent(() => handleEditorChanged(args));
      }

      private void handleEditorChanged(EditValueChangedEventArgs e)
      {
         var ds = e.CreateDrillDownDataSource();
         for (int i = 0; i < ds.RowCount; i++)
         {
            ds.SetValue(i, _valueField, Convert.ToBoolean(e.Editor.EditValue));
         }
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         pivotGridControl.TotalsVisible = false;
         pivotGridControl.ShowHeaders = false;
         pivotGridControl.OptionsSelection.MultiSelect = false;


         var compoundName = new PivotGridField("Compound", PivotArea.RowArea);
         compoundName.Options.AllowExpand = DefaultBoolean.False;
         var curveField = new PivotGridField("Calculation Method", PivotArea.ColumnArea);
         curveField.Options.AllowExpand = DefaultBoolean.False;
         var categoryField = new PivotGridField("Category", PivotArea.ColumnArea);
         categoryField.Options.AllowExpand = DefaultBoolean.False;
         _valueField = new PivotGridField("Value", PivotArea.DataArea) { SummaryType = PivotSummaryType.Max };
         
         pivotGridControl.Fields.Add(_valueField);
         pivotGridControl.Fields.Add(compoundName);
         pivotGridControl.Fields.Add(categoryField);
         pivotGridControl.Fields.Add(curveField);

         var repositoryItemCheckEdit = new RepositoryItemCheckEdit();

         pivotGridControl.RepositoryItems.Add(repositoryItemCheckEdit);
         pivotGridControl.OptionsCustomization.AllowEdit = true;
         _valueField.FieldEdit = repositoryItemCheckEdit;
         _valueField.Options.AllowEdit = true;
      }

      public void AttachPresenter(IPivotGridTestPresenter presenter)
      {
      }

      public void BindTo(DataTable dataTable)
      {
         pivotGridControl.DataSource = dataTable;
         
         pivotGridControl.BestFitRowArea();
      }
   }
}
