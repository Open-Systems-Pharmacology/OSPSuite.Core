using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core.Snapshots
{
   public class DistributionMetaData
   {
      public double Mean { get; set; }
      public double Deviation { get; set; }
      public DistributionType Distribution { get; set; }
   }
}