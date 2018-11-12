using OSPSuite.Assets;
using OSPSuite.Utility.Format;
using OSPSuite.Core.Chart.ParameterIdentifications;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public interface IParameterIdentificationCorrelationAnalysisPresenter : IParameterIdentificationSingleRunAnalysisPresenter
   {
   }

   public class ParameterIdentificationCorrelationAnalysisPresenter : ParameterIdentificationMatrixAnalysisPresenter<ParameterIdentificationCorrelationMatrix>, IParameterIdentificationCorrelationAnalysisPresenter
   {
      public ParameterIdentificationCorrelationAnalysisPresenter(IParameterIdentificationSingleRunAnalysisView view, IParameterIdentificationMatrixPresenter matrixPresenter, IPresentationSettingsTask presentationSettingsTask, IMatrixCalculator matrixCalculator) :
         base(view, matrixPresenter, presentationSettingsTask, matrixCalculator, ApplicationIcons.CorrelationAnalysis, Captions.ParameterIdentification.CorrelationMatrixNotAvailable)
      {
         matrixPresenter.NumberFormatter = new NumericFormatter<double>(new NumericFormatterOptions {AllowsScientificNotation = false, DecimalPlace = NumericFormatterOptions.Instance.DecimalPlace});
         view.SetAnalysisView(matrixPresenter.View);
      }

      protected override Matrix CalculateMatrix()
      {
         if (SelectedRunResults?.JacobianMatrix == null)
            return null;

         return _matrixCalculator.CorrelationMatrixFrom(SelectedRunResults.JacobianMatrix, SelectedRunResults.BestResult.ResidualsResult);
      }
   }
}