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

      /// <summary>
      ///    Constant formulas of quantities are compared as values by the quantity comparison when hidden entities are not
      ///    compared. A constant formula reaching this comparison is used directly (reaction or transport kinetic, molecule
      ///    start formula, ...) and is always compared, as its value is part of the model.
      /// </summary>
      public override void Compare(IComparison<ConstantFormula> comparison)
      {
         CompareDoubleValues(x => x.Value, x => x.Value, comparison,x=> _displayUnitRetriever.PreferredUnitFor(x));
         CompareValues(x => x.Dimension, x => x.Dimension, comparison);
      }
   }
}