using System.Data;
using System.Linq;
using System.Windows.Forms;
using OSPSuite.Utility.Extensions;
using DevExpress.Utils.Commands;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.Commands.Internal;
using OSPSuite.Assets;
using OSPSuite.Core.Extensions;
using OSPSuite.UI.Mappers;
using OSPSuite.UI.Services;

namespace OSPSuite.UI.Views.Journal
{
   public class CustomPasteSelectionCoreCommand : PasteSelectionCoreCommand
   {
      private readonly ClipboardTask _clipboardTask;
      private readonly IDataTableToDocumentTableMapper _mapper;

      public CustomPasteSelectionCoreCommand(IRichEditControl control, PasteSource pasteSource)
         : base(control, pasteSource)
      {
         _clipboardTask = new ClipboardTask();
         _mapper = new DataTableToDocumentTableMapper();
      }

      public override void ForceExecute(ICommandUIState state)
      {
         var dataObject = Clipboard.GetDataObject();
         if (dataObject != null)
         {
            if (dataObject.GetDataPresent(ObjectTypes.DataTable))
            {
               _mapper.MapFrom(dataObject.GetData(ObjectTypes.DataTable).DowncastTo<DataTable>(), Control.Document);
               return;
            }

            if (isMetaFileOnly(dataObject))
            {
               if (pasteMetaFile()) 
                  return;
            }
         }
         base.ForceExecute(state);
      }

      private static bool isMetaFileOnly(IDataObject dataObject)
      {
         var formats = dataObject.GetFormats();

         return (dataObject.GetDataPresent(DataFormats.EnhancedMetafile) && formats.Count() == 1) ||
            (formats.ContainsAll(new[] { DataFormats.EnhancedMetafile, DataFormats.MetafilePict }) && formats.Count() == 2);
      }

      private bool pasteMetaFile()
      {
         var metaFile = _clipboardTask.GetEnhMetafileFromClipboard();
         if (metaFile == null) return false;
         var image = Control.Document.Images.Insert(Control.Document.CaretPosition, metaFile);

         // The 3/4 scaling is because of the resolution for enhanced metafiles is default 72 ppi while windows uses 96 ppi. This has the effect
         // of scaling up the image during copy
         image.ScaleX = (float) 0.75;
         image.ScaleY = (float) 0.75;
         return true;
      }
   }
}