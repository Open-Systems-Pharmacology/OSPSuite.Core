using System.Collections.Generic;

namespace OSPSuite.UI.Controls
{
   public class UxLineThicknessEdit : UxComboBoxEdit
   {
      private readonly int[] _validValues;

      public UxLineThicknessEdit()
      {
         _validValues = new[] {1, 2, 3};
      }

      public IEnumerable<int> GetValidValues()
      {
         return _validValues;
      }
   }
}