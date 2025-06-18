using OSPSuite.Core.Chart;

namespace OSPSuite.Core.Snapshots.Mappers
{
   public class ChartSnapshotContext : SnapshotContext
   {
      public IChart Chart { get; }

      public ChartSnapshotContext(IChart chart, SnapshotContext baseContext) : base(baseContext)
      {
         Chart = chart;
      }
   }
}