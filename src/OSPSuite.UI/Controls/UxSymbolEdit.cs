using System.Collections.Generic;
using OSPSuite.Utility;
using OSPSuite.Core.Chart;

namespace OSPSuite.UI.Controls
{
   public class UxSymbolEdit : UxComboBoxEdit
   {
      public IEnumerable<Symbols> GetValidValues()
      {
         return EnumHelper.AllValuesFor<Symbols>();
      }
   }
}
