using DevExpress.Office.Commands.Internal;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.Commands;

namespace OSPSuite.UI.Views.Journal
{
   public class CustomPasteSelectionCommand : PasteSelectionCommand
   {
      public CustomPasteSelectionCommand(IRichEditControl control)
         : base(control)
      {

      }

      protected override RichEditCommand CreateInsertObjectCommand()
      {
         return new CustomPasteSelectionCoreCommand(Control, new ClipboardPasteSource());
      }
   }
}