using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Views;
using OSPSuite.Starter.Presenters;

namespace OSPSuite.Starter.Views
{
   public interface IHistogramTestView : IView<IHistogramTestPresenter>
   {
      void PlotPopulationData(ContinuousDistributionData distributionData, DistributionSettings distributionSettings);
   }
}