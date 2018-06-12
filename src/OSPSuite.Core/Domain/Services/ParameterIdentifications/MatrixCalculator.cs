using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Domain.Services.ParameterIdentifications
{
   public interface IMatrixCalculator
   {
      /// <summary>
      ///    Returns the Fisher Matrix based on the <paramref name="jacobianMatrix" />
      /// </summary>
      Matrix FisherMatrixFrom(JacobianMatrix jacobianMatrix);

      /// <summary>
      ///    Returns the Covariance Matrix based on the <paramref name="jacobianMatrix" /> and
      ///    <paramref name="residualsResult" /> or throws an <see cref="MatrixCalculationException"/> if the calculation fails
      /// </summary>
      /// <exception cref="MatrixCalculationException"> is thrown if the calculation fails</exception>
      Matrix CovarianceMatrixFrom(JacobianMatrix jacobianMatrix, ResidualsResult residualsResult);

      /// <summary>
      ///    Returns the Correlation Matrix based on the <paramref name="jacobianMatrix" /> and
      ///    <paramref name="residualsResult" /> or throws an <see cref="MatrixCalculationException"/> if the calculation fails
      /// </summary>
      /// <exception cref="MatrixCalculationException"> is thrown if the calculation fails</exception>
      Matrix CorrelationMatrixFrom(JacobianMatrix jacobianMatrix, ResidualsResult residualsResult);
   }

   public class MatrixCalculator : IMatrixCalculator
   {
      public Matrix FisherMatrixFrom(JacobianMatrix jacobianMatrix)
      {
         return matrixFrom(jacobianMatrix.ParameterNames, calculateFisherMatrix(jacobianMatrix).Fisher);
      }

      public Matrix CovarianceMatrixFrom(JacobianMatrix jacobianMatrix, ResidualsResult residualsResult)
      {
         var dpdpMat = calculateCovarianceMatrix(jacobianMatrix, residualsResult);
         return matrixFrom(jacobianMatrix.ParameterNames, dpdpMat);
      }

      public Matrix CorrelationMatrixFrom(JacobianMatrix jacobianMatrix, ResidualsResult residualsResult)
      {
         try
         {
            //Norm = (1./ sqrt(diag(dpdpMat))) * (1./ sqrt(diag(dpdpMat)))'
            //corr_matrix = dpdpMat.* Norm
            var dpdpMat = calculateCovarianceMatrix(jacobianMatrix, residualsResult);
            dpdpMat.Diagonal();
            var std = dpdpMat.Diagonal();
            std.PointwisePower(-0.5, std);
            var norm1 = Matrix<double>.Build.DenseOfColumnVectors(std);
            var norm = norm1.Multiply(norm1.Transpose());
            var correlationMatrix = dpdpMat.PointwiseMultiply(norm);
            return matrixFrom(jacobianMatrix.ParameterNames, correlationMatrix);
         }
         catch (Exception e)
         {
            throw new MatrixCalculationException(Error.CorrelationMatrixCannotBeCalculated, e);
         }
      }

      private Matrix<double> jacobiDenseMatrixFrom(JacobianMatrix jacobianMatrix)
      {
         return Matrix<double>.Build.DenseOfRowArrays(jacobianMatrix.Rows.Select(x => x.Values));
      }

      private Matrix<double> calculateCovarianceMatrix(JacobianMatrix jacobianMatrix, ResidualsResult residualsResult)
      {
         try
         {
            var resut = calculateFisherMatrix(jacobianMatrix);
            var fisher = resut.Fisher;
            var jacobian = resut.Jacobian;

            var jacobiTransposed = resut.JacobiTransposed;
            var fisherInv = fisher.PseudoInverse();
            var numberOfIdentifiedParameters = jacobianMatrix.ColumnCount;
            var numberOfResiduls = jacobianMatrix.RowCount;

            // covariance matrix = fisher_inv* Jacob'*eye(length(resid))*sum(resid.^2)/(size(Jacob,1)-size(Jacob,2))*Jacob*fisher_inv; 
            var identity = Matrix<double>.Build.DenseIdentity(numberOfResiduls);
            identity = identity.Multiply(residualsResult.SumResidual2 / (numberOfResiduls - numberOfIdentifiedParameters));

            return fisherInv.Multiply(jacobiTransposed).Multiply(identity).Multiply(jacobian).Multiply(fisherInv);
         }
         catch (Exception e)
         {
            throw new MatrixCalculationException(Error.CovarianceMatrixCannotBeCalculated, e);
         }
      }


      private FisherCalculationResults calculateFisherMatrix(JacobianMatrix jacobianMatrix)
      {
         //fisher = Jacob'*Jacob;
         var jacobi = jacobiDenseMatrixFrom(jacobianMatrix);
         var jacobiTransposed = jacobi.Transpose();

         return new FisherCalculationResults
         {
            Jacobian = jacobi,
            JacobiTransposed = jacobiTransposed,
            Fisher = jacobiTransposed.Multiply(jacobi)
         };
      }

      private Matrix matrixFrom(IReadOnlyList<string> parameterNames, Matrix<double> numericMatrix)
      {
         if (numericMatrix == null)
            return null;

         var matrix = new Matrix(parameterNames, parameterNames);

         foreach (var row in numericMatrix.EnumerateRowsIndexed())
         {
            matrix.SetRow(row.Item1, row.Item2.ToArray());
         }
         return matrix;
      }

      private class FisherCalculationResults
      {
         public Matrix<double> Jacobian { get; set; }
         public Matrix<double> JacobiTransposed { get; set; }
         public Matrix<double> Fisher { get; set; }
      }
   }
}