using System.Collections.Generic;
using OSPSuite.Utility;
using OSPSuite.Core.Chart;

namespace OSPSuite.UI.Controls
{
   public class UxLineTypeEdit : UxComboBoxEdit
   {
      public IEnumerable<LineStyles> GetValidValues()
      {
         return EnumHelper.AllValuesFor<LineStyles>();
      }
   }
}
