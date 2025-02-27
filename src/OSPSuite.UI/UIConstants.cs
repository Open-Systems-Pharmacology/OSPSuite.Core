﻿using System;
using System.Collections.Generic;
using System.Drawing;
using DevExpress.Skins;
using OSPSuite.Assets;

namespace OSPSuite.UI
{
   public static class UIConstants
   {
      public static class Colors
      {
         // These colors change with the skin, so we use a method to read the latest up-to-date value from the skin
         public static Color Disabled => skinColorFor("ReadOnly");

         private static Color skinColorFor(string state)
         {
            return CommonSkins.GetSkin(DevExpress.LookAndFeel.UserLookAndFeel.Default).Colors[state];
         }
      }

      public const int TOOL_TIP_INITIAL_DELAY = 500;
      public const int TOOL_TIP_INITIAL_DELAY_LONG = 1500;
      public const string EMPTY_COLUMN = " ";

      public const double DEFAULT_HTML_FONT_SIZE = 8.25;

      /// <summary>
      ///    Size used for icons displayed in tab
      /// </summary>
      public static IconSize ICON_SIZE_TAB = IconSizes.Size24x24;

      /// <summary>
      ///    Size used for icons displayed in context menu
      /// </summary>
      public static IconSize ICON_SIZE_CONTEXT_MENU = IconSizes.Size16x16;

      /// <summary>
      ///    Size used for icons displayed in tree view
      /// </summary>
      public static IconSize ICON_SIZE_TREE_VIEW = IconSizes.Size24x24;

      public const string DEFAULT_HTML_FONT = "Tahoma";

      public static class Size
      {
         private static readonly double _scaleFactor = createScaleFactor();
         public static readonly int EMBEDDED_BUTTON_WIDTH = ScaleForScreenDPI(25);
         public static readonly int EMBEDDED_CHECK_BOX_WIDTH = ScaleForScreenDPI(60);
         public static readonly int EMBEDDED_DESCRIPTION_WIDTH = ScaleForScreenDPI(80);
         public static readonly int EMPTY_SPACE_HEIGHT = ScaleForScreenDPI(20);
         public static readonly int RADIO_GROUP_HEIGHT = ScaleForScreenDPI(24);
         public static readonly int ADD_REMOVE_BUTTON_WIDTH = ScaleForScreenDPI(100);
         public static readonly int APPLY_TO_ALL_BUTTON_WIDTH = ScaleForScreenDPI(100);
         public static readonly int ADD_REMOVE_BUTTON_HEIGHT = ScaleForScreenDPI(60);

         public const double SCREEN_RESIZE_FRACTION = 0.9;

         //Buttons tends to grow to wide on bigger screen. We set a max
         public static readonly int BUTTON_WIDTH = ScaleForScreenDPI(105);

         public static readonly int LARGE_BUTTON_WIDTH = ScaleForScreenDPI(150);

         //Buttons tends to grow to tall on bigger screen. We set a max
         public static readonly int BUTTON_HEIGHT = ScaleForScreenDPI(24, 48);
         public static readonly int LARGE_BUTTON_HEIGHT = ScaleForScreenDPI(29, 48);

         public static readonly int OPTIMIZED_RANGE_WIDTH = ScaleForScreenDPI(300);
         public static readonly int APPLICATION_MENU_RIGHT_PANE_WIDTH = ScaleForScreenDPI(300);

         public static readonly int COMPUTED_EXTRA_HEIGHT = computedExtraRowHeight();

         private static int computedExtraRowHeight()
         {
            //This extra constant seems to be what we need to have perfect height on 200% scaling.
            const int extraHeight = 3;
            var scaledExtraHeight = ScaleForScreenDPI(extraHeight);
            var needExtraHeight = scaledExtraHeight != extraHeight;
            //We return the extra row height (not scaled)
            return needExtraHeight ? extraHeight : 0;
         }

         public static int ScaleForScreenDPI(int size, int? maxSize = null)
         {
            var proposedSize = (int)(_scaleFactor * size);
            return Math.Min(proposedSize, maxSize.GetValueOrDefault(proposedSize));
         }

         private static double createScaleFactor()
         {
            using (var graphics = Graphics.FromHwnd(IntPtr.Zero))
            {
               if (graphics.DpiX > 120)
               {
                  return graphics.DpiX / 96;
               }

               return graphics.DpiX > 96 ? 1.25 : 1.0;
            }
         }
      }

      public static class Chart
      {
         public const int SERIES_MARKER_SIZE = 5;
         public static readonly System.Drawing.Size LEGEND_MARKER_SIZE = new System.Drawing.Size(9, 9);
      }

      internal static class Skins
      {
         public const string METROPOLIS = "Metropolis";
         public const string METROPOLIS_DARK = "MetropolisDark";
         public const string VISUAL_STUDIO2013_LIGHT = "Visual Studio 2013 Light";
         public const string VISUAL_STUDIO2013_DARK = "Visual Studio 2013 Dark";
         public const string VISUAL_STUDIO2013_BLUE = "Visual Studio 2013 Blue";
         public const string OFFICE2010_BLACK = "Office 2010 Black";
         public const string BLUEPRINT = "Blueprint";
         public const string SHARP_PLUS = "Sharp Plus";
         public const string DARKROOM = "Darkroom";
         public const string DEV_EXPRESS_DARK_STYLE = "DevExpress Dark Style";
         public const string DARK_SIDE = "Dark Side";
         public const string HIGH_CONTRAST = "High Contrast";

         public static IReadOnlyList<string> SkinsWithoutBorder = new List<string>
         {
            METROPOLIS,
            METROPOLIS_DARK,
            VISUAL_STUDIO2013_BLUE,
            VISUAL_STUDIO2013_DARK,
            VISUAL_STUDIO2013_LIGHT
         };

         public static IReadOnlyList<string> ForbiddenSkins = new List<string>
         {
            OFFICE2010_BLACK,
            BLUEPRINT,
            SHARP_PLUS,
            DARKROOM,
            DEV_EXPRESS_DARK_STYLE,
            DARK_SIDE,
            HIGH_CONTRAST
         };
      }
   }
}