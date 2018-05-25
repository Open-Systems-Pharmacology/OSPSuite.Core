using OSPSuite.Assets;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Comparison
{
   public class TableFormulaDiffBuilder : DiffBuilder<TableFormula>
   {
      private readonly ObjectBaseDiffBuilder _objectBaseDiffBuilder;
      private readonly EnumerableComparer _enumerableComparer;
      private readonly IDisplayNameProvider _displayNameProvider;

      public TableFormulaDiffBuilder(ObjectBaseDiffBuilder objectBaseDiffBuilder, EnumerableComparer enumerableComparer, IDisplayNameProvider displayNameProvider)
      {
         _objectBaseDiffBuilder = objectBaseDiffBuilder;
         _enumerableComparer = enumerableComparer;
         _displayNameProvider = displayNameProvider;
      }

      public override void Compare(IComparison<TableFormula> comparison)
      {
         _objectBaseDiffBuilder.Compare(comparison);
         CompareValues(x => x.Dimension, x => x.Dimension, comparison);
         CompareStringValues(x => x.XName, x => x.XName, comparison);
         CompareStringValues(x => x.YName, x => x.YName, comparison);
         CompareValues(x => x.UseDerivedValues, x => x.UseDerivedValues, comparison);
         _enumerableComparer.CompareEnumerables(comparison, x => x.AllPoints(), (pt1, pt2) => ValueComparer.AreValuesEqual(pt1.X, pt2.X, comparison.Settings.RelativeTolerance), x => displayValueFor(x, comparison));
      }

      private string displayValueFor(ValuePoint valuePoint, IComparison<TableFormula> comparison)
      {
         var formulaOwnerName = _displayNameProvider.DisplayNameFor(comparison.CommonAncestor);
         var tableFormula = comparison.Object1.DowncastTo<TableFormula>();
         return Captions.Comparisons.ValuePointAt(tableFormula.Name, formulaOwnerName, tableFormula.XDisplayValueFor(valuePoint.X));
      }
   }
}