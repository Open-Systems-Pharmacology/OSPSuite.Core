namespace OSPSuite.Core.Chart.ParameterIdentifications
{
   public class ParameterIdentificationTimeProfileChart : AnalysisChart
   {
   }

   public class ParameterIdentificationTimeProfileConfidenceIntervalChart : AnalysisChartWithLocalRepositories
   {
   }

   public class ParameterIdentificationTimeProfilePredictionIntervalChart : AnalysisChartWithLocalRepositories
   {
   }

   public class ParameterIdentificationTimeProfileVPCIntervalChart : AnalysisChartWithLocalRepositories
   {
   }

   public class ParameterIdentificationResidualVsTimeChart : AnalysisChartWithLocalRepositories
   {
   }

   public class ParameterIdentificationPredictedVsObservedChart : PredictedVsObservedChart
   {
      public ParameterIdentificationPredictedVsObservedChart()
      {
         ChartSettings.LegendPosition = LegendPositions.BottomInside;
      }
   }
}