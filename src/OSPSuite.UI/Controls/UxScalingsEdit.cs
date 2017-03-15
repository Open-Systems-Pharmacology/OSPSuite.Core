using System.Collections.Generic;
using OSPSuite.Utility;
using DevExpress.XtraEditors;
using OSPSuite.Core.Domain;

namespace OSPSuite.UI.Controls
{
   public class UxScalingsEdit : ComboBoxEdit
   {
      public IEnumerable<Scalings> GetValidValues()
      {
         return EnumHelper.AllValuesFor<Scalings>();
      }
   }
}
