using System.Windows.Forms;

namespace OSPSuite.UI.Extensions
{
   public static class KeysExtensions
   {
      public static bool IsPressed(this Keys keyData, Keys key)
      {
         return keyData == key;
      }
   }
}