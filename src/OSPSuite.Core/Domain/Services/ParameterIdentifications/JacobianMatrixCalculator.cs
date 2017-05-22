using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Core.Domain.Services.ParameterIdentifications
{
   public interface IJacobianMatrixCalculator
   {
      JacobianMatrix CalculateFor(ParameterIdentification parameterIdentification, OptimizationRunResult runResult, ICache<ISimulation, ISimModelBatch> simModelBatches);
   }

   public class JacobianMatrixCalculator : IJacobianMatrixCalculator
   {
      public JacobianMatrix CalculateFor(ParameterIdentification parameterIdentification, OptimizationRunResult runResult, ICache<ISimulation, ISimModelBatch> simModelBatches)
      {
         var allVariableIdentificationParameters = parameterIdentification.AllVariableIdentificationParameters.ToList();
         var matrix = new JacobianMatrix(allVariableIdentificationParameters.AllNames());

         foreach (var outputMappings in parameterIdentification.AllOutputMappings.GroupBy(x => x.FullOutputPath))
         {
            var outputMapping = outputMappings.ElementAt(0);
            var simModelBatch = simModelBatches[outputMapping.Simulation];
            var fullOutputPath = outputMappings.Key;
            var outputResult = runResult.SimulationResultFor(fullOutputPath);

            matrix.AddRows(jacobianRowFrom(runResult, fullOutputPath, outputResult, allVariableIdentificationParameters, outputMapping, simModelBatch));
            matrix.AddPartialDerivatives(partialDerivativesFrom(fullOutputPath, outputResult, allVariableIdentificationParameters, outputMapping, simModelBatch));
         }

         return matrix;
      }

      private PartialDerivatives partialDerivativesFrom(string fullOutputPath, DataColumn outputResult, IReadOnlyList<IdentificationParameter> allIdentificationParameters, OutputMapping outputMapping, ISimModelBatch simModelBatch)
      {
         var partialDerivatives = new PartialDerivatives(fullOutputPath, allIdentificationParameters.AllNames());

         outputResult.Values.Each((outputValue, index) =>
         {
            var derivativeValues = retrievePartialDerivativeFor(allIdentificationParameters, x => retrievePartialDerivativeForOutputs(x, outputMapping, simModelBatch, index));
            partialDerivatives.AddPartialDerivative(derivativeValues);
         });

         return partialDerivatives;
      }

      private static IEnumerable<JacobianRow> jacobianRowFrom(OptimizationRunResult runResult, string fullOutputPath, DataColumn outputResult, IReadOnlyList<IdentificationParameter> allIdentificationParameters, OutputMapping outputMapping, ISimModelBatch simModelBatch)
      {
         var timeGrid = outputResult.BaseGrid;

         return from residual in runResult.AllResidualsFor(fullOutputPath).Where(x => x.Weight > 0)
            let timeIndex = timeGrid.LeftIndexOf(Convert.ToSingle(residual.Time))
            let outputValue = outputResult[timeIndex]
            let derivativeValues = retrievePartialDerivativeFor(allIdentificationParameters, x => retrievePartialDerivativeForResiduals(x, outputMapping, simModelBatch, timeIndex, outputValue, residual.Weight))
            select new JacobianRow(fullOutputPath, residual.Time, derivativeValues);
      }

      private static double[] retrievePartialDerivativeFor(IReadOnlyList<IdentificationParameter> allIdentificationParameters, Func<ParameterSelection, double> partialDerivativeFunc)
      {
         var derivativeValues = new double[allIdentificationParameters.Count];
         allIdentificationParameters.Each((identificationParameter, i) => { derivativeValues[i] = retrievePartialDerivativeFor(identificationParameter, partialDerivativeFunc); });
         return derivativeValues;
      }

      private static double retrievePartialDerivativeFor(IdentificationParameter identificationParameter, Func<ParameterSelection, double> partialDerivativeFunc)
      {
         double sensitivityValue = 0;
         foreach (var linkedParameter in identificationParameter.AllLinkedParameters)
         {
            var sensitivityForParameter = partialDerivativeFunc(linkedParameter);
            if (identificationParameter.UseAsFactor)
               sensitivityForParameter *= linkedParameter.Parameter.Value;

            sensitivityValue += sensitivityForParameter;
         }
         return sensitivityValue;
      }

      private static double retrievePartialDerivativeForOutputs(ParameterSelection linkedParameter, OutputMapping outputMapping, ISimModelBatch simModelBatch, int timeIndex)
      {
         if (!Equals(linkedParameter.Simulation, outputMapping.Simulation))
            return 0;

         var sensitivity = simModelBatch.SensitivityValuesFor(outputMapping.OutputPath, linkedParameter.Path);
         return sensitivity[timeIndex];
      }

      private static double retrievePartialDerivativeForResiduals(ParameterSelection linkedParameter, OutputMapping outputMapping, ISimModelBatch simModelBatch, int timeIndex, float outputValue, double weight)
      {
         var partialDerivative = retrievePartialDerivativeForOutputs(linkedParameter, outputMapping, simModelBatch, timeIndex);
         var sensitivityValue = partialDerivative * weight;

         if (sensitivityValue == 0 || outputMapping.Scaling == Scalings.Linear)
            return sensitivityValue;

         return sensitivityValue / (outputValue * Math.Log(10));
      }
   }
}