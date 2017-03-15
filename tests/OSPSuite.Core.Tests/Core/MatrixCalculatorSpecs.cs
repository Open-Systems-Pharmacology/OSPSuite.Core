using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;

namespace OSPSuite.Core
{
   public abstract class concern_for_FisherMatrixCalculator : ContextSpecification<IMatrixCalculator>
   {
      protected ResidualsResult _residualsResult;
      protected JacobianMatrix _jacobianMatrix;

      protected override void Context()
      {
         sut = new MatrixCalculator();
         _residualsResult = new ResidualsResult();
         _jacobianMatrix = new JacobianMatrix(new[] {"P1", "P2"});
         _jacobianMatrix.AddRow(new JacobianRow("O1", 1, new[] { 1d, 5d }));
         _jacobianMatrix.AddRow(new JacobianRow("O1", 2, new[] { 2d, 11d }));
         _jacobianMatrix.AddRow(new JacobianRow("O1", 3, new[] { 3d, 12d }));
         _jacobianMatrix.AddRow(new JacobianRow("O1", 4, new[] { 4d, 2d }));

         _residualsResult.AddOutputResiduals(
            "01", 
            new DataRepository("OBS"), 
            new[] { new Residual(1, 0.1000, 1), new Residual(2, -0.200, 1), new Residual(3, 1.0000, 1), new Residual(4, 0.5000, 1) }
            );
      }
   }

   public class When_calculating_the_fisher_matrix_based_on_a_given_jacobi_matrix : concern_for_FisherMatrixCalculator
   {
      private Matrix _fisher;

      protected override void Because()
      {
         _fisher = sut.FisherMatrixFrom(_jacobianMatrix);
      }

      [Observation]
      public void should_return_the_same_results_as_in_matlab()
      {
         _fisher.RowCount.ShouldBeEqualTo(2);
         _fisher.ColumnCount.ShouldBeEqualTo(2);
         _fisher[0][0].ShouldBeEqualTo(30);
         _fisher[0][1].ShouldBeEqualTo(71);
         _fisher[1][0].ShouldBeEqualTo(71);
         _fisher[1][1].ShouldBeEqualTo(294);
      }
   }

   public class When_calculating_the_covariance_matrix_based_on_a_given_jacobi_matrix_and_residual_results : concern_for_FisherMatrixCalculator
   {
      private Matrix _covariance;

    
      protected override void Because()
      {
         _covariance = sut.CovarianceMatrixFrom(_jacobianMatrix,_residualsResult);
      }

      [Observation]
      public void should_return_the_same_value_as_in_matlab()
      {
         _covariance.RowCount.ShouldBeEqualTo(2);
         _covariance.ColumnCount.ShouldBeEqualTo(2);
         _covariance[0][0].ShouldBeEqualTo(0.0506, 1e-2);
         _covariance[0][1].ShouldBeEqualTo(-0.0122, 1e-2);
         _covariance[1][0].ShouldBeEqualTo(-0.0122, 1e-2);
         _covariance[1][1].ShouldBeEqualTo(0.0052, 1e-2);
      }
   }

   public class When_calculating_the_correlation_matrix_based_on_a_given_jacobi_matrix_and_residual_results : concern_for_FisherMatrixCalculator
   {
      private Matrix _correlation;


      protected override void Because()
      {
         _correlation = sut.CorrelationMatrixFrom(_jacobianMatrix, _residualsResult);
      }

      [Observation]
      public void should_return_the_same_value_as_in_matlab()
      {
         _correlation.RowCount.ShouldBeEqualTo(2);
         _correlation.ColumnCount.ShouldBeEqualTo(2);
         _correlation[0][0].ShouldBeEqualTo(1, 1e-2);
         _correlation[0][1].ShouldBeEqualTo(-0.7560, 1e-2);
         _correlation[1][0].ShouldBeEqualTo(-0.7560, 1e-2);
         _correlation[1][1].ShouldBeEqualTo(1, 1e-2);
      }
   }
}