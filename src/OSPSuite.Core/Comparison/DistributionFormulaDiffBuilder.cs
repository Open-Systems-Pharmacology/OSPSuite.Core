using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core.Comparison
{
   /// <summary>
   ///    A distribution is defined by the sub parameters its aliases (mean, deviation, ...) are bound to. Two distributions
   ///    of the same type with identical sub parameters still differ when an alias references another parameter, hence the
   ///    referenced paths are compared by alias. Distributions of different types are reported as a type mismatch before
   ///    reaching this builder.
   /// </summary>
   public class DistributionFormulaDiffBuilder : DiffBuilder<DistributionFormula>
   {
      private readonly ObjectBaseDiffBuilder _objectBaseDiffBuilder;
      private readonly EnumerableComparer _enumerableComparer;

      public DistributionFormulaDiffBuilder(ObjectBaseDiffBuilder objectBaseDiffBuilder, EnumerableComparer enumerableComparer)
      {
         _objectBaseDiffBuilder = objectBaseDiffBuilder;
         _enumerableComparer = enumerableComparer;
      }

      public override void Compare(IComparison<DistributionFormula> comparison)
      {
         _objectBaseDiffBuilder.Compare(comparison);
         CompareValues(x => x.Dimension, x => x.Dimension, comparison);
         _enumerableComparer.CompareEnumerables(comparison, x => x.ObjectPaths, x => x.Alias);
      }
   }
}
