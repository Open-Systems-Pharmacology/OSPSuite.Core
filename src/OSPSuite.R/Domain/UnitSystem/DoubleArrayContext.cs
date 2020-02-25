using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.R.Domain.UnitSystem
{
   public class DoubleArrayContext : IWithDimension
   {
      public IDimension Dimension { get; set; }
      public double? MolWeight { get; set; }

      public DoubleArrayContext(IDimension dimension,  double? molWeight)
      {
         Dimension = dimension;
         MolWeight = molWeight;
      }
   }
}