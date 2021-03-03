using System.Collections.Generic;
using OSPSuite.Utility;
using OSPSuite.Core.Chart;

namespace OSPSuite.UI.Controls
{
   public class UxNumberModeEdit : UxComboBoxEdit
   {
      public IEnumerable<NumberModes> GetValidValues()
      {
         return EnumHelper.AllValuesFor<NumberModes>();
      }
   }
}
