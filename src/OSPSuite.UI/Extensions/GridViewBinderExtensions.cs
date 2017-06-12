using System.Collections.Generic;
using OSPSuite.DataBinding.DevExpress.XtraGrid;

namespace OSPSuite.UI.Extensions
{
   public static class GridViewBinderExtensions
   {
      //from DevExpressDocumentation XtraGrid Adv... Grouping "Process Group Rows"
      public static IEnumerable<T> SelectedItems<T>(this GridViewBinder<T> gridViewBinder, int rowHandle)
      {
         var view = gridViewBinder.GridView;

         if (view.IsDataRow(rowHandle))
            yield return gridViewBinder.ElementAt(rowHandle);

         if (!view.IsGroupRow(rowHandle))
            yield break;

         for (int i = 0; i < view.GetChildRowCount(rowHandle); i++)
         {
            //Get the handle of a child row with the required index
            int childHandle = view.GetChildRowHandle(rowHandle, i);

            foreach (var selectedObject in SelectedItems(gridViewBinder, childHandle))
            {
               yield return selectedObject;
            }
         }
      }
   }
}