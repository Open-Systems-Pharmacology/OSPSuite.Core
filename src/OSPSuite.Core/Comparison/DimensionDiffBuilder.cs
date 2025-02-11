using OSPSuite.Assets;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Comparison
{
   internal class DimensionDiffBuilder : DiffBuilder<Dimension>
   {
      public override void Compare(IComparison<Dimension> comparison)
      {
         CompareValues(x => x.Name, x => Captions.Dimension, comparison);
      }
   }
}