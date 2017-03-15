using System.Windows.Forms;
using DevExpress.XtraEditors;
using OSPSuite.Presentation.Regions;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Controls
{
   public class UxPanelControl : PanelControl, IRegion
   {
      public void Add(object view)
      {
         this.FillWith(view as Control);
      }

      public void ToggleVisibility()
      {
         //nothing to do here*/
      }

      public bool IsVisible => true;
   }
}