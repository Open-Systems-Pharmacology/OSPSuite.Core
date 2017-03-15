using OSPSuite.Core.Chart;

namespace OSPSuite.Core.Comparison
{
   public class CurveDataTemplateDiffBuilder : DiffBuilder<CurveDataTemplate>
   {
      public override void Compare(IComparison<CurveDataTemplate> comparison)
      {
         CompareStringValues(x => x.Path, x => x.Path, comparison);
         CompareValues(x => x.QuantityType, x => x.QuantityType, comparison);
         CompareStringValues(x => x.RepositoryName, x => x.RepositoryName, comparison);
      }
   }
}