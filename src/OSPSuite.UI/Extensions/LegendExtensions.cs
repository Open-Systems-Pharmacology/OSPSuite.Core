using System;
using DevExpress.Utils;
using DevExpress.XtraCharts;
using OSPSuite.Core.Chart;

namespace OSPSuite.UI.Extensions
{
   public static class LegendExtensions
   {
      public static void LegendPosition(this Legend legend, LegendPositions legendPosition)
      {
         switch (legendPosition)
         {
            case LegendPositions.None:
               legend.Visibility = DefaultBoolean.False;
               break;
            case LegendPositions.Right:
               setLegend(legend, LegendAlignmentHorizontal.RightOutside, LegendAlignmentVertical.Top);
               break;
            case LegendPositions.RightInside:
               setLegend(legend, LegendAlignmentHorizontal.Right, LegendAlignmentVertical.Top);
               break;
            case LegendPositions.Bottom:
               setLegend(legend, LegendAlignmentHorizontal.Right, LegendAlignmentVertical.BottomOutside);
               break;
            case LegendPositions.BottomInside:
               setLegend(legend, LegendAlignmentHorizontal.Right, LegendAlignmentVertical.Bottom);
               break;
            case LegendPositions.TopLeftOutside:
               setLegend(legend, LegendAlignmentHorizontal.LeftOutside, LegendAlignmentVertical.Top);
               break;
            case LegendPositions.TopLeftInside:
               setLegend(legend, LegendAlignmentHorizontal.Left, LegendAlignmentVertical.Top);
               break;
            case LegendPositions.BottomLeftOutside:
               setLegend(legend, LegendAlignmentHorizontal.Left, LegendAlignmentVertical.BottomOutside);
               break;
            case LegendPositions.BottomLeftInside:
               setLegend(legend, LegendAlignmentHorizontal.Left, LegendAlignmentVertical.Bottom);
               break;

            default:
               throw new ArgumentException("LegendPosition " + legendPosition + " not implemented.");
         }
      }

      private static void setLegend(Legend legend, LegendAlignmentHorizontal horizontal, LegendAlignmentVertical vertical)
      {
         legend.Visibility = DefaultBoolean.True;
         legend.AlignmentHorizontal = horizontal;
         legend.AlignmentVertical = vertical;
      }
   }
}