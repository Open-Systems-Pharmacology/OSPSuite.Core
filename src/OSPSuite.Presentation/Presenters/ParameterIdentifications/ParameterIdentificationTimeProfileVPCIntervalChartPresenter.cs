using OSPSuite.Assets;
using OSPSuite.Core.Chart.ParameterIdentifications;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Presentation.Services.Charts;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public interface IParameterIdentificationTimeProfileVPCIntervalChartPresenter: IParameterIdentificationSingleRunAnalysisPresenter
   {
   }

   public class ParameterIdentificationTimeProfileVPCIntervalChartPresenter : AbstractParameterIdentificationTimeProfileConfidenceIntervalChartPresenter<ParameterIdentificationTimeProfileVPCIntervalChart>, IParameterIdentificationTimeProfileVPCIntervalChartPresenter
   {
      public ParameterIdentificationTimeProfileVPCIntervalChartPresenter(IParameterIdentificationSingleRunAnalysisView view, ChartPresenterContext chartPresenterContext, ITimeProfileConfidenceIntervalCalculator timeProfileConfidenceIntervalCalculator) :
         base(view, chartPresenterContext, timeProfileConfidenceIntervalCalculator, ApplicationIcons.TimeProfileVPCInterval, PresenterConstants.PresenterKeys.ParameterIdentificationTimeProfileVPCIntervalChartPresenter, x => x.CalculateVPCIntervalFor)
      {
      }
   }
}