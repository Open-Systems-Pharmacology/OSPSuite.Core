using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Core.Services
{
   public abstract class concern_for_ConfidenceIntervalCalculator : ContextSpecification<IConfidenceIntervalCalculator>
   {
      protected ResidualsResult _residualsResult;
      protected JacobianMatrix _jacobianMatrix;

      protected override void Context()
      {
         sut = new ConfidenceIntervalCalculator(new MatrixCalculator());
         _residualsResult = new ResidualsResult();
         _jacobianMatrix = new JacobianMatrix(new[] { "P1", "P2" });
         _jacobianMatrix.AddRow(new JacobianRow("O1", 1, new[] { 1d, 5d }));
         _jacobianMatrix.AddRow(new JacobianRow("O1", 2, new[] { 2d, 11d }));
         _jacobianMatrix.AddRow(new JacobianRow("O1", 3, new[] { 3d, 12d }));
         _jacobianMatrix.AddRow(new JacobianRow("O1", 4, new[] { 4d, 2d }));

         _residualsResult.AddOutputResiduals(
            "01",
            new DataRepository("OBS"),
            new[] { new Residual(1, 0.1000,1), new Residual(2, -0.200, 1), new Residual(3, 1.0000, 1), new Residual(4, 0.5000, 1) }
            );
      }
     
   }

   public class When_calculating_the_confidence_interval_of_a_predefined_jacobian_matrix_and_residuals : concern_for_ConfidenceIntervalCalculator
   {
      private ICache<string, double> _result;

      protected override void Because()
      {
         _result = sut.ConfidenceIntervalFrom(_jacobianMatrix, _residualsResult);
      }

      [Observation]
      public void should_return_the_expected_values()
      {
         _result["P1"].ShouldBeEqualTo(0.9676, 1e-2);
         _result["P2"].ShouldBeEqualTo(0.3091, 1e-2);
         double.IsNaN(_result["Unknown"]).ShouldBeTrue();
      }
   }


   public class When_calculating_the_confidence_interval_of_a_predefined_jacobian_matrix_and_residuals_with_a_degree_of_freedom_equal_to_zero : concern_for_ConfidenceIntervalCalculator
   {
      private ICache<string, double> _result;

      protected override void Context()
      {
         base.Context();
         _jacobianMatrix = new JacobianMatrix(new[] { "P1", "P2" });
         _jacobianMatrix.AddRow(new JacobianRow("O1", 1, new[] { 1d, 5d }));
         _jacobianMatrix.AddRow(new JacobianRow("O1", 2, new[] { 2d, 11d }));
      }

      protected override void Because()
      {
         _result = sut.ConfidenceIntervalFrom(_jacobianMatrix, _residualsResult);
      }

      [Observation]
      public void should_return_the_expected_values()
      {
         _result.Count.ShouldBeEqualTo(0);
      }
   }
}