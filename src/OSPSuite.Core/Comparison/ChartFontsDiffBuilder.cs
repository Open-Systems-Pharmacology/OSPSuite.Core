using OSPSuite.Core.Chart;

namespace OSPSuite.Core.Comparison
{
   public class ChartFontsDiffBuilder : DiffBuilder<ChartFonts>
   {
      public override void Compare(IComparison<ChartFonts> comparison)
      {
         CompareValues(x => x.FontFamilyName, x => x.FontFamilyName, comparison);
         CompareValues(x => x.AxisSize, x => x.AxisSize, comparison);
         CompareValues(x => x.DescriptionSize, x => x.DescriptionSize, comparison);
         CompareValues(x => x.WatermarkSize, x => x.WatermarkSize, comparison);
         CompareValues(x => x.OriginSize, x => x.OriginSize, comparison);
         CompareValues(x => x.LegendSize, x => x.LegendSize, comparison);
         CompareValues(x => x.TitleSize, x => x.TitleSize, comparison);
      }
   }
}