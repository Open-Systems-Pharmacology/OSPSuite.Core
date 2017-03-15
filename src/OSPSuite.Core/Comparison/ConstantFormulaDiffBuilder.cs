using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Services;

namespace OSPSuite.Core.Comparison
{
   public class ConstantFormulaDiffBuilder : DiffBuilder<ConstantFormula>
   {
      private readonly IDisplayUnitRetriever _displayUnitRetriever;

      public ConstantFormulaDiffBuilder(IDisplayUnitRetriever displayUnitRetriever)
      {
         _displayUnitRetriever = displayUnitRetriever;
      }

      public override void Compare(IComparison<ConstantFormula> comparison)
      {
         if (!comparison.Settings.CompareHiddenEntities) return;
         CompareDoubleValues(x => x.Value, x => x.Value, comparison,x=> _displayUnitRetriever.PreferredUnitFor(x));
         CompareValues(x => x.Dimension, x => x.Dimension, comparison);
      }
   }
}