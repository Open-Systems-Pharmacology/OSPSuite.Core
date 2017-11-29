using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Domain.Services.ParameterIdentifications
{
   public interface ITimeProfileConfidenceIntervalCalculator
   {
      IReadOnlyList<DataRepository> CalculateConfidenceIntervalFor(ParameterIdentification parameterIdentification, ParameterIdentificationRunResult runResult);
      IReadOnlyList<DataRepository> CalculatePredictionIntervalFor(ParameterIdentification parameterIdentification, ParameterIdentificationRunResult runResult);
      IReadOnlyList<DataRepository> CalculateVPCIntervalFor(ParameterIdentification parameterIdentification, ParameterIdentificationRunResult runResult);
   }

   public class TimeProfileConfidenceIntervalCalculator : ITimeProfileConfidenceIntervalCalculator
   {
      private readonly double LN10 = Math.Log(10.0);

      private readonly IMatrixCalculator _matrixCalculator;
      private readonly IConfidenceIntervalDataRepositoryCreator _dataRepositoryCreator;
      private double _sigma;
      private Matrix<double> _covP;
      private double _dt;
      private ParameterIdentificationRunResult _runResult;

      public TimeProfileConfidenceIntervalCalculator(IMatrixCalculator matrixCalculator, IConfidenceIntervalDataRepositoryCreator dataRepositoryCreator)
      {
         _matrixCalculator = matrixCalculator;
         _dataRepositoryCreator = dataRepositoryCreator;
      }

      private IReadOnlyList<DataRepository> calculateConfidenceIntervalFor(ParameterIdentification parameterIdentification, ParameterIdentificationRunResult runResult, string confidenceIntervalName, Func<Scalings, double, double, double, double> intervalCalculation, bool requiresActiveOutput)
      {
         var dataRepositories = new List<DataRepository>();
         if (runResult.JacobianMatrix == null)
            return dataRepositories;

         var residualsResult = runResult.BestResult.ResidualsResult;
         var allResiduals = residualsResult.AllResidualsWithWeightsStrictBiggerZero;
         var nr = allResiduals.Count;
         var np = parameterIdentification.AllVariableIdentificationParameters.Count();
         var df = nr - np;
         if (df <= 0)
            return dataRepositories;

         var Q = residualsResult.SumResidual2;

         try
         {
            _sigma = Math.Sqrt(Q / df);
            _runResult = runResult;
            var covariance = _matrixCalculator.CovarianceMatrixFrom(runResult.JacobianMatrix, residualsResult);
            _covP = Matrix<double>.Build.DenseOfRowArrays(covariance.Rows);

            var qt = new StudentT(0.0, 1, df);
            _dt = qt.InverseCumulativeDistribution(1 - Constants.CONFIDENCE_INTERVAL_ALPHA / 2);

            parameterIdentification.AllOutputMappings.GroupBy(x => x.FullOutputPath).Each(x =>
            {
               var outputMapping = x.ElementAt(0);
               var dataRepository = _dataRepositoryCreator.CreateFor(confidenceIntervalName, confidanceIntervalFor(outputMapping, intervalCalculation, requiresActiveOutput), outputMapping, runResult.BestResult);
               if (!dataRepository.IsNull())
                  dataRepositories.Add(dataRepository);
            });

            return dataRepositories;
         }
         finally
         {
            _covP = null;
            _sigma = double.NaN;
            _dt = double.NaN;
            _runResult = null;
         }
      }

      private double[] confidanceIntervalFor(OutputMapping outputMapping, Func<Scalings, double, double, double, double> intervalCalculation, bool requiresActiveOutput)
      {
         var fullOutputPath = outputMapping.FullOutputPath;
         var simulationResult = _runResult.BestResult.SimulationResultFor(fullOutputPath);
         var residualResults = _runResult.BestResult.ResidualsResult.AllOutputResidualsFor(fullOutputPath);
         var jacobian = _runResult.JacobianMatrix;
         var outputPartialDerivatives = jacobian.PartialDerivativesFor(fullOutputPath);
         var w = meanWeightStrictPositiveFor(residualResults);

         if (requiresActiveOutput && w == 0)
            return null;

         var sigmaE = _sigma / w;

         var confidenceIntervalValues = new double[simulationResult.Values.Count];

         simulationResult.Values.Each((outputValue, index0) =>
         {
            var partialDerivatives = outputPartialDerivatives.PartialDerivativeAt(index0);
            var grad = Matrix<double>.Build.DenseOfColumnArrays(partialDerivatives);
            var sigmaPMatrix = grad.Transpose().Multiply(_covP).Multiply(grad);
            sigmaPMatrix = sigmaPMatrix.PointwisePower(0.5);
            var sigmaP = sigmaPMatrix[0, 0];

            confidenceIntervalValues[index0] = intervalCalculation(outputMapping.Scaling, sigmaE, sigmaP, outputValue);
         });

         return confidenceIntervalValues;
      }

      private double confidenceInterval(Scalings scaling, double sigmaE, double sigmaP, double outputValue)
      {
         if (scaling == Scalings.Linear)
            return _dt * sigmaP;

         var sigmaPLog = outputValue != 0 ? sigmaP / outputValue / LN10 : 0;
         return Math.Pow(10, _dt * sigmaPLog);
      }

      private double predictionInterval(Scalings scaling, double sigmaE, double sigmaP, double outputValue)
      {
         if (scaling == Scalings.Linear)
            return _dt * Math.Sqrt(sigmaE * sigmaE + sigmaP * sigmaP);

         var sigmaPLog = outputValue != 0 ? sigmaP / outputValue / LN10 : 0;
         var sigmaELog = outputValue != 0 ? sigmaE : 0;

         return Math.Pow(10, _dt * Math.Sqrt(sigmaELog * sigmaELog + sigmaPLog * sigmaPLog));
      }

      private double predictionVPCInterval(Scalings scaling, double sigmaE, double sigmaP, double outputValue)
      {
         if (scaling == Scalings.Linear)
            return _dt * sigmaE;

         var sigmaELog = outputValue != 0 ? sigmaE : 0;
         return Math.Pow(10, _dt * sigmaELog);
      }

      private static float meanWeightStrictPositiveFor(IReadOnlyList<OutputResiduals> residualResults)
      {
         return residualResults.SelectMany(x => x.Residuals).Where(x => x.Weight > 0).Select(x => x.Weight).ToFloatArray().ArithmeticMean();
      }

      public IReadOnlyList<DataRepository> CalculateConfidenceIntervalFor(ParameterIdentification parameterIdentification, ParameterIdentificationRunResult runResult)
      {
         return calculateConfidenceIntervalFor(parameterIdentification, runResult, Captions.ParameterIdentification.TimeProfileConfidenceIntervalAnalysis, confidenceInterval, requiresActiveOutput: false);
      }

      public IReadOnlyList<DataRepository> CalculatePredictionIntervalFor(ParameterIdentification parameterIdentification, ParameterIdentificationRunResult runResult)
      {
         return calculateConfidenceIntervalFor(parameterIdentification, runResult, Captions.ParameterIdentification.TimeProfilePredictionIntervalAnalysis, predictionInterval, requiresActiveOutput: true);
      }

      public IReadOnlyList<DataRepository> CalculateVPCIntervalFor(ParameterIdentification parameterIdentification, ParameterIdentificationRunResult runResult)
      {
         return calculateConfidenceIntervalFor(parameterIdentification, runResult, Captions.ParameterIdentification.TimeProfileVPCIntervalAnalysis, predictionVPCInterval, requiresActiveOutput: true);
      }
   }
}