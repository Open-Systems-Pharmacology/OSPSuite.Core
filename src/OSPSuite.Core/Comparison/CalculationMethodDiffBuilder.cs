using OSPSuite.Assets;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Comparison
{
   public class CalculationMethodDiffBuilder : DiffBuilder<CalculationMethod>
   {
      public override void Compare(IComparison<CalculationMethod> comparison)
      {
         //No need to compare name and display name as they will be correlated
         CompareStringValues(x => x.DisplayName, Captions.CalculationMethod, comparison);
         CompareStringValues(x => x.Category, x => x.Category, comparison);
      }
   }
}