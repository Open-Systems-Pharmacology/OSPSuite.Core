using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Services;

namespace OSPSuite.Core.Comparison
{
   public class ConstantFormulaDiffBuilder : DiffBuilder<ConstantFormula>
   {
      private readonly IObjectComparer _objectComparer;
      private readonly IDisplayUnitRetriever _displayUnitRetriever;

      public ConstantFormulaDiffBuilder(IObjectComparer objectComparer, IDisplayUnitRetriever displayUnitRetriever)
      {
         _objectComparer = objectComparer;
         _displayUnitRetriever = displayUnitRetriever;
      }

      public override void Compare(IComparison<ConstantFormula> comparison)
      {
         if (!comparison.Settings.CompareHiddenEntities) return;
         CompareDoubleValues(x => x.Value, x => x.Value, comparison,x=> _displayUnitRetriever.PreferredUnitFor(x));
         _objectComparer.Compare(comparison.DimensionComparison());
      }
   }
}