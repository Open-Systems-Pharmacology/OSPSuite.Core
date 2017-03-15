using OSPSuite.Core.Chart;

namespace OSPSuite.Core.Comparison
{
   public class CurveTemplateDiffBuilder : DiffBuilder<CurveTemplate>
   {
      private readonly IObjectComparer _objectComparer;

      public CurveTemplateDiffBuilder(IObjectComparer objectComparer)
      {
         _objectComparer = objectComparer;
      }

      public override void Compare(IComparison<CurveTemplate> comparison)
      {
         CompareValues(x => x.Name, x => x.Name, comparison);
         _objectComparer.Compare(comparison.ChildComparison(x => x.xData));
         _objectComparer.Compare(comparison.ChildComparison(x => x.yData));
         CompareValues(x => x.IsBaseGrid, x => x.IsBaseGrid, comparison);
         _objectComparer.Compare(comparison.ChildComparison(x => x.CurveOptions));
      }
   }
}