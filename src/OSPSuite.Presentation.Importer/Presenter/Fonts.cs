using System.Drawing;
using DevExpress.Utils;

namespace OSPSuite.Presentation.Presenter
{
   /// <summary>
   ///    Storage for application default font definitions
   /// </summary>
   public static class Fonts
   {
      /// <summary>
      ///    Use this font for tab headers on tabs that are not selected
      /// </summary>
      public static readonly Font NonSelectedTabHeaderFont = new Font(AppearanceObject.DefaultFont.Name, AppearanceObject.DefaultFont.Size, FontStyle.Regular);

      /// <summary>
      ///    Use this font for tab headers when the tab is selected
      /// </summary>
      public static readonly Font SelectedTabHeaderFont = new Font(AppearanceObject.DefaultFont.Name, AppearanceObject.DefaultFont.Size, FontStyle.Bold);
   }
}