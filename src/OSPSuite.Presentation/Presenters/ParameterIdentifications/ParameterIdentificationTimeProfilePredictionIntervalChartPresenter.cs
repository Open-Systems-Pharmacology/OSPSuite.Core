using OSPSuite.Assets;
using OSPSuite.Core.Chart.ParameterIdentifications;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Presentation.Services.Charts;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public interface IParameterIdentificationTimeProfilePredictionIntervalChartPresenter : IParameterIdentificationSingleRunAnalysisPresenter
   {
   }

 
   public class ParameterIdentificationTimeProfilePredictionIntervalChartPresenter : AbstractParameterIdentificationTimeProfileConfidenceIntervalChartPresenter<ParameterIdentificationTimeProfilePredictionIntervalChart>, IParameterIdentificationTimeProfilePredictionIntervalChartPresenter
   {
      public ParameterIdentificationTimeProfilePredictionIntervalChartPresenter(IParameterIdentificationSingleRunAnalysisView view, ChartPresenterContext chartPresenterContext, ITimeProfileConfidenceIntervalCalculator timeProfileConfidenceIntervalCalculator) :
         base(view, chartPresenterContext, timeProfileConfidenceIntervalCalculator, ApplicationIcons.TimeProfilePredictionInterval, PresenterConstants.PresenterKeys.ParameterIdentificationTimeProfilePredictionIntervalChartPresenter, x => x.CalculatePredictionIntervalFor)
      {
      }
   }
}