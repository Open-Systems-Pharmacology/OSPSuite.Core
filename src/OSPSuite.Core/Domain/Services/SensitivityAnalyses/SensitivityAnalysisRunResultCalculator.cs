using System;
using System.Linq;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.SensitivityAnalyses;

namespace OSPSuite.Core.Domain.Services.SensitivityAnalyses
{
   public interface ISensitivityAnalysisRunResultCalculator
   {
      SensitivityAnalysisRunResult CreateFor(SensitivityAnalysis sensitivityAnalysis, VariationData variationData, SimulationResults simulationResults);
   }

   public class SensitivityAnalysisRunResultCalculator : ISensitivityAnalysisRunResultCalculator
   {
      private readonly ISensitivityAnalysisPKAnalysesTask _pkAnalysesTask;

      public SensitivityAnalysisRunResultCalculator(ISensitivityAnalysisPKAnalysesTask pkAnalysesTask)
      {
         _pkAnalysesTask = pkAnalysesTask;
      }

      public SensitivityAnalysisRunResult CreateFor(SensitivityAnalysis sensitivityAnalysis, VariationData variationData, SimulationResults simulationResults)
      {
         var sensitivityRunResult = new SensitivityAnalysisRunResult();

         var pkAnalyses = _pkAnalysesTask.CalculateFor(sensitivityAnalysis.Simulation, variationData.NumberOfVariations, simulationResults);
         foreach (var pkParameter in pkAnalyses.All())
         {
            sensitivityAnalysis.AllSensitivityParameters.Each((sensitivityParameter, index) =>
            {
               var pkSensitivity = calculateParameterSensitivity(sensitivityParameter, index, variationData, pkParameter);
               if(pkSensitivity!=null)
                  sensitivityRunResult.AddPKParameterSensitivity(pkSensitivity);
            });
         }

         return sensitivityRunResult;
      }

      private PKParameterSensitivity calculateParameterSensitivity(SensitivityParameter sensitivityParameter, int sensitivityParameterIndex, VariationData variationData, QuantityPKParameter pkParameter)
      {
         var defaultParameterValue = sensitivityParameter.DefaultValue;
         var defaultPKValue = pkParameter.Values[variationData.DefaultVariationId];
         var allVariations = variationData.VariationsFor(sensitivityParameter.Name);

         if (float.IsNaN(defaultPKValue) || defaultPKValue == 0 || defaultParameterValue == 0 || !allVariations.Any())
            return null;

         var sensitivity = new PKParameterSensitivity
         {
            ParameterName = sensitivityParameter.Name,
            PKParameterName = pkParameter.Name,
            QuantityPath = pkParameter.QuantityPath,
            Value = double.NaN
         };
      
         var delta = (from variation in allVariations
            let deltaP = difference(variation.Variation[sensitivityParameterIndex], defaultParameterValue)
            let deltaPK = difference(pkParameter.Values[variation.VariationId], defaultPKValue)
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