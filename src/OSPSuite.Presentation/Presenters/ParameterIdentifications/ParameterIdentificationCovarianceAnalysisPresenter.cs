using OSPSuite.Assets;
using OSPSuite.Core.Chart.ParameterIdentifications;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public interface IParameterIdentificationCovarianceAnalysisPresenter : IParameterIdentificationSingleRunAnalysisPresenter
   {
   }

   public class ParameterIdentificationCovarianceAnalysisPresenter : ParameterIdentificationMatrixAnalysisPresenter<ParameterIdentificationCovarianceMatrix>, IParameterIdentificationCovarianceAnalysisPresenter
   {
      private readonly IParameterIdentificationConfidenceIntervalPresenter _confidenceIntervalPresenter;

      public ParameterIdentificationCovarianceAnalysisPresenter(IParameterIdentificationSingleRunAnalysisView view, IParameterIdentificationCovarianceAnalysisView covarianceAnalysisView,
         IParameterIdentificationMatrixPresenter matrixPresenter, IPresentationSettingsTask presentationSettingsTask, IMatrixCalculator matrixCalculator, IParameterIdentificationConfidenceIntervalPresenter confidenceIntervalPresenter) :
            base(view, matrixPresenter, presentationSettingsTask, matrixCalculator, ApplicationIcons.CovarianceAnalysis, Captions.ParameterIdentification.CovarianceMatrixNotAvailable)
      {
         _confidenceIntervalPresenter = confidenceIntervalPresenter;
         covarianceAnalysisView.SetMatrixView(matrixPresenter.View);
         covarianceAnalysisView.SetConfidenceIntevalView(_confidenceIntervalPresenter.View);
         view.SetAnalysisView(covarianceAnalysisView);
         AddSubPresenters(_confidenceIntervalPresenter);
      }

      protected override Matrix CalculateMatrix()
      {
         if (SelectedRunResults?.JacobianMatrix == null)
            return null;

         return _matrixCalculator.CovarianceMatrixFrom(SelectedRunResults.JacobianMatrix, SelectedRunResults.BestResult.ResidualsResult);
      }

      protected override void UpdateAnalysis()
      {
         base.UpdateAnalysis();
         _confidenceIntervalPresenter.CalculateConfidenceIntervalFor(_parameterIdentification, SelectedRunResults);
      }
   }
}