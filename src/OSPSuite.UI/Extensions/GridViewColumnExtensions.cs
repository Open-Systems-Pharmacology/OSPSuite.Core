using OSPSuite.DataBinding.DevExpress.XtraGrid;

namespace OSPSuite.UI.Extensions
{
   public static class GridViewColumnExtensions
   {
      /// <summary>
      /// Set the visibility of the underlying XtraColumn defined in the <paramref name="column"/>.
      /// If the column is set to visible, the visible index will be set to the given index +1 (DevExpress visible index starts at 1)
      /// </summary>
      /// <param name="column">Grid view column</param>
      /// <param name="index0">0-based index used to set the visible index in the XtraColumn</param>
      /// <param name="visible">Is the column visible?</param>
      public static void UpdateVisibleIndex(this IGridViewColumn column, int index0, bool visible)
      {
         column.Visible = visible;
         column.WithShowInColumnChooser(visible);
         if (!visible) return;
      }
   }
}