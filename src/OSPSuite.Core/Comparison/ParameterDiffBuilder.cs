using System;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Comparison
{
   public class ParameterDiffBuilder : DiffBuilder<IParameter>
   {
      private readonly QuantityDiffBuilder _quantityDiffBuilder;
      private readonly IObjectComparer _objectComparer;

      public ParameterDiffBuilder(QuantityDiffBuilder quantityDiffBuilder, IObjectComparer objectComparer)
      {
         _quantityDiffBuilder = quantityDiffBuilder;
         _objectComparer = objectComparer;
      }

      public override void Compare(IComparison<IParameter> comparison)
      {
         if (!ShouldCompareParametersIn(comparison))
            return;

         _quantityDiffBuilder.Compare(comparison);
         CompareValues(x => x.BuildMode, x => x.BuildMode, comparison);

         if (shouldCompareRHSFormula(comparison))
            _objectComparer.Compare(comparison.ChildComparison(x => x.RHSFormula));
         else
            CompareValues(x => x.RHSFormula != null, Captions.Diff.IsStateVariable, comparison);

         if (comparison.Settings.OnlyComputingRelevant)
            return;

         CompareValues(x => x.Visible, x => x.Visible, comparison);
      }

      private bool shouldCompareRHSFormula(IComparison<IParameter> comparison)
      {
         if (!comparison.ComparedObjectsDefined)
            return false;

         return comparison.Object1.RHSFormula != null && comparison.Object2.RHSFormula != null;
      }

      public static Func<IComparison<IParameter>, bool> ShouldCompareParametersIn { get; set; } = shouldCompareParameterInComparison;

      private static bool shouldCompareParameterInComparison(IComparison<IParameter> comparison) => comparison.ComparedObjectsDefined;
   }
}