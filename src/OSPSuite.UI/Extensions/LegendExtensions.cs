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
               legend.Visibility = DefaultBoolean.True;
               legend.AlignmentHorizontal = LegendAlignmentHorizontal.RightOutside;
               legend.AlignmentVertical = LegendAlignmentVertical.Top;
               break;
            case LegendPositions.Bottom:
               legend.Visibility = DefaultBoolean.True;
               legend.AlignmentHorizontal = LegendAlignmentHorizontal.Right;
               legend.AlignmentVertical = LegendAlignmentVertical.BottomOutside;
               break;
            case LegendPositions.RightInside:
               legend.Visibility = DefaultBoolean.True;
               legend.AlignmentHorizontal = LegendAlignmentHorizontal.Right;
               legend.AlignmentVertical = LegendAlignmentVertical.Top;
               break;
            case LegendPositions.BottomInside:
               legend.Visibility = DefaultBoolean.True;
               legend.AlignmentHorizontal = LegendAlignmentHorizontal.Right;
               legend.AlignmentVertical = LegendAlignmentVertical.Bottom;
               break;

            default:
               throw new ArgumentException("LegendPosition " + legendPosition + " not implemented.");
         }
      }
   }
}