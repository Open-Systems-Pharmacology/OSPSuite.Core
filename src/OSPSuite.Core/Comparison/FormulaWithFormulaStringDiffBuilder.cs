using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core.Comparison
{
   public class FormulaWithFormulaStringDiffBuilder : DiffBuilder<FormulaWithFormulaString>
   {
      private readonly EnumerableComparer _enumerableComparer;

      public FormulaWithFormulaStringDiffBuilder(EnumerableComparer enumerableComparer)
      {
         _enumerableComparer = enumerableComparer;
      }

      public override void Compare(IComparison<FormulaWithFormulaString> comparison)
      {
         CompareValues(x => x.Dimension, x => x.Dimension, comparison);
         CompareStringValues(x => x.FormulaString, x => x.FormulaString, comparison);
         _enumerableComparer.CompareEnumerables(comparison, x => x.ObjectPaths, x => x.Alias);
      }
   }
}