using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Services;

namespace OSPSuite.Core.Comparison
{
   public class TableFormulaDiffBuilder : DiffBuilder<TableFormula>
   {
      private readonly IObjectComparer _objectComparer;
      private readonly ObjectBaseDiffBuilder _objectBaseDiffBuilder;
      private readonly EnumerableComparer _enumerableComparer;

      public TableFormulaDiffBuilder(IObjectComparer objectComparer, ObjectBaseDiffBuilder objectBaseDiffBuilder, EnumerableComparer enumerableComparer)
      {
         _objectComparer = objectComparer;
         _objectBaseDiffBuilder = objectBaseDiffBuilder;
         _enumerableComparer = enumerableComparer;
      }

      public override void Compare(IComparison<TableFormula> comparison)
      {
         _objectBaseDiffBuilder.Compare(comparison);
         _objectComparer.Compare(comparison.DimensionComparison());
         CompareStringValues(x => x.XName, x => x.XName, comparison);
         CompareStringValues(x => x.YName, x => x.YName, comparison);
         CompareValues(x => x.UseDerivedValues, x => x.UseDerivedValues, comparison);
         //We do not use enumerable comparison here as we need to compare the values by index
         _enumerableComparer.CompareEnumerablesByIndex(comparison, x => x.AllPoints);
      }
   }
}