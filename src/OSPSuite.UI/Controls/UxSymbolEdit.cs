using System.Collections.Generic;
using OSPSuite.Utility;
using DevExpress.XtraEditors;
using OSPSuite.Core.Chart;

namespace OSPSuite.UI.Controls
{
   public class UxSymbolEdit : ComboBoxEdit
   {
      public IEnumerable<Symbols> GetValidValues()
      {
         return EnumHelper.AllValuesFor<Symbols>();
      }
   }
}
