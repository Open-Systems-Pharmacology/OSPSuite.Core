using System;
using System.Data;
using System.Windows.Forms;
using OSPSuite.Utility.Extensions;
using DevExpress.Data.PivotGrid;
using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraPivotGrid;
using OSPSuite.Assets;
using OSPSuite.UI.Mappers;
using OSPSuite.UI.Services;

namespace OSPSuite.UI.Controls
{
   public class UxPivotGrid : PivotGridControl
   {
      protected PivotGridCellsToDataTableMapper _pivotGridCellsToDataTableMapper;
      private readonly IClipboardTask _clipboardCopyTask;
      protected Func<string, string> _parameterDisplayFunc;

      public UxPivotGrid(IClipboardTask clipboardCopyTask)
      {
         TotalsVisible = false;
         ShowHeaders = false;
         _pivotGridCellsToDataTableMapper = new PivotGridCellsToDataTableMapper();
         _clipboardCopyTask = clipboardCopyTask;
         OptionsSelection.MultiSelect = false;
         PopupMenuShowing += (o, e) => this.DoWithinExceptionHandler(() => onPopupMenuShowing(o, e));
         _parameterDisplayFunc = s => s;
      }

      public UxPivotGrid() : this(new ClipboardTask())
      {
      }

      public PivotGridField CreateDataAreaNamed(string name)
      {
         return new PivotGridField(name, PivotArea.DataArea) {SummaryType = PivotSummaryType.Max};
      }

      public PivotGridField CreateRowAreaNamed(string name)
      {
         var field = new PivotGridField(name, PivotArea.RowArea);
         field.Options.AllowExpand = DefaultBoolean.False;
         return field;
      }

      public PivotGridField CreateColumnAreaNamed(string name)
      {
         var field = new PivotGridField(name, PivotArea.ColumnArea);
         field.Options.AllowExpand = DefaultBoolean.False;
         return field;
      }

      public PivotGridField AddField(PivotGridField field)
      {
         Fields.Add(field);
         return field;
      }

      public bool TotalsVisible
      {
         set
         {
            OptionsView.ShowColumnTotals = value;
            OptionsView.ShowColumnGrandTotals = value;
            OptionsView.ShowColumnGrandTotalHeader = value;
            OptionsView.ShowCustomTotalsForSingleValues = value;
            OptionsView.ShowRowGrandTotals = value;
            OptionsView.ShowRowTotals = value;
         }
      }

      public bool ShowHeaders
      {
         set
         {
            OptionsView.ShowColumnHeaders = value;
            OptionsView.ShowDataHeaders = value;
            OptionsView.ShowFilterHeaders = value;
            OptionsView.ShowRowHeaders = value;
            OptionsView.ShowFilterSeparatorBar = value;
         }
      }

      protected override void OnKeyDown(KeyEventArgs e)
      {
         base.OnKeyDown(e);
         this.DoWithinExceptionHandler(() =>
         {
            if (e.KeyCode != Keys.C || !e.Control) return;

            if (e.Shift)
               copyAllCellsToClipboard();
            else
               copySelectionToClipboard();
         });
      }

      private void onPopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
      {
         if (e.Menu == null)
            return;

         if (e.MenuType != PivotGridMenuType.HeaderArea)
            return;

         var copySelectionMenu = new DXMenuItem(Captions.CopySelection, clickCopySelectionMenuItem, ApplicationIcons.CopySelection) {Shortcut = Shortcut.CtrlC};
         var copyAllMenu = new DXMenuItem(Captions.CopyTable, clickCopyTableMenuItem, ApplicationIcons.Copy) {Shortcut = Shortcut.CtrlShiftC};

         e.Menu.Items.Clear();
         e.Menu.Items.Insert(0, copySelectionMenu);
         e.Menu.Items.Insert(1, copyAllMenu);
      }

      private void clickCopyTableMenuItem(object sender, EventArgs e)
      {
         this.DoWithinExceptionHandler(copyAllCellsToClipboard);
      }

      private void clickCopySelectionMenuItem(object sender, EventArgs e)
      {
         this.DoWithinExceptionHandler(copySelectionToClipboard);
      }

      private void copySelectionToClipboard()
      {
         var dataTable = _pivotGridCellsToDataTableMapper.MapSelectionFrom(Cells, _parameterDisplayFunc, outputDisplayFormatter);
         Clipboard.SetDataObject(_clipboardCopyTask.CreateDataObject(dataTable));
      }

      private void copyAllCellsToClipboard()
      {
         var dataTable = _pivotGridCellsToDataTableMapper.MapFrom(Cells, _parameterDisplayFunc, outputDisplayFormatter);

         Clipboard.SetDataObject(_clipboardCopyTask.CreateDataObject(dataTable));
      }

      private static object outputDisplayFormatter(PivotCellEventArgs args)
      {
         double outValue;
         if (double.TryParse(args.DisplayText, out outValue))
         {
            return args.DisplayText;
         }

         return double.NaN;
      }

      public DataTable GetCellsSummary()
      {
         return _pivotGridCellsToDataTableMapper.MapFrom(Cells, _parameterDisplayFunc);
      }

      public void SetParameterDisplay(Func<string, string> parameterDisplayFunc)
      {
         _parameterDisplayFunc = parameterDisplayFunc;
      }
   }
}