using System;
using System.Linq;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Settings;
using OSPSuite.Presentation.Views.Charts;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.UI.Views.Charts
{
   internal class BaseUserControlWithColumnSettings : BaseUserControl, IViewWithColumnSettings
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
      }

      public void ApplyAllColumnSettings()
      {
         _presenter.AllColumnSettings.OrderBy(x => x.VisibleIndex).Each(ApplyColumnSettings);
         _gridView.CollapseAllGroups(); // otherwise an arbitrary group can be open 
      }

      public void ApplyColumnSettings(GridColumnSettings columnSettings)
      {
         var gridColumn = FindColumnByName(columnSettings.ColumnName);
         if (gridColumn == null)
            return;

         DoWithoutColumnSettingsUpdateNotification(() => columnSettings.ApplyTo(gridColumn));
      }

      /// <summary>
      ///    updates Visibility, VisibleIndex, GroupIndex in GridColumnSettings by values of corresponding GridColumn
      /// </summary>
      /// <param name="columnSettings"></param>
      private void updateColumnSettings(GridColumnSettings columnSettings)
      {
         var gridColumn = FindColumnByName(columnSettings.ColumnName);
         if (gridColumn == null) return;

         columnSettings.GroupIndex = gridColumn.GroupIndex;
         columnSettings.Visible = gridColumn.Visible;
         columnSettings.VisibleIndex = gridColumn.VisibleIndex;
         columnSettings.Width = gridColumn.Width;
      }

      private void onColumnSettingsChanged(object sender, EventArgs e)
      {
         _presenter.AllColumnSettings.Each(updateColumnSettings);
         _presenter.NotifyColumnSettingsChanged();
      }

      private void activateGridColumnChangedEventHandlers()
      {
         _gridView.ColumnPositionChanged += onColumnSettingsChanged;
         _gridView.ColumnWidthChanged += onColumnSettingsChanged;
         _gridView.EndGrouping += onColumnSettingsChanged;
      }

      private void deactivateGridColumnChangedEventHandlers()
      {
         _gridView.ColumnPositionChanged -= onColumnSettingsChanged;
         _gridView.ColumnWidthChanged -= onColumnSettingsChanged;
         _gridView.EndGrouping -= onColumnSettingsChanged;
      }

      protected void DoWithoutColumnSettingsUpdateNotification(Action action)
      {
         try
         {
            deactivateGridColumnChangedEventHandlers();
            action();
         }
         finally
         {
            activateGridColumnChangedEventHandlers();
         }
      }

      // to be defined in subclass (using GridColumn.FieldName for GridControl bind to DataTable or GridColumn.Tag when using BTS.DataBinding.GridViewBinder
      public virtual GridColumn FindColumnByName(string columnName)
      {
         // finding GridColumn by FieldName or Name is not possible here, because these properties are defined by BTS.DataBinding.GridViewBinder
         // therefore this workaround via Tag is used
         return _gridView.Columns.FirstOrDefault(col => Equals(col.Tag, columnName));
      }
   }
}