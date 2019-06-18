using DevExpress.XtraEditors;
using OSPSuite.Assets;

namespace OSPSuite.UI.Controls
{
   public partial class UxHintPanel : XtraUserControl
   {
      public UxHintPanel()
      {
         InitializeComponent();
      }

      public string NoteText
      {
         get => panelNote.Text;
         set => panelNote.Text = value;
      }

      public ApplicationIcon Image
      {
         set => panelNote.ArrowImage = value.ToImage(IconSizes.Size32x32);
      }

      public int MaxLines
      {
         get => panelNote.MaxRows;
         set => panelNote.MaxRows = value;
      }
   }
}