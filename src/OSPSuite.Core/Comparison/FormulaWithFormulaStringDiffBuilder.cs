using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core.Comparison
{
   public class FormulaWithFormulaStringDiffBuilder : DiffBuilder<FormulaWithFormulaString>
   {
      private readonly IObjectComparer _objectComparer;
      private readonly EnumerableComparer _enumerableComparer;

      public FormulaWithFormulaStringDiffBuilder(IObjectComparer objectComparer, EnumerableComparer enumerableComparer)
      {
         _objectComparer = objectComparer;
         _enumerableComparer = enumerableComparer;
      }

      public override void Compare(IComparison<FormulaWithFormulaString> comparison)
      {
         _objectComparer.Compare(comparison.DimensionComparison());
         CompareStringValues(x => x.FormulaString, x => x.FormulaString, comparison);
         _enumerableComparer.CompareEnumerables(comparison, x => x.ObjectPaths, x => x.Alias);
      }
   }
}