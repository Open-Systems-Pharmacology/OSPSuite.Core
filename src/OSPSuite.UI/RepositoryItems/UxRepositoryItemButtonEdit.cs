using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;

namespace OSPSuite.UI.RepositoryItems
{
   /// <summary>
   ///    Button repository with an editor hidden by default
   /// </summary>
   public class UxRepositoryItemButtonEdit : RepositoryItemButtonEdit
   {
      public UxRepositoryItemButtonEdit()
      {
         TextEditStyle = TextEditStyles.HideTextEditor;
      }

      public UxRepositoryItemButtonEdit(ButtonPredefines firstButtonKind, string toolTip = null, object tag = null) : this()
      {
         Buttons[0].Kind = firstButtonKind;
         Buttons[0].ToolTip = toolTip;
         Buttons[0].Tag = tag;
      }

      /// <summary>
      ///    add a button to the buttons list and returns the added editor button
      /// </summary>
      public EditorButton AddButton(ButtonPredefines buttonStyle, string toolTip = null, object tag = null)
      {
         int index = Buttons.Count;
         var newButton = new EditorButton(buttonStyle) {ToolTip = toolTip, Tag = tag};
         Buttons.Add(newButton);
         return Buttons[index];
      }
   }
}