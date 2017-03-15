using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;
using DevExpress.XtraEditors;

namespace OSPSuite.UI.Extensions
{
   public static class MRUEditExtensions
   {
      public static void FillWith(this MRUEdit mruEdit, IEnumerable<string> availableItems)
      {
         mruEdit.Properties.Items.Clear();
         //Reverse to add them in the expected order
         availableItems.Reverse().Each(item => mruEdit.Properties.Items.Add(item));
      }
   }
}