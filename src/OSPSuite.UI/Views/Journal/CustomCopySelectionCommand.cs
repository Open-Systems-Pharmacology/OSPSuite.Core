using System.Drawing.Imaging;
using DevExpress.Office.Utils;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using DevExpress.XtraRichEdit.Commands;
using OSPSuite.UI.Services;

namespace OSPSuite.UI.Views.Journal
{
   public class CustomCopySelectionCommand : CopySelectionCommand
   {
      private readonly RichEditControl _control;
      private readonly IClipboardTask _clipboardTask;

      public CustomCopySelectionCommand(RichEditControl control, IClipboardTask clipboardTask)
         : base(control)
      {
         _control = control;
         _clipboardTask = clipboardTask;
      }

      protected override void ExecuteCore()
      {
         var range = _control.Document.Selection;
         var documentImageCollection = _control.Document.Images.Get(range);
         if (range.Length == 1 && documentImageCollection.Count == 1)
         {
            copyImageToClipboard(documentImageCollection[0]);
         }
         else
         {
            base.ExecuteCore();
         }
      }

      private void copyImageToClipboard(DocumentImage documentImage)
      {
         var image = new Metafile(documentImage.Image.GetImageBytesStreamSafe(OfficeImageFormat.Emf));
         _clipboardTask.PutEnhMetafileOnClipboard(_control, image);
      }
   }
}