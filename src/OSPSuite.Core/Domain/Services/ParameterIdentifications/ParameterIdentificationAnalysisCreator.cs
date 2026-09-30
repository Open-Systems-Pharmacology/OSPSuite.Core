using System;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.ParameterIdentifications;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Utility.Extensions;

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
         if (curvesReferenceRunResults(simulationAnalysis))
            return _cloneManager.Clone(simulationAnalysis.DowncastTo<AnalysisChart>());

         return base.CreateAnalysisBasedOn(simulationAnalysis);
      }

      /// <summary>
      ///    Curves of an analysis chart without local repositories reference the columns of the parameter identification
      ///    run results. Those results are not available when deserializing the chart on its own, so the curves would be lost.
      /// </summary>
      private static bool curvesReferenceRunResults(ISimulationAnalysis simulationAnalysis)
      {
         return simulationAnalysis is AnalysisChart && !(simulationAnalysis is AnalysisChartWithLocalRepositories);
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