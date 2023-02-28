using DevExpress.Utils;
using System.Drawing;

namespace OSPSuite.UI.Extensions
{
   public static class AppearanceObjectExtensions
   {
      public static void UpdateAppearanceColors(this AppearanceObject appearance, Color backGround, Color foreGround)
      {
         appearance.BackColor = backGround;
         appearance.BackColor2 = backGround;
         appearance.ForeColor = foreGround;
      }
   }
}
