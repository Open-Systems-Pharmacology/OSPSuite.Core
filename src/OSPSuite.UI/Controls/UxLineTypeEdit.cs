using System.Collections.Generic;
using OSPSuite.Utility;
using DevExpress.XtraEditors;
using OSPSuite.Core.Chart;

namespace OSPSuite.UI.Controls
{
   public class UxLineTypeEdit : ComboBoxEdit
   {
      public IEnumerable<LineStyles> GetValidValues()
      {
         return EnumHelper.AllValuesFor<LineStyles>();
      }
   }
}
