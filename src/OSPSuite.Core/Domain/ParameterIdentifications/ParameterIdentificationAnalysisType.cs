namespace OSPSuite.Core.Domain.ParameterIdentifications
{
   public enum ParameterIdentificationAnalysisType
   {
      TimeProfile,
      PredictedVsObserved,
      ResidualsVsTime,
      ResidualHistogram,
      CorrelationMatrix,
      CovarianceMatrix,
      TimeProfileConfidenceInterval,
      TimeProfilePredictionInterval,
      TimeProfileVPCInterval,
   }
}