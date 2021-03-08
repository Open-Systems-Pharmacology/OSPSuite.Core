using System.Linq;
using DevExpress.XtraGrid.Views.Base;
using OSPSuite.Core.Chart;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.RepositoryItems;
using OSPSuite.Utility;

namespace OSPSuite.UI.Controls
{
   public class UxRepositoryItemLineStyles : UxRepositoryItemComboBox
   {
      public UxRepositoryItemLineStyles(BaseView view, bool removeLineStyleNone = false) : base(view)
      {
         var allLineStyles = EnumHelper.AllValuesFor<LineStyles>().ToList();
         if (removeLineStyleNone)
            allLineStyles.Remove(LineStyles.None);

         this.FillComboBoxRepositoryWith(allLineStyles);
      }
   }
}