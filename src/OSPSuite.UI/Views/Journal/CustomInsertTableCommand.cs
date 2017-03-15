using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.Commands;

namespace OSPSuite.UI.Views.Journal
{
   public class CustomInsertTableCommand : InsertTableCommand
   {
      public CustomInsertTableCommand(IRichEditControl control) : base(control)
      {
      }

      protected override RichEditCommand CreateInsertObjectCommand()
      {
         return new CustomInsertTableCoreCommand(Control);
      }
   }
}