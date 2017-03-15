using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Comparison
{
   public class FormulaUsablePathDiffBuilder : DiffBuilder<FormulaUsablePath>
   {
      private readonly ObjectPathDiffBuilder _objectPathDiffBuilder;

      public FormulaUsablePathDiffBuilder(ObjectPathDiffBuilder objectPathDiffBuilder)
      {
         _objectPathDiffBuilder = objectPathDiffBuilder;
      }

      public override void Compare(IComparison<FormulaUsablePath> comparison)
      {
         _objectPathDiffBuilder.Compare(comparison);
         CompareStringValues(x => x.Alias, x => x.Alias, comparison);
         CompareValues(x => x.Dimension, x => x.Dimension, comparison);
      }
   }
}