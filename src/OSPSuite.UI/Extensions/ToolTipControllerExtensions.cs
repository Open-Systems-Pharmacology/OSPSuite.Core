using DevExpress.Utils;
using DevExpress.XtraEditors.Container;
using OSPSuite.UI.Services;

namespace OSPSuite.UI.Extensions
{
   public static class ToolTipControllerExtensions
   {
      public static ToolTipController Initialize(this ToolTipController toolTipController, IImageListRetriever imageListRetriever, int delayInMilliseconds = UIConstants.TOOL_TIP_INITIAL_DELAY)
      {
         toolTipController.Rounded = true;
         toolTipController.ShowBeak = false;
         toolTipController.AllowHtmlText = true;
         toolTipController.ImageList = imageListRetriever.AllImages16x16;
         toolTipController.AutoPopDelay = 10000;
         toolTipController.InitialDelay = delayInMilliseconds;
         return toolTipController;
      }

      public static ToolTipController For(this ToolTipController toolTipController, EditorContainer editorContainer)
      {
         editorContainer.ToolTipController = toolTipController;
         return toolTipController;
      }
   }
}