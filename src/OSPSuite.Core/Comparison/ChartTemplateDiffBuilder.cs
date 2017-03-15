using OSPSuite.Core.Chart;

namespace OSPSuite.Core.Comparison
{
   public class ChartTemplateDiffBuilder : DiffBuilder<CurveChartTemplate>
   {
      private readonly EnumerableComparer _enumerableComparer;
      private readonly IObjectComparer _objectComparer;

      public ChartTemplateDiffBuilder(EnumerableComparer enumerableComparer, IObjectComparer objectComparer)
      {
         _enumerableComparer = enumerableComparer;
         _objectComparer = objectComparer;
      }

      public override void Compare(IComparison<CurveChartTemplate> comparison)
      {
         _objectComparer.Compare(comparison.ChildComparison(x => x.ChartSettings));
         _objectComparer.Compare(comparison.ChildComparison(x => x.FontAndSize));
         _enumerableComparer.CompareEnumerables(comparison, x => x.Axes, item => item.Caption);
         _enumerableComparer.CompareEnumerables(comparison, x => x.Curves, item => item.Name);
      }
   }
}