using System;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.ParameterIdentifications;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Core.Domain.Services.ParameterIdentifications
{
   public interface IParameterIdentificationAnalysisCreator : ISimulationAnalysisCreator
   {
      ISimulationAnalysis CreateAnalysisFor(ParameterIdentification parameterIdentification, ParameterIdentificationAnalysisType parameterIdentificationAnalysisType);
   }

   public class ParameterIdentificationAnalysisCreator : ParameterAnalysableAnalysisCreator, IParameterIdentificationAnalysisCreator
   {
      private readonly IChartFactory _chartFactory;
      private readonly ICloneManagerForModel _cloneManager;

      public ParameterIdentificationAnalysisCreator(IChartFactory chartFactory, IOSPSuiteExecutionContext context,IContainerTask containerTask,  IIdGenerator idGenerator , IObjectIdResetter objectIdResetter, ICloneManagerForModel cloneManager) : base(containerTask, context, objectIdResetter, idGenerator)
      {
         _chartFactory = chartFactory;
         _cloneManager = cloneManager;
      }

      public override ISimulationAnalysis CreateAnalysisBasedOn(ISimulationAnalysis simulationAnalysis)
      {
         //Charts are copied with the clone manager rather than by serialization, for two reasons:
         //1. Time profile and predicted vs observed charts plot the simulation results of the parameter identification runs,
         //   which are not part of the chart. Serializing such a chart only writes references to those results, and they cannot
         //   be resolved when the chart is deserialized on its own, so its simulation curves would be lost.
         //2. Charts with local repositories (residuals vs time, confidence, prediction and VPC intervals) need copies of their
         //   repositories with new ids. A copy made by serialization keeps the repository and column ids of the source chart:
         //   the residual curves of the copy are shown twice, and the parameter identification cannot be loaded again
         //   because the same ids are found in both charts.
         if (simulationAnalysis is AnalysisChart analysisChart)
            return _cloneManager.Clone(analysisChart);

         return base.CreateAnalysisBasedOn(simulationAnalysis);
      }

      public ISimulationAnalysis CreateAnalysisFor(ParameterIdentification parameterIdentification, ParameterIdentificationAnalysisType parameterIdentificationAnalysisType)
      {
         switch (parameterIdentificationAnalysisType)
         {
            case ParameterIdentificationAnalysisType.TimeProfile:
               return createTimeProfileAnalysisFor(parameterIdentification);
            case ParameterIdentificationAnalysisType.ResidualsVsTime:
               return createResidualVsTimeAnalysisFor(parameterIdentification);
            case ParameterIdentificationAnalysisType.ResidualHistogram:
               return createHistogramAnalysisFor(parameterIdentification);
            case ParameterIdentificationAnalysisType.PredictedVsObserved:
               return createPredictedVsObservedAnalysisFor(parameterIdentification);
            case ParameterIdentificationAnalysisType.CorrelationMatrix:
               return createCorrelationAnalysisFor(parameterIdentification);
            case ParameterIdentificationAnalysisType.CovarianceMatrix:
               return createCovarianceAnalysisFor(parameterIdentification);
            case ParameterIdentificationAnalysisType.TimeProfileConfidenceInterval:
               return createTimeProfileConfidenceInterval(parameterIdentification);
            case ParameterIdentificationAnalysisType.TimeProfilePredictionInterval:
               return createTimeProfilePredictionInterval(parameterIdentification);
            case ParameterIdentificationAnalysisType.TimeProfileVPCInterval:
               return createTimeProfileVPCInterval(parameterIdentification);
            default:
               throw new ArgumentOutOfRangeException(nameof(parameterIdentificationAnalysisType), parameterIdentificationAnalysisType, null);
         }
      }

      private ISimulationAnalysis createTimeProfileVPCInterval(ParameterIdentification parameterIdentification)
      {
         return createAnalysisFor<ParameterIdentificationTimeProfileVPCIntervalChart>(parameterIdentification);
      }

      private ISimulationAnalysis createTimeProfilePredictionInterval(ParameterIdentification parameterIdentification)
      {
         return createAnalysisFor<ParameterIdentificationTimeProfilePredictionIntervalChart>(parameterIdentification);
      }

      private ISimulationAnalysis createTimeProfileConfidenceInterval(ParameterIdentification parameterIdentification)
      {
         return createAnalysisFor<ParameterIdentificationTimeProfileConfidenceIntervalChart>(parameterIdentification);
      }

      private ISimulationAnalysis createCovarianceAnalysisFor(ParameterIdentification parameterIdentification)
      {
         return createAnalysisFor<ParameterIdentificationCovarianceMatrix>(parameterIdentification);
      }

      private ISimulationAnalysis createCorrelationAnalysisFor(ParameterIdentification parameterIdentification)
      {
         return createAnalysisFor<ParameterIdentificationCorrelationMatrix>(parameterIdentification);
      }

      private ISimulationAnalysis createPredictedVsObservedAnalysisFor(ParameterIdentification parameterIdentification)
      {
         return createChartAnalysisFor<ParameterIdentificationPredictedVsObservedChart>(parameterIdentification);
      }

      private ISimulationAnalysis createHistogramAnalysisFor(ParameterIdentification parameterIdentification)
      {
         return createAnalysisFor<ParameterIdentificationResidualHistogram>(parameterIdentification);
      }

      private ISimulationAnalysis createTimeProfileAnalysisFor(ParameterIdentification parameterIdentification)
      {
         return createChartAnalysisFor<ParameterIdentificationTimeProfileChart>(parameterIdentification);
      }

      private ISimulationAnalysis createResidualVsTimeAnalysisFor(ParameterIdentification parameterIdentification)
      {
         return createChartAnalysisFor<ParameterIdentificationResidualVsTimeChart>(parameterIdentification);
      }

      private T createChartAnalysisFor<T>(ParameterIdentification parameterIdentification) where T : CurveChart, ISimulationAnalysis
      {
         var chart = _chartFactory.Create<T>();
         AddSimulationAnalysisTo(parameterIdentification, chart);
         return chart;
      }

      private T createAnalysisFor<T>(ParameterIdentification parameterIdentification) where T : ISimulationAnalysis, new()
      {
         return AnalysisFor<T>(parameterIdentification);
      }
   }
}