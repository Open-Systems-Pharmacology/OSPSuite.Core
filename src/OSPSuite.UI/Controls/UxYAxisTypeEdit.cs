using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility;
using DevExpress.XtraEditors;
using OSPSuite.Core.Chart;

namespace OSPSuite.UI.Controls
{
   public class UxYAxisTypeEdit : ComboBoxEdit
   {
      public IEnumerable<AxisTypes> GetValidValues()
      {
         return EnumHelper.AllValuesFor<AxisTypes>().Where(axisType => axisType != AxisTypes.X);
      }
   }
}
