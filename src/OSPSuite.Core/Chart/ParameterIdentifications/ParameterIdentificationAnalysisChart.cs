namespace OSPSuite.Core.Chart.ParameterIdentifications
{
   public class ParameterIdentificationTimeProfileChart : AnalysisChart
   {
      public override AnalysisChartTypes AnalysisChartType => AnalysisChartTypes.TimeProfile;
   }

   public class ParameterIdentificationTimeProfileConfidenceIntervalChart : AnalysisChartWithLocalRepositories
   {
      public override AnalysisChartTypes AnalysisChartType => AnalysisChartTypes.TimeProfile;
   }

   public class ParameterIdentificationTimeProfilePredictionIntervalChart : AnalysisChartWithLocalRepositories
   {
      public override AnalysisChartTypes AnalysisChartType => AnalysisChartTypes.TimeProfile;
   }

   public class ParameterIdentificationTimeProfileVPCIntervalChart : AnalysisChartWithLocalRepositories
   {
      public override AnalysisChartTypes AnalysisChartType => AnalysisChartTypes.TimeProfile;
   }

   public class ParameterIdentificationResidualVsTimeChart : ResidualsVsTimeChart
   {
   }

   public class ParameterIdentificationPredictedVsObservedChart : PredictedVsObservedChart
   {
   }
}