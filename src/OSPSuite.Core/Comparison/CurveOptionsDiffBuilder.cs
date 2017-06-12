using OSPSuite.Core.Chart;

namespace OSPSuite.Core.Comparison
{
   public class CurveOptionsDiffBuilder : DiffBuilder<CurveOptions>
   {
      public override void Compare(IComparison<CurveOptions> comparison)
      {
         CompareValues(x => x.Color, x => x.Color, comparison);
         CompareValues(x => x.InterpolationMode, x => x.InterpolationMode, comparison);
         CompareValues(x => x.LegendIndex, x => x.LegendIndex, comparison);
         CompareValues(x => x.LineStyle, x => x.LineStyle, comparison);
         CompareValues(x => x.LineThickness, x => x.LineThickness, comparison);
         CompareValues(x => x.Symbol, x => x.Symbol, comparison);
         CompareValues(x => x.Visible, x => x.Visible, comparison);
         CompareValues(x => x.VisibleInLegend, x => x.VisibleInLegend, comparison);
         CompareValues(x => x.yAxisType, x => x.yAxisType, comparison);
      }
   }
}