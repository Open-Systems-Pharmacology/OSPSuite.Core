using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Comparison
{
   public class QuantiySelectionDiffBuilder : DiffBuilder<QuantitySelection>
   {
      public override void Compare(IComparison<QuantitySelection> comparison)
      {
         CompareValues(x => x.Path, x => x.Path, comparison);
         CompareValues(x => x.QuantityType, x => x.QuantityType, comparison);
      }
   }
}