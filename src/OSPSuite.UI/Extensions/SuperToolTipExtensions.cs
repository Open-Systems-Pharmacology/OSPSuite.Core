using DevExpress.Utils;

namespace OSPSuite.UI.Extensions
{
   public static class SuperToolTipExtensions
   {
      /// <summary>
      /// Add a title to the super tool tip
      /// </summary>
      public static SuperToolTip WithTitle(this SuperToolTip superToolTip, string title)
      {
         superToolTip.Items.AddTitle(title);
         return superToolTip;
      }

      /// <summary>
      /// Add the text in the item list of the super tool tip
      /// </summary>
      public static SuperToolTip WithText(this SuperToolTip superToolTip, string text)
      {
         superToolTip.Items.Add(text);
         return superToolTip;
      }
   }
}