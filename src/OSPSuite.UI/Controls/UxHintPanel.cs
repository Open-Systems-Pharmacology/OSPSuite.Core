using OSPSuite.Assets;

namespace OSPSuite.UI.Controls
{
   public partial class UxHintPanel : DevExpress.XtraEditors.XtraUserControl
   {

      public UxHintPanel()
      {
         InitializeComponent();
      }

      public string NoteText
      {
         get { return panelNote.Text; }
         set { panelNote.Text = value; }
      }

      public ApplicationIcon Image
      {
         set
         {
            panelNote.ArrowImage = value.ToImage(IconSizes.Size32x32);
         }
      }

   }
}
