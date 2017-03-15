using System;
using System.Linq;
using OSPSuite.Utility.Extensions;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Settings;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views.Charts
{
   internal class BaseUserControlWithColumnSettings : BaseUserControl
   {
      private IPresenterWithColumnSettings _presenter;
      private GridView _gridView;

      protected void InitializeWith(GridView gridView)
      {
         _gridView = gridView;
      }

      protected void AttachPresenter(IPresenterWithColumnSettings presenter)
      {
         _presenter = presenter;
         _presenter.ColumnSettingsChanged += onColumnSettingsChanged;
      }

      public void ApplyAllColumnSettings()
      {
         _presenter.AllColumnSettings().OrderBy(x => x.VisibleIndex).Each(applyColumnSettings);
         _gridView.CollapseAllGroups(); // otherwise an arbitrary group can be open (see MANTIS 2691)
      }

      /// <summary>
      ///    updates Visibility, VisibleIndex, GroupIndex in GridColumnSettings by values of corresponding GridColumn
      /// </summary>
      /// <param name="columnSettings"></param>
      private void updateColumnSettings(GridColumnSettings columnSettings)
      {
         var gridColumn = FindColumnByName(columnSettings.ColumnName);
         if (gridColumn == null) return;

         _presenter.ColumnSettingsChanged -= onColumnSettingsChanged;
         columnSettings.GroupIndex = gridColumn.GroupIndex;
         columnSettings.Visible = gridColumn.Visible; // triggers handler of ItemChanged of ColumnSettings
         columnSettings.VisibleIndex = gridColumn.VisibleIndex;
         columnSettings.Width = gridColumn.Width;
         _presenter.ColumnSettingsChanged += onColumnSettingsChanged;
      }

      private void onColumnSettingsChanged(object sender, EventArgs e)
      {
         updateAllColumnSettings();
      }

      private void updateAllColumnSettings()
      {
         foreach (var columnSettings in _presenter.AllColumnSettings())
            updateColumnSettings(columnSettings);
      }

      /// <summary>
      ///    applies Visibility, VisibleIndex, GroupIndex of GridColumnSettings to corresponding GridColumn
      /// </summary>
      /// <param name="columnSettings"></param>
      private void applyColumnSettings(GridColumnSettings columnSettings)
      {
         var gridColumn = FindColumnByName(columnSettings.ColumnName);
         if (gridColumn != null)
         {
            DeactivateGridColumnChangedEventHandlers();
            columnSettings.ApplyTo(gridColumn);
            ActivateGridColumnChangedEventHandlers();
         }
      }

      internal void ActivateGridColumnChangedEventHandlers()
      {
         _gridView.ColumnPositionChanged += onColumnSettingsChanged;
         _gridView.ColumnWidthChanged += onColumnSettingsChanged;
         _gridView.EndGrouping += onColumnSettingsChanged;
      }

      internal void DeactivateGridColumnChangedEventHandlers()
      {
         _gridView.ColumnPositionChanged -= onColumnSettingsChanged;
         _gridView.ColumnWidthChanged -= onColumnSettingsChanged;
         _gridView.EndGrouping -= onColumnSettingsChanged;
      }

      private void onColumnSettingsChanged(GridColumnSettings columnSettings)
      {
         if (columnSettings == null) return;
         applyColumnSettings(columnSettings);
      }

      // to be defined in subclass (using GridColumn.FieldName for GridControl bind to DataTable or GridColumn.Tag when using BTS.DataBinding.GridViewBinder
      public virtual GridColumn FindColumnByName(string columnName)
      {
         // finding GridColumn by FieldName or Name is not possible here, because these properties are defined by BTS.DataBinding.GridViewBinder
         // therefore this workaround via Tag is used
         return _gridView.Columns.Cast<GridColumn>().FirstOrDefault(col => col.Tag != null && col.Tag.ToString() == columnName);
      }
   }
}