using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Services.SensitivityAnalyses
{
   public interface ISensitivityAnalysisRunResultCalculator
   {
      SensitivityAnalysisRunResult CreateFor(SensitivityAnalysis sensitivityAnalysis, VariationData variationData, SimulationResults simulationResults, IReadOnlyList<IndividualRunInfo> runErrors, bool addOutputParameterSensitivitiesToResult);
   }

   public class SensitivityAnalysisRunResultCalculator : ISensitivityAnalysisRunResultCalculator
   {
      private readonly ISensitivityAnalysisPKAnalysesTask _pkAnalysesTask;

      public SensitivityAnalysisRunResultCalculator(ISensitivityAnalysisPKAnalysesTask pkAnalysesTask)
      {
         _pkAnalysesTask = pkAnalysesTask;
      }

      public SensitivityAnalysisRunResult CreateFor(SensitivityAnalysis sensitivityAnalysis, VariationData variationData, SimulationResults simulationResults, IReadOnlyList<IndividualRunInfo> runErrors, bool addOutputParameterSensitivitiesToResult)
      {
         var sensitivityRunResult = new SensitivityAnalysisRunResult
         {
            Errors = runErrors
         };

         addPKAnalysisSensitivities(variationData, simulationResults, sensitivityRunResult, sensitivityAnalysis);

         if (addOutputParameterSensitivitiesToResult)
            addOutputSensitivities(variationData, simulationResults, sensitivityRunResult, sensitivityAnalysis);

         return sensitivityRunResult;
      }

      private void addOutputSensitivities(VariationData variationData, SimulationResults simulationResults, SensitivityAnalysisRunResult sensitivityRunResult, SensitivityAnalysis sensitivityAnalysis)
      {
         variationData.AllVariations.Each(variation =>
         {
            //one variation corresponds to one row in the simulation table (e.g. one IndividualId)
            //this is the variation of one parameter compared to the base simulation
            var resultsForVariation = simulationResults.ResultsFor(variation.VariationId);

            //Retrieve this parameter
            var sensitivityParameter = sensitivityAnalysis.SensitivityParameterByName(variation.ParameterName);
            var parameterPath = sensitivityParameter.ParameterSelection.Path;

            //For all output, we add the sensitivity
            resultsForVariation.AllValues.Each(outputValue =>
            {
               var outputParameterSensitivity = calculateOutputParameterSensitivity(outputValue, variation, parameterPath);
               sensitivityRunResult.AddOutputParameterSensitivity(outputParameterSensitivity);
            });
         });
      }

      private OutputParameterSensitivity calculateOutputParameterSensitivity(QuantityValues outputValue, ParameterVariation variationData, string parameterPath)
      {
         return new OutputParameterSensitivity(variationData.ParameterName, parameterPath, variationData.ParameterValue, outputValue.QuantityPath, outputValue.Values, outputValue.Time.Values);
      }

      private void addPKAnalysisSensitivities(VariationData variationData, SimulationResults simulationResults, SensitivityAnalysisRunResult sensitivityRunResult, SensitivityAnalysis sensitivityAnalysis)
      {
         var pkAnalyses = _pkAnalysesTask.CalculateFor(sensitivityAnalysis.Simulation, simulationResults);
         foreach (var pkParameter in pkAnalyses.All())
         {
            sensitivityAnalysis.AllSensitivityParameters.Each(sensitivityParameter =>
            {
               var pkSensitivity = calculatePKParameterSensitivity(sensitivityParameter, variationData, pkParameter);
               if (pkSensitivity != null)
                  sensitivityRunResult.AddPKParameterSensitivity(pkSensitivity);
            });
         }
      }

      private PKParameterSensitivity calculatePKParameterSensitivity(SensitivityParameter sensitivityParameter, VariationData variationData, QuantityPKParameter pkParameter)
      {
         var defaultParameterValue = sensitivityParameter.DefaultValue;
         var defaultPKValue = pkParameter.ValueFor(variationData.DefaultVariationId);
         var allVariations = variationData.VariationsFor(sensitivityParameter.Name);

         if (float.IsNaN(defaultPKValue) || defaultPKValue == 0 || defaultParameterValue == 0 || !allVariations.Any())
            return null;

         var sensitivity = new PKParameterSensitivity
         {
            ParameterName = sensitivityParameter.Name,
            PKParameterName = pkParameter.Name,
            QuantityPath = pkParameter.QuantityPath,
            ParameterPath = sensitivityParameter.ParameterSelection.Path,
            Value = double.NaN
         };

         var delta = (from variation in allVariations
            let deltaP = difference(variation.ParameterValue, defaultParameterValue)
            let deltaPK = difference(pkParameter.ValueFor(variation.VariationId), defaultPKValue)
            select deltaPK / deltaP).Sum();

         sensitivity.Value = delta * defaultParameterValue / defaultPKValue / allVariations.Count;

         if (Math.Abs(sensitivity.Value) < Constants.SENSITIVITY_THRESHOLD)
            sensitivity.Value = 0.0;

         return sensitivity;
      }

      private double difference(double value, double defaultValue)
      {
         return value - defaultValue;
      }
   }
}