using System.Drawing;
using DevExpress.XtraEditors;

namespace OSPSuite.UI.Extensions
{
   public static class LabelControlExtensions
   {
      public static void AsFullViewText(this LabelControl labelControl, string text)
      {
         labelControl.Font = new Font(labelControl.Font.Name, 15.0f);
         labelControl.Text = text;
         labelControl.AutoSizeMode = LabelAutoSizeMode.Vertical;
         labelControl.Width = 400;
         labelControl.AsDescription();
         labelControl.BackColor = Color.Transparent;
         labelControl.Top = 200;
         labelControl.Left = 200;
      }

      public static void AsDescription(this LabelControl labelControl)
      {
         labelControl.AutoSizeMode = LabelAutoSizeMode.Vertical;
         labelControl.AllowHtmlString = true;
      }
   }
}