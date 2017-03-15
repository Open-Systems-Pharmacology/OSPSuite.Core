using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using OSPSuite.Assets;

namespace OSPSuite.UI.Extensions
{
   public static class ButtonExtensions
   {
      public static SimpleButton InitWithImage(this SimpleButton button, ApplicationIcon applicationIcon, string text = null, ImageLocation imageLocation = ImageLocation.MiddleLeft, string toolTip = null)
      {
         return button.InitWithImage(applicationIcon, IconSizes.Size16x16, text: text, imageLocation: imageLocation, toolTip: toolTip);
      }

      public static SimpleButton InitWithImage(this SimpleButton button, ApplicationIcon applicationIcon, IconSize iconSize, string text = null, ImageLocation imageLocation = ImageLocation.MiddleLeft, string toolTip = null)
      {
         button.Image = applicationIcon.ToImage(iconSize);
         button.ImageLocation = imageLocation;
         if (text != null)
            button.Text = text;

         if (toolTip != null)
            button.ToolTip = toolTip;

         return button;
      }

      public static void AsAddButton(this LayoutControlItem buttonControlItem, string caption = Captions.AddButtonText)
      {
         buttonControlItem.AsLargeButtonWithImage(ApplicationIcons.Forward, caption);
      }

      public static void AsRemoveButton(this LayoutControlItem buttonControlItem, string caption = Captions.RemoveButtonText)
      {
         buttonControlItem.AsLargeButtonWithImage(ApplicationIcons.Back, caption);
      }

      public static void AsLargeButtonWithImage(this LayoutControlItem buttonControlItem, ApplicationIcon applicationIcon, string text)
      {
         buttonControlItem.AdjustControlSize(UIConstants.Size.ADD_REMOVE_BUTTON_WIDTH, UIConstants.Size.ADD_REMOVE_BUTTON_HEIGHT);
         var button = buttonControlItem.Control as SimpleButton;
         button?.InitWithImage(applicationIcon, IconSizes.Size24x24, text: text, imageLocation: ImageLocation.TopCenter);
      }
   }
}