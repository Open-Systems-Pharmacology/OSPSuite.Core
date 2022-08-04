using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using OSPSuite.Assets;

namespace OSPSuite.UI.Extensions
{
   public static class ButtonExtensions
   {
      public static SimpleButton InitWithImage(this SimpleButton button, ApplicationIcon applicationIcon, string text = null, ImageLocation imageLocation = ImageLocation.MiddleLeft, string toolTip = null)
      {
         return button.InitWithImage(applicationIcon, IconSizes.Size16x16, text, imageLocation, toolTip);
      }

      public static SimpleButton InitWithImage(this SimpleButton button, ApplicationIcon applicationIcon, IconSize iconSize, string text = null, ImageLocation imageLocation = ImageLocation.MiddleLeft, string toolTip = null)
      {
         button.ImageOptions.SetImage(applicationIcon, iconSize);
         button.ImageLocation = imageLocation;
         if (text != null)
            button.Text = text;

         if (toolTip != null)
            button.ToolTip = toolTip;

         return button;
      }

      public static void AsAddButton(this LayoutControlItem buttonControlItem, string caption = Captions.AddButtonText, LayoutControl layoutControl = null)
      {
         buttonControlItem.AsLargeButtonWithImage(ApplicationIcons.Forward, caption, layoutControl);
      }

      public static void AsRemoveButton(this LayoutControlItem buttonControlItem, string caption = Captions.RemoveButtonText, LayoutControl layoutControl = null)
      {
         buttonControlItem.AsLargeButtonWithImage(ApplicationIcons.Back, caption, layoutControl);
      }

      public static void AsLargeButtonWithImage(this LayoutControlItem buttonControlItem, ApplicationIcon applicationIcon, string text, LayoutControl layoutControl = null)
      {
         buttonControlItem.AdjustControlSize(UIConstants.Size.ADD_REMOVE_BUTTON_WIDTH, UIConstants.Size.ADD_REMOVE_BUTTON_HEIGHT, layoutControl);
         var button = buttonControlItem.Control as SimpleButton;
         button?.InitWithImage(applicationIcon, IconSizes.Size24x24, text: text, imageLocation: ImageLocation.TopCenter);
      }

      public static void AsButtonWithImage(this LayoutControlItem buttonControlItem, ApplicationIcon applicationIcon, string text, ImageLocation imageLocation = ImageLocation.MiddleLeft, string toolTip = null, LayoutControl layoutControl = null)
      {
         buttonControlItem.AdjustButtonSize(layoutControl);
         var button = buttonControlItem.Control as SimpleButton;
         button?.InitWithImage(applicationIcon, IconSizes.Size16x16, text, imageLocation, toolTip);
      }
   }
}