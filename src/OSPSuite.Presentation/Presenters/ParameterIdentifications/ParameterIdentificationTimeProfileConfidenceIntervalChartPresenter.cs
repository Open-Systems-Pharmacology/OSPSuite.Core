using OSPSuite.Assets;
using OSPSuite.Core.Chart.ParameterIdentifications;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Presentation.Services.Charts;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public interface IParameterIdentificationTimeProfileConfidenceIntervalChartPresenter : IParameterIdentificationSingleRunAnalysisPresenter
   {
   }

   public class ParameterIdentificationTimeProfileConfidenceIntervalChartPresenter : AbstractParameterIdentificationTimeProfileConfidenceIntervalChartPresenter<ParameterIdentificationTimeProfileConfidenceIntervalChart>, IParameterIdentificationTimeProfileConfidenceIntervalChartPresenter
   {
      public ParameterIdentificationTimeProfileConfidenceIntervalChartPresenter(IParameterIdentificationSingleRunAnalysisView view, ChartPresenterContext chartPresenterContext, ITimeProfileConfidenceIntervalCalculator timeProfileConfidenceIntervalCalculator) :
         base(view, chartPresenterContext, timeProfileConfidenceIntervalCalculator, ApplicationIcons.TimeProfileConfidenceInterval, PresenterConstants.PresenterKeys.ParameterIdentificationTimeProfileConfidenceIntervalChartPresenter, x => x.CalculateConfidenceIntervalFor)
      {
      }
   }
}