using System.Collections.Generic;
using OSPSuite.Utility;
using DevExpress.XtraEditors;
using OSPSuite.Core.Chart;

namespace OSPSuite.UI.Controls
{
   public class UxNumberModeEdit : ComboBoxEdit
   {
      public IEnumerable<NumberModes> GetValidValues()
      {
         return EnumHelper.AllValuesFor<NumberModes>();
      }
   }
}
