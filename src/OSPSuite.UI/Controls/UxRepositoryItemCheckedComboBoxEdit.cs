using System;
using OSPSuite.Utility.Extensions;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Base;

namespace OSPSuite.UI.Controls
{
   public class UxRepositoryItemCheckedComboBoxEdit : RepositoryItemCheckedComboBoxEdit
   {
      public UxRepositoryItemCheckedComboBoxEdit(BaseView view, Type enumType)
      {
         EditValueChanged += (EventHandler)((o, e) => view.PostEditor());

         // Populate the Items collection with all available flags.
         SetFlags(enumType);
         // Remove items that correspond to compound flags.
         removeCombinedFlags();
      }

      // Traverse through items and remove those that correspond to bitwise combinations of simple flags.
      private void removeCombinedFlags()
      {
         for (int i = Items.Count - 1; i > 0; i--)
         {
            var val1 = Items[i].Value.DowncastTo<Enum>();
            for (int j = i - 1; j >= 0; j--)
            {
               var val2 = Items[j].Value.DowncastTo<Enum>();
               if (val1.HasFlag(val2))
               {
                  Items.RemoveAt(i);
                  break;
               }
            }
         }
      }
   }
}
