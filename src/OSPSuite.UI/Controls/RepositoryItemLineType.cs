using System;
using DevExpress.XtraEditors.Repository;
using OSPSuite.Core.Chart;

namespace OSPSuite.UI.Controls
{
   public class RepositoryItemLineType : RepositoryItemComboBox
   {
      public RepositoryItemLineType()
      {
         Items.AddRange(Enum.GetValues(typeof(LineStyles)));
      }
   }
}
