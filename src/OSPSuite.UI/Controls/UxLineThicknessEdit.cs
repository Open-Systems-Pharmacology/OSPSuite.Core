using System.Collections.Generic;
using DevExpress.XtraEditors;

namespace OSPSuite.UI.Controls
{
   public class UxLineThicknessEdit : ComboBoxEdit
   {
      private readonly int[] _validValues;

      public UxLineThicknessEdit()
      {
         _validValues = new [] {1,2,3};
      }

      public IEnumerable<int> GetValidValues()
      {
         return _validValues;
      }
   }
}
