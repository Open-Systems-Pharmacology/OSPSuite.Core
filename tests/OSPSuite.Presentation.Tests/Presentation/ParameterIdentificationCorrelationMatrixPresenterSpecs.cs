using System.Linq;
using FakeItEasy;
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
   public abstract class concern_for_ParameterIdentificationCorrelationMatrixPresenter : ContextSpecification<ParameterIdentificationCorrelationAnalysisPresenter>
   {
      private IPresentationSettingsTask _presentationSettingsTask;
      protected IParameterIdentificationSingleRunAnalysisView _view;
      protected ParameterIdentification _parameterIdentification;
      protected ParameterIdentificationCorrelationMatrix _correlationCovarianceMatrix;
      protected IMatrixCalculator _matrixCalculator;
      protected IParameterIdentificationMatrixPresenter _matrixPresenter;
      protected Matrix _matrix;

      protected override void Context()
      {
         _presentationSettingsTask = A.Fake<IPresentationSettingsTask>();
         _view = A.Fake<IParameterIdentificationSingleRunAnalysisView>();
         _matrixCalculator = A.Fake<IMatrixCalculator>();
         _parameterIdentification = A.Fake<ParameterIdentification>();
         _correlationCovarianceMatrix = A.Fake<ParameterIdentificationCorrelationMatrix>();
         _matrixPresenter = A.Fake<IParameterIdentificationMatrixPresenter>();

         A.CallTo(() => _parameterIdentification.Results).Returns(new[] {new ParameterIdentificationRunResult {JacobianMatrix = new JacobianMatrix(new[] {"A"})}});
         _matrix = new Matrix(new[] {"A"}, new[] {"A"});
         _matrix.SetRow(0, new[] {1d});
         sut = new ParameterIdentificationCorrelationAnalysisPresenter(_view, _matrixPresenter, _presentationSettingsTask, _matrixCalculator);
      }
   }

   public class When_displaying_a_correlation_analysis_for_a_parameter_identification : concern_for_ParameterIdentificationCorrelationMatrixPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(_matrixCalculator).WithReturnType<Matrix>().Returns(_matrix);
      }

      protected override void Because()
      {
         sut.InitializeAnalysis(_correlationCovarianceMatrix, _parameterIdentification);
      }

      [Observation]
      public void the_matrix_calculator_is_used_to_calculate_a_correlation_matrix()
      {
         A.CallTo(() => _matrixCalculator.CorrelationMatrixFrom(_parameterIdentification.Results.First().JacobianMatrix, _parameterIdentification.Results.First().BestResult.ResidualsResult)).MustHaveHappened();
      }

      [Observation]
      public void the_matrix_view_must_be_made_part_of_the_main_view()
      {
         A.CallTo(() => _view.SetAnalysisView(_matrixPresenter.View)).MustHaveHappened();
      }

      [Observation]
      public void the_view_must_be_bound_to_the_selected_result()
      {
         A.CallTo(() => _view.BindToSelectedRunResult()).MustHaveHappened();
      }

      [Observation]
      public void should_bind_the_data_table_to_the_view()
      {
         A.CallTo(() => _matrixPresenter.Edit(_matrix)).MustHaveHappened();
      }
   }

   public class When_displaying_a_correlation_analysis_for_a_parameter_identification_and_the_calculation_fails : concern_for_ParameterIdentificationCorrelationMatrixPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(_matrixCalculator).WithReturnType<Matrix>().Throws(x => new MatrixCalculationException("XX"));
      }

      protected override void Because()
      {
         sut.InitializeAnalysis(_correlationCovarianceMatrix, _parameterIdentification);
      }

      [Observation]
      public void should_show_the_calculation_error_in_the_presenter()
      {
         A.CallTo(() => _matrixPresenter.ShowCalculationError("XX")).MustHaveHappened();
      }
   }
}