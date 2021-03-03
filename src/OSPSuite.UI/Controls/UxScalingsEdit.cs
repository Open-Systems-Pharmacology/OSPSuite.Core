using System.Collections.Generic;
using OSPSuite.Utility;
using OSPSuite.Core.Domain;

namespace OSPSuite.UI.Controls
{
   public class UxScalingsEdit : UxComboBoxEdit
   {
      public IEnumerable<Scalings> GetValidValues()
      {
         return EnumHelper.AllValuesFor<Scalings>();
      }
   }
}
