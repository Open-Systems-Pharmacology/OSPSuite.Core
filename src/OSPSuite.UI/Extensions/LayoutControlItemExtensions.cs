using System.Drawing;
using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.XtraLayout;
using OSPSuite.Presentation.Extensions;

namespace OSPSuite.UI.Extensions
{
   public static class LayoutControlItemExtensions
   {
      public static void InitializeAsHeader(this LayoutControlItem layoutControlItem, UserLookAndFeel lookAndFeel, string text)
      {
         var currentSkin = CommonSkins.GetSkin(lookAndFeel);
         Color color = currentSkin.TranslateColor(SystemColors.ControlText);
         layoutControlItem.AppearanceItemCaption.ForeColor = color;
         layoutControlItem.TextVisible = !string.IsNullOrEmpty(text);
         layoutControlItem.Text = text.FormatForLabel();
      }

      public static void AdjustLargeButtonSize(this LayoutControlItem layoutControlItem)
      {
         layoutControlItem.AdjustButtonSize(UIConstants.Size.LARGE_BUTTON_WIDTH, UIConstants.Size.LARGE_BUTTON_HEIGHT);
      }

      public static void AdjustLongButtonSize(this LayoutControlItem layoutControlItem)
      {
         layoutControlItem.AdjustButtonSize(UIConstants.Size.LARGE_BUTTON_WIDTH, UIConstants.Size.BUTTON_HEIGHT);
      }

      /// <summary>
      /// Sets the layout control item size to custom and define the min and max width
      /// </summary>
      public static void AdjustSize(this LayoutControlItem layoutControlItem, int width, int height)
      {
         var size = new Size(width, height);
         layoutControlItem.SizeConstraintsType = SizeConstraintsType.Custom;
         layoutControlItem.MaxSize = size;
         layoutControlItem.MinSize = size;
      }

      /// <summary>
      /// Only sets the control item height. Width will be calculated dynamically
      /// </summary>
      public static void AdjustControlHeight(this LayoutControlItem layoutControlItem, int height)
      {
         //using 0 allows the control witdh to be calculated dynamically
         layoutControlItem.AdjustControlSize(0, height);
      }

      /// <summary>
      /// Only sets the control item width. Height will be calculated dynamically
      /// </summary>
      public static void AdjustControlWidth(this LayoutControlItem layoutControlItem, int width)
      {
         //using 0 allows the control witdh to be calculated dynamically
         layoutControlItem.AdjustControlSize(width, 0);
      }

      /// <summary>
      /// Sets the size of the inner control 
      /// </summary>
      public static void AdjustControlSize(this LayoutControlItem layoutControlItem, int width, int height)
      {
         var size = new Size(width, height);
         layoutControlItem.SizeConstraintsType = SizeConstraintsType.Custom;
         layoutControlItem.ControlMaxSize = size;
         layoutControlItem.ControlMinSize = size;
      }

      public static void AdjustButtonSize(this LayoutControlItem layoutControlItem)
      {
         AdjustButtonSize(layoutControlItem, UIConstants.Size.BUTTON_WIDTH, UIConstants.Size.BUTTON_HEIGHT);
      }

      public static void AdjustButtonSize(this LayoutControlItem layoutControlItem, int width, int height)
      {
         layoutControlItem.AdjustSize(width, height);
         layoutControlItem.Size = new Size(width, height);
      }

      public static void AdjustButtonSizeWithImageOnly(this LayoutControlItem layoutControlItem)
      {
         layoutControlItem.AdjustSize(UIConstants.Size.BUTTON_HEIGHT, UIConstants.Size.BUTTON_HEIGHT);
         layoutControlItem.Size = new Size(UIConstants.Size.BUTTON_HEIGHT, UIConstants.Size.BUTTON_HEIGHT);
      }
   }
}