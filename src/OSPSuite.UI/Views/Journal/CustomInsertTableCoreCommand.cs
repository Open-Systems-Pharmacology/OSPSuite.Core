using DevExpress.Utils.Commands;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.Commands.Internal;

namespace OSPSuite.UI.Views.Journal
{
   public class CustomInsertTableCoreCommand : InsertTableCoreCommand
   {
      private readonly IRichEditControl _control;

      public CustomInsertTableCoreCommand(IRichEditControl control) : base(control)
      {
         _control = control;
      }

      public override void ForceExecute(ICommandUIState state)
      {
         var table = _control.Document.Tables.Create(Control.Document.CaretPosition, RowCount, ColumnCount);
         var paragraphs = _control.Document.BeginUpdateParagraphs(table.Range);
         paragraphs.RightIndent = 0;
         _control.Document.EndUpdateParagraphs(paragraphs);
      }
   }
}