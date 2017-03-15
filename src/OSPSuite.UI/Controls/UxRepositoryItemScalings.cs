using OSPSuite.Utility;
using DevExpress.XtraGrid.Views.Base;
using OSPSuite.Core.Domain;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.RepositoryItems;

namespace OSPSuite.UI.Controls
{
   public class UxRepositoryItemScalings : UxRepositoryItemComboBox
   {
      public UxRepositoryItemScalings(BaseView view) : base(view)
      {
         this.FillComboBoxRepositoryWith(EnumHelper.AllValuesFor<Scalings>());
      }
   }
}