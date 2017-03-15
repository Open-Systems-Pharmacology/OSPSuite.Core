using System;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using MathNet.Numerics.Distributions;
using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Core.Domain.Services.ParameterIdentifications
{
   public interface IConfidenceIntervalCalculator
   {
      /// <summary>
      ///    Returns a cache {ParmeterPath, Value} containing the confidence interval for each parameter of the parameter
      ///    identification. For an unknown path, the cache will return NaN
      /// </summary>
      ICache<string, double> ConfidenceIntervalFrom(JacobianMatrix jacobianMatrix, ResidualsResult residualsResult);
   }

   public class ConfidenceIntervalCalculator : IConfidenceIntervalCalculator
   {
      private readonly IMatrixCalculator _matrixCalculator;

      public ConfidenceIntervalCalculator(IMatrixCalculator matrixCalculator)
      {
         _matrixCalculator = matrixCalculator;
      }

      public ICache<string, double> ConfidenceIntervalFrom(JacobianMatrix jacobianMatrix, ResidualsResult residualsResult)
      {
         var confidenceInterval = new Cache<string, double>(x => double.NaN);
         Matrix covarianceMatrix;
         try
         {
            covarianceMatrix = _matrixCalculator.CovarianceMatrixFrom(jacobianMatrix, residualsResult);
         }
         catch (MatrixCalculationException)
         {
            return confidenceInterval;
         }

         if (covarianceMatrix == null)
            return confidenceInterval;

         var numberOfParameters = jacobianMatrix.ColumnCount;
         var numberOfData = jacobianMatrix.RowCount;
         var df = numberOfData - numberOfParameters;

         if (df <= 0)
            return confidenceInterval;

         var t = new StudentT(0.0, 1, df);
         var dt = t.InverseCumulativeDistribution(1 - Constants.CONFIDENCE_INTERVAL_ALPHA / 2);

         jacobianMatrix.ParameterNames.Each((path, index) =>
         {
            var value = Math.Sqrt(covarianceMatrix[index][index]);
            confidenceInterval.Add(path, dt * value);
         });

         return confidenceInterval;
      }
   }
}