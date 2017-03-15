using DevExpress.XtraGrid.Columns;
using OSPSuite.Presentation.Settings;

namespace OSPSuite.UI.Extensions
{
   public static class GridColumnSettingsExtensions
   {
      public static void ApplyTo(this GridColumnSettings columnSettings,GridColumn gridColumn)
      {
         gridColumn.Visible = columnSettings.Visible;
         gridColumn.Caption = columnSettings.Caption;
         gridColumn.GroupIndex = columnSettings.GroupIndex;

         if (columnSettings.Visible && columnSettings.VisibleIndex != GridColumnSettings.INDEX_NOT_DEFINED)
            gridColumn.VisibleIndex = columnSettings.VisibleIndex;
         
         gridColumn.Width = columnSettings.Width;

         if (!string.IsNullOrEmpty(columnSettings.SortColumnName) && gridColumn.View.Columns.ColumnByFieldName(columnSettings.SortColumnName) != null)
            gridColumn.FieldNameSortGroup = columnSettings.SortColumnName;
      }
   }
}