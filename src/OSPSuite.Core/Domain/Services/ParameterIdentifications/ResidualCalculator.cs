using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using RemoveLLOQMode = OSPSuite.Core.Domain.ParameterIdentifications.RemoveLLOQMode;

namespace OSPSuite.Core.Domain.Services.ParameterIdentifications
{
   public abstract class ResidualCalculator : IResidualCalculator
   {
      private readonly ITimeGridRestrictor _timeGridRestrictor;
      private readonly IDimensionFactory _dimensionFactory;
      private RemoveLLOQMode _removeLLOQMode;

      protected ResidualCalculator(ITimeGridRestrictor timeGridRestrictor, IDimensionFactory dimensionFactory)
      {
         _timeGridRestrictor = timeGridRestrictor;
         _dimensionFactory = dimensionFactory;
         _removeLLOQMode = RemoveLLOQModes.Never;
      }

      public void Initialize(RemoveLLOQMode removeLLOQMode)
      {
         _removeLLOQMode = removeLLOQMode;
      }

      public ResidualsResult Calculate(IReadOnlyList<SimulationRunResults> simulationsResults, IReadOnlyList<OutputMapping> allOutputMappings)
      {
         if (simulationsResults.Any(x => !x.Success))
            return residualResultWithErrorFrom(simulationsResults);
        
         var simulationColumnsCache = new Cache<string, DataColumn>(x => x.PathAsString, x => null);
         simulationsResults.Select(x => x.Results).Each(repo => simulationColumnsCache.AddRange(repo.AllButBaseGrid()));

         var residualResult = new ResidualsResult();

         foreach (var outputMapping in allOutputMappings)
         {
            var simulationColumn = simulationColumnsCache[outputMapping.FullOutputPath];
            if (simulationColumn == null)
            {
               residualResult.ExceptionOccured = true;
               residualResult.ExceptionMessage = Error.CannotFindSimulationResultsForOutput(outputMapping.FullOutputPath);
               return residualResult;
            }

            var outputResiduals = calculateOutputResiduals(simulationColumn, outputMapping);
            residualResult.AddOutputResiduals(outputMapping.FullOutputPath, outputMapping.WeightedObservedData, outputResiduals);
         }

         return residualResult;
      }

      private ResidualsResult residualResultWithErrorFrom(IReadOnlyList<SimulationRunResults> simulationsResults)
      {
         var exceptionMessage = simulationsResults.Select(x => x.Error).Where(x => x != null).ToString("\n");
         return  new ResidualsResult
         {
            ExceptionOccured = true,
            ExceptionMessage = exceptionMessage
         };
      }

      private IReadOnlyList<Residual> calculateOutputResiduals(DataColumn simulationColumn, OutputMapping outputMapping)
      {
         var observedData = outputMapping.WeightedObservedData.ObservedData;
         var observedValueColumn = observedData.ObservationColumns().First();
         var observedTimeColumn = observedData.BaseGrid;
         var observedTimeIndices = _timeGridRestrictor.GetRelevantIndices(observedData, _removeLLOQMode, outputMapping.Scaling);
         var outputResiduals = new List<Residual>(observedTimeIndices.Count);
         var lloq = observedValueColumn.DataInfo.LLOQ ?? 0;
         var mergedDimension = _dimensionFactory.MergedDimensionFor(simulationColumn);
         var currentObservedDataUnit = observedValueColumn.Dimension.BaseUnit;
         var residualCalculatorFunc = residualMethodFor(outputMapping);

         lloq = convertToBaseUnit(mergedDimension, currentObservedDataUnit, lloq);

         foreach (var index in observedTimeIndices)
         {
            var weight = outputMapping.Weight * outputMapping.WeightedObservedData.Weights[index];
            var observedValue = convertToBaseUnit(mergedDimension, currentObservedDataUnit, observedValueColumn[index]);
            if(!observedValue.IsValid())
               continue;

            var observedTime = observedTimeColumn[index];
            var simulatedValue = simulationColumn.GetValue(observedTime);
            if(!simulatedValue.IsValid())
               continue;

            outputResiduals.Add(new Residual(observedTime, weight * residualCalculatorFunc(simulatedValue, observedValue, lloq), weight));
         }

         return outputResiduals;
      }

      private static float convertToBaseUnit(IDimension mergedDimension, Unit currentUnit, float value)
      {
         return Convert.ToSingle(mergedDimension.UnitValueToBaseUnitValue(currentUnit, value));
      }

      protected abstract double LogResidual(float simulatedValue, float observedValue, float lloq);
      protected abstract double LinResidual(float simulatedValue, float observedValue, float lloq);

      private Func<float, float, float, double> residualMethodFor(OutputMapping outputMapping)
      {
         if (outputMapping.Scaling == Scalings.Log)
            return LogResidual;

         return LinResidual;
      }

      protected double LogSafe(float x)
      {
         if (x < Constants.LOG_SAFE_EPSILON)
            x = Constants.LOG_SAFE_EPSILON;

         return Math.Log10(x);
      }
   }
}