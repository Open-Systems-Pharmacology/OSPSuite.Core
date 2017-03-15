using DevExpress.XtraEditors;

namespace OSPSuite.UI.Views.Journal
{
   public static class TokenEditExtensions
   {
      public static void InitializeSeparator(this TokenEdit tokenEdit, params string[] separators)
      {
         tokenEdit.Properties.Separators.Clear();
         tokenEdit.Properties.Separators.AddRange(separators);
      }

      public static void Initialize(this TokenEdit tokenEdit)
      {
         tokenEdit.Properties.EditMode = TokenEditMode.Manual;
         tokenEdit.Properties.ShowDropDown = true;
      }
   }
}