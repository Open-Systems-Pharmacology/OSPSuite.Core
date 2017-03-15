using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;

namespace OSPSuite.UI.RepositoryItems
{
   /// <summary>
   /// Button repository with an editor hidden by default
   /// </summary>
   public class UxRepositoryItemButtonEdit : RepositoryItemButtonEdit
   {
      public UxRepositoryItemButtonEdit()
      {
         TextEditStyle = TextEditStyles.HideTextEditor;
      }

      public UxRepositoryItemButtonEdit(ButtonPredefines firstButtonKind): this()
      {
         Buttons[0].Kind = firstButtonKind;
      }

      /// <summary>
      /// add a button to the buttons list and returns the added editor button
      /// </summary>
      /// <param name="buttonStyle"></param>
      /// <returns></returns>
      public EditorButton AddButton(ButtonPredefines buttonStyle)
      {
         int index = Buttons.Count;
         Buttons.Add(new EditorButton(buttonStyle));
         return Buttons[index];
      }

   }
}