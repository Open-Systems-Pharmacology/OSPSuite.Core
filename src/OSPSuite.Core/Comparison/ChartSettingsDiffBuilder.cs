using OSPSuite.Core.Chart;

namespace OSPSuite.Core.Comparison
{
   public class ChartSettingsDiffBuilder : DiffBuilder<ChartSettings>
   {
      public override void Compare(IComparison<ChartSettings> comparison)
      {
         CompareValues(x => x.BackColor, x => x.BackColor, comparison);
         CompareValues(x => x.DiagramBackColor, x => x.DiagramBackColor, comparison);
         CompareValues(x => x.LegendPosition, x => x.LegendPosition, comparison);
         CompareValues(x => x.SideMarginsEnabled, x => x.SideMarginsEnabled, comparison);
      }
   }
}