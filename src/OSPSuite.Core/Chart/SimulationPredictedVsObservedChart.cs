using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Chart
{
   public class SimulationPredictedVsObservedChart : ChartWithObservedData, ISimulationAnalysis
   {
      public IAnalysable Analysable { get; set; }
   }
}