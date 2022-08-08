namespace OSPSuite.Core.Chart.Simulations
{
   public class SimulationPredictedVsObservedChart : PredictedVsObservedChart
   {
      public SimulationPredictedVsObservedChart()
      {
         ChartSettings.LegendPosition = LegendPositions.BottomInside;
      }
   }

   public class SimulationResidualVsTimeChart : AnalysisChartWithLocalRepositories
   {
   }
}