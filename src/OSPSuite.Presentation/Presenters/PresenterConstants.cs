using OSPSuite.Utility.Collections;

namespace OSPSuite.Presentation.Presenters
{
   public static class PresenterConstants
   {
      public static class PresenterKeys 
      {
         private static readonly Cache<string, string> _presenterKeyCache = new Cache<string, string>();

         public static readonly string EditParameterIdentificationPresenter = createConstant("EditParameterIdentificationPresenter");
         public static readonly string ParameterIdentificationTimeProfileChartPresenter = createConstant("ParameterIdentificationTimeProfileChartPresenter");
         public static readonly string ParameterIdentificationResidualVsTimeChartPresenter = createConstant("ParameterIdentificationResidualVsTimeChartPresenter");
         public static readonly string ParameterIdentificationResidualHistogramPresenter = createConstant("ParameterIdentificationResidualHistogramPresenter");
         public static readonly string ParameterIdentificationPredictedVsActualChartPresenter = createConstant("ParameterIdentificationPredictedVsActualChartPresenter");
         public static readonly string ParameterIdentificationCorrelationCovarianceMatrixPresenter = createConstant("ParameterIdentificationCorrelationCovarianceMatrixPresenter");
         public static readonly string ParameterIdentificationTimeProfileConfidenceIntervalChartPresenter = createConstant("ParameterIdentificationTimeProfileConfidenceIntervalChartPresenter");
         public static readonly string ParameterIdentificationTimeProfilePredictionIntervalChartPresenter = createConstant("ParameterIdentificationTimeProfilePredictionIntervalChartPresenter");
         public static readonly string ParameterIdentificationTimeProfileVPCIntervalChartPresenter = createConstant("ParameterIdentificationTimeProfileVPCIntervalChartPresenter");
         public static readonly string SensitivityAnalysisPKParameterAnalysisPresenter = createConstant("SensitivityAnalysisPKParameterAnalysisPresenter");
         public static readonly string EditSensitivityAnalysisPresenter = createConstant("EditSensitivityAnalysisPresenter");

         private static string createConstant(string presenterName)
         {
            _presenterKeyCache.Add(presenterName, presenterName);
            return presenterName;
         }
      }
   }
}