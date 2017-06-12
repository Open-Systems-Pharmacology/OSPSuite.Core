using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Views.Charts;

namespace OSPSuite.Presentation.Presenters.Charts
{
   public interface ISingleCurveSettingsPresenter : IPresenter<ISingleCurveSettingsView>, IDisposablePresenter
   {
      void Edit(IChart subjectChart, Curve subject);
   }

   public class SingleCurveSettingsPresenter : AbstractDisposablePresenter<ISingleCurveSettingsView, ISingleCurveSettingsPresenter>, ISingleCurveSettingsPresenter
   {
      private readonly IChartUpdater _chartUpdater;

      public SingleCurveSettingsPresenter(ISingleCurveSettingsView view, IChartUpdater chartUpdater) : base(view)
      {
         _chartUpdater = chartUpdater;
         view.CancelVisible = false;
      }

      public void Edit(IChart subjectChart, Curve subject)
      {
         _view.BindTo(subject);

         _view.Display();
         if(_view.Canceled)
            return;

         _chartUpdater.Update(subjectChart);
      }
   }
}