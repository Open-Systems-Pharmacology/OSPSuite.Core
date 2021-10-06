using DevExpress.XtraGrid.Views.Base;
using OSPSuite.Core.Chart;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.RepositoryItems;
using OSPSuite.Utility;

namespace OSPSuite.UI.Controls
{
   public class UxRepositoryItemSymbols : UxRepositoryItemComboBox
   {
      public UxRepositoryItemSymbols(BaseView view) : base(view)
      {
         this.FillComboBoxRepositoryWith(EnumHelper.AllValuesFor<Symbols>());
      }
   }

}