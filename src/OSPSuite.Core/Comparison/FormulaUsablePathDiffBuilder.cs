using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Comparison
{
   public class FormulaUsablePathDiffBuilder : DiffBuilder<FormulaUsablePath>
   {
      private readonly IObjectComparer _objectComparer;
      private readonly ObjectPathDiffBuilder _objectPathDiffBuilder;

      public FormulaUsablePathDiffBuilder(IObjectComparer objectComparer, ObjectPathDiffBuilder objectPathDiffBuilder)
      {
         _objectComparer = objectComparer;
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