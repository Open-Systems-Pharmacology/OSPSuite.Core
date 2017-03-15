using DevExpress.XtraEditors.Controls;
using OSPSuite.Assets;

namespace OSPSuite.UI.RepositoryItems
{
   /// <summary>
   /// A repository only containing an image used as a button
   /// </summary>
   public class UxRepositoryItemButtonImage : UxRepositoryItemButtonEdit
   {
      public UxRepositoryItemButtonImage(ApplicationIcon applicationIcon)
         : this(applicationIcon, string.Empty)
      {
      }

      public UxRepositoryItemButtonImage(ApplicationIcon applicationIcon, string toolTip)
         : base(ButtonPredefines.Glyph)
      {
         TextEditStyle = TextEditStyles.HideTextEditor;
         Buttons[0].Image = applicationIcon;
         Buttons[0].ToolTip = toolTip;
      }
   }
}