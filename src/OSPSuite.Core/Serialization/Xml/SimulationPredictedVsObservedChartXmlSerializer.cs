
using OSPSuite.Core.Chart.Simulations;
using OSPSuite.Core.Serialization.Chart;

namespace OSPSuite.Core.Serialization.Xml
{
   public class SimulationPredictedVsObservedChartXmlSerializer : CurveChartXmlSerializer<SimulationPredictedVsObservedChart>
   {
      public override void PerformMapping()
         {
            base.PerformMapping();
            Map(x => x.DeviationFoldValues);
         }
   }
}
