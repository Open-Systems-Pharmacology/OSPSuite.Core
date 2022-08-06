using System;
using System.Drawing;
using System.Linq;
using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.Utils.Layout;
using DevExpress.XtraLayout;
using OSPSuite.Presentation.Extensions;
using OSPSuite.UI.Controls;
using static OSPSuite.UI.UIConstants.Size;

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

      public static void AdjustLargeButtonSize(this LayoutControlItem layoutControlItem, LayoutControl layoutControl = null)
      {
         layoutControlItem.AdjustControlSize(LARGE_BUTTON_WIDTH, LARGE_BUTTON_HEIGHT, layoutControl);
      }

      public static void AdjustLongButtonSize(this LayoutControlItem layoutControlItem, LayoutControl layoutControl = null)
      {
         layoutControlItem.AdjustControlSize(LARGE_BUTTON_WIDTH, BUTTON_HEIGHT, layoutControl);
      }

      public static void AdjustTallButtonSize(this LayoutControlItem layoutControlItem, LayoutControl layoutControl = null)
      {
         layoutControlItem.AdjustControlSize(BUTTON_WIDTH, LARGE_BUTTON_HEIGHT, layoutControl);
      }

      /// <summary>
      ///    Sets the layout control item size to custom and define the min and max width
      /// </summary>
      public static void AdjustSize(this LayoutControlItem layoutControlItem, int width, int height, LayoutControl layoutControl = null)
      {
         var size = new Size(width, height);
         layoutControl?.BeginUpdate();
         layoutControlItem.SizeConstraintsType = SizeConstraintsType.Custom;
         layoutControlItem.MaxSize = size;
         layoutControlItem.MinSize = size;
         //In case the control size became smaller that the previous min size, the max size is ignored. So we set it again 
         layoutControlItem.MaxSize = size;
         layoutControlItem.Size = size;
         layoutControl?.EndUpdate();
      }

      /// <summary>
      ///    Only sets the control item height. Width will be calculated dynamically
      /// </summary>
      public static void AdjustControlHeight(this LayoutControlItem layoutControlItem, int height, LayoutControl layoutControl = null)
      {
         //using 0 allows the control width to be calculated dynamically
         layoutControlItem.AdjustControlSize(0, height, layoutControl);
      }

      /// <summary>
      ///    Only sets the control item width. Height will be calculated dynamically
      /// </summary>
      public static void AdjustControlWidth(this LayoutControlItem layoutControlItem, int width, LayoutControl layoutControl = null)
      {
         //using 0 allows the control width to be calculated dynamically
         layoutControlItem.AdjustControlSize(width, 0, layoutControl);
      }

      /// <summary>
      ///    Sets the size of the inner control
      /// </summary>
      public static void AdjustControlSize(this LayoutControlItem layoutControlItem, int width, int height, LayoutControl layoutControl = null)
      {
         var size = new Size(width, height);
         layoutControl?.BeginUpdate();
         layoutControlItem.SizeConstraintsType = SizeConstraintsType.Custom;
         layoutControlItem.ControlMaxSize = size;
         layoutControlItem.ControlMinSize = size;
         //In case the control size became smaller that the previous min size, the max size is ignored. So we set it again 
         layoutControlItem.ControlMaxSize = size;
         layoutControl?.EndUpdate();
      }

      public static void AdjustButtonSize(this LayoutControlItem layoutControlItem, LayoutControl layoutControl = null)
      {
         layoutControlItem.AdjustControlSize(BUTTON_WIDTH, BUTTON_HEIGHT, layoutControl);
      }

      public static void AdjustButtonSizeWithImageOnly(this LayoutControlItem layoutControlItem, LayoutControl layoutControl = null)
      {
         layoutControlItem.AdjustControlSize(BUTTON_HEIGHT, BUTTON_HEIGHT, layoutControl);
      }

      public static void AdjustGridViewHeight(this LayoutControlItem layoutControlItem, UxGridView gridView, LayoutControl layoutControl)
      {
         layoutControlItem.AdjustControlHeight(gridView.OptimalHeight, layoutControl);
      }

      public static void AdjustTablePanelHeight(this LayoutControlItem layoutControlItem, TablePanel tablePanel, LayoutControl layoutControl)
      {
         var visibleRows = tablePanel.Rows.Where(x => x.Visible).ToList();
         var visibleRowHeight = visibleRows.Sum(x => ScaleForScreenDPI(Convert.ToInt16(x.Height)) + COMPUTED_EXTRA_HEIGHT);
         layoutControlItem.AdjustControlHeight(visibleRowHeight, layoutControl);
      }
   }
}