using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Comparison
{
   public class DistributedParameterDiffBuilder : DiffBuilder<IDistributedParameter>
   {
      private readonly ParameterDiffBuilder _parameterDiffBuilder;
      private readonly EnumerableComparer _enumerableComparer;

      public DistributedParameterDiffBuilder(ParameterDiffBuilder parameterDiffBuilder, EnumerableComparer enumerableComparer)
      {
         _parameterDiffBuilder = parameterDiffBuilder;
         _enumerableComparer = enumerableComparer;
      }

      public override void Compare(IComparison<IDistributedParameter> comparison)
      {
         _parameterDiffBuilder.Compare(comparison);

         if (shouldCompareDistributedParameterValue(comparison))
            CompareDoubleValues(x => x.Value, x => x.Value, comparison, x => x.DisplayUnit);

         //the distribution is defined by the sub parameters (mean, deviation, percentile...) which are not necessarily reflected in the value
         _enumerableComparer.CompareEnumerables(comparison, x => x.Children, item => item.Name);
      }

      private bool shouldCompareDistributedParameterValue(IComparison<IDistributedParameter> comparison)
      {
         //in case of distributed parameters, we compare the value even if the comparison mode is formula
         if (comparison.Settings.FormulaComparison == FormulaComparison.Value)
            return false;

         if (!comparison.ComparedObjectsDefined)
            return true;

         var parameter1 = comparison.Object1;
         var parameter2 = comparison.Object2;

         //if one of the parameter is fixed, value were compared already in the parameter diff builder so nothing to do here
         if (parameter1.IsFixedValue || parameter2.IsFixedValue)
            return false;

         return ParameterDiffBuilder.ShouldCompareParametersIn(comparison);
      }
   }
}