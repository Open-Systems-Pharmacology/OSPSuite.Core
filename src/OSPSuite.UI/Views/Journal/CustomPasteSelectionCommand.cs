using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.Commands;
using DevExpress.XtraRichEdit.Commands.Internal;

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