using System;
using System.Drawing;
using OSPSuite.Utility.Exceptions;
using DevExpress.Utils;
using DevExpress.XtraPivotGrid;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Services;

namespace OSPSuite.UI.Controls
{
   public partial class PKAnalysisPivotGridControl : UxPivotGrid
   {
      protected PKAnalysisFormatter _formatter;
      public PivotGridField ParameterField { get; private set; }
      public PivotGridField ValueField { get; private set; }
      public IExceptionManager ExceptionManager { get; set; }

      public PKAnalysisPivotGridControl(IClipboardTask clipboardCopyTask) : base(clipboardCopyTask)
      {
         initializePKAnalysisPivotGridControl();
      }

      public PKAnalysisPivotGridControl()
      {
         initializePKAnalysisPivotGridControl();
      }

      private void initializePKAnalysisPivotGridControl()
      {
         InitializeComponent();
         initializeGrid();
         _formatter = new PKAnalysisFormatter();
      }

      private void initializeGrid()
      {
         CustomCellDisplayText += (o, e) => onEvent(() => onCustomCellDisplayText(e));
         CustomSummary += (o, e) => onEvent(() => onCustomSummary(e));
         CustomDrawCell += (o, e) => onEvent(() => onCustomDrawCell(e));
         FieldValueDisplayText += (o, e) => onEvent(() => onFieldValueDisplayText(e));

         TotalsVisible = false;
         ShowHeaders = false;
      }

      private void onFieldValueDisplayText(PivotFieldDisplayTextEventArgs e)
      {
         if (e.Field != ParameterField) return;
         e.DisplayText = _parameterDisplayFunc(e.Value.ToString());
      }

      private void onCustomDrawCell(PivotCustomDrawCellEventArgs e)
      {
         if (e.DataField != ValueField) return;
         if (e.Value == null)
            updateAppearanceBackColor(e.Appearance, Colors.Disabled);
      }

      private void updateAppearanceBackColor(AppearanceObject appearance, Color color)
      {
         appearance.BackColor = color;
         appearance.BackColor2 = color;
      }

      private void onCustomSummary(PivotGridCustomSummaryEventArgs e)
      {
         if (e.DataField != ValueField) return;
         // Get the record set corresponding to the current cell.
         var ds = e.CreateDrillDownDataSource();
         e.CustomValue = ds.Value<object>(ValueField);
      }

      private void onEvent(Action action)
      {
         if (ExceptionManager != null)
            ExceptionManager.Execute(action);
         else
            action();
      }

      private void onCustomCellDisplayText(PivotCellDisplayTextEventArgs e)
      {
         if (e.DataField != ValueField) return;
         e.DisplayText = _formatter.Format(e.Value);
      }

      public void AddParameterField(PivotGridField parameterField)
      {
         ParameterField = parameterField;
         AddField(ParameterField);
      }

      public void AddValueField(PivotGridField valueField)
      {
         ValueField = valueField;
         AddField(ValueField);
      }
   }
}