using System.Windows.Forms;
using OSPSuite.Utility;
using OSPSuite.Core.Services;

namespace OSPSuite.UI.Mappers
{
   public interface IDialogResultToViewResultMapper : IMapper<DialogResult, ViewResult>
   {
   }

   public class DialogResultToViewResultMapper : IDialogResultToViewResultMapper
   {
      public ViewResult MapFrom(DialogResult input)
      {
         switch (input)
         {
            case DialogResult.OK:
               return ViewResult.OK;
            case DialogResult.Cancel:
               return ViewResult.Cancel;
            case DialogResult.Yes:
               return ViewResult.Yes;
            case DialogResult.No:
               return ViewResult.No;
            case DialogResult.None:
            case DialogResult.Retry:
            case DialogResult.Abort:
            case DialogResult.Ignore:
               return ViewResult.Cancel;
            default:
               return ViewResult.Cancel;
         }
      }
   }
}