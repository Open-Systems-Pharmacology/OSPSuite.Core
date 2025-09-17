using System.Linq;
using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Chart.ParameterIdentifications;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_ParameterIdentificationCovarianceMatrixPresenter : ContextSpecification<ParameterIdentificationCovarianceAnalysisPresenter>
   {
      private IPresentationSettingsTask _presentationSettingsTask;
      private IParameterIdentificationSingleRunAnalysisView _view;
      private ParameterIdentification _parameterIdentification;
      private ParameterIdentificationCovarianceMatrix _correlationCovarianceMatrix;
      private IMatrixCalculator _matrixCalculator;
      private IParameterIdentificationMatrixPresenter _matrixPresenter;
      private IParameterIdentificationCovarianceAnalysisView _covarianceView;
      private IParameterIdentificationConfidenceIntervalPresenter _confidenceIntervalPresenter;

      protected override void Context()
      {
         _presentationSettingsTask = A.Fake<IPresentationSettingsTask>();
         _view = A.Fake<IParameterIdentificationSingleRunAnalysisView>();
         _matrixCalculator = A.Fake<IMatrixCalculator>();
         _parameterIdentification = A.Fake<ParameterIdentification>();
         _correlationCovarianceMatrix = A.Fake<ParameterIdentificationCovarianceMatrix>();
         _matrixPresenter = A.Fake<IParameterIdentificationMatrixPresenter>();
         _covarianceView = A.Fake<IParameterIdentificationCovarianceAnalysisView>();
         _confidenceIntervalPresenter = A.Fake<IParameterIdentificationConfidenceIntervalPresenter>();
         A.CallTo(() => _parameterIdentification.Results).Returns(new[] { new ParameterIdentificationRunResult { JacobianMatrix = new JacobianMatrix(new[] { "A" }) } });
         var matrix = new Matrix(new[] { "A" }, new[] { "A" });
         matrix.SetRow(0, new[] { 1d });
         A.CallTo(_matrixCalculator).WithReturnType<Matrix>().Returns(matrix);
         sut = new ParameterIdentificationCovarianceAnalysisPresenter(_view, _covarianceView, _matrixPresenter, _presentationSettingsTask, _matrixCalculator, _confidenceIntervalPresenter);
      }

      public class When_displaying_an_analysis_for_a_parameter_identification : concern_for_ParameterIdentificationCovarianceMatrixPresenter
      {
         protected override void Because()
         {
            sut.InitializeAnalysis(_correlationCovarianceMatrix, _parameterIdentification);
         }

         [Observation]
         public void the_matrix_calculator_is_used_to_calculate_a_correlation_matrix()
         {
            A.CallTo(() => _matrixCalculator.CovarianceMatrixFrom(_parameterIdentification.Results.First().JacobianMatrix, _parameterIdentification.Results.First().BestResult.ResidualsResult)).MustHaveHappened();
         }

         [Observation]
         public void should_set_the_matrix_view_and_the_parameter_confidence_interval_view_into_the_analysis_view()
         {
            A.CallTo(() => _covarianceView.SetMatrixView(_matrixPresenter.View)).MustHaveHappened();
            A.CallTo(() => _covarianceView.SetConfidenceIntevalView(_confidenceIntervalPresenter.View)).MustHaveHappened();
         }
      }

      public class When_displaying_a_covariance_analysis_for_a_parameter_identification_when_sensitivity_calculation_has_failed : concern_for_ParameterIdentificationCovarianceMatrixPresenter
      {
         protected override void Context()
         {
            base.Context();
            _parameterIdentification.Results.First().Status = RunStatus.SensitivityCalculationFailed;
         }

         protected override void Because()
         {
            sut.InitializeAnalysis(_correlationCovarianceMatrix, _parameterIdentification);
         }

         [Observation]
         public void should_show_the_sensitivity_error_in_the_presenter()
         {
            A.CallTo(() => _matrixPresenter.ShowCalculationError(A<string>.That.Matches(x => errorMessageMatchesExpected(x)))).MustHaveHappened();
         }

         private bool errorMessageMatchesExpected(string s)
         {
            return s.Equals(Captions.ParameterIdentification.SensitivityCalculationFailed(_parameterIdentification.Name, new[] { _parameterIdentification.Results.First().Message }));
         }
      }
   }
}