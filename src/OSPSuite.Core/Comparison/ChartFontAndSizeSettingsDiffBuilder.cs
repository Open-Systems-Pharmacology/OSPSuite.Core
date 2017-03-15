using OSPSuite.Core.Chart;

namespace OSPSuite.Core.Comparison
{
   public class ChartFontAndSizeSettingsDiffBuilder : DiffBuilder<ChartFontAndSizeSettings>
   {
      private readonly IObjectComparer _objectComparer;

      public ChartFontAndSizeSettingsDiffBuilder(IObjectComparer objectComparer)
      {
         _objectComparer = objectComparer;
      }

      public override void Compare(IComparison<ChartFontAndSizeSettings> comparison)
      {
         CompareValues(x => x.ChartHeight, x => x.ChartHeight, comparison);
         CompareValues(x => x.ChartWidth, x => x.ChartWidth, comparison);
         _objectComparer.Compare(comparison.ChildComparison(x => x.Fonts, comparison.CommonAncestor));
      }
   }
}