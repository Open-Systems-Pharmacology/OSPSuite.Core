using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Views.Charts;

namespace OSPSuite.Presentation.Presenters.Charts
{
   public interface ISingleCurveSettingsPresenter : IPresenter<ISingleCurveSettingsView>, IDisposablePresenter
   {
      void Edit(IChart chart, Curve curve);
   }

   public class SingleCurveSettingsPresenter : AbstractDisposablePresenter<ISingleCurveSettingsView, ISingleCurveSettingsPresenter>, ISingleCurveSettingsPresenter
   {
      private readonly IChartUpdater _chartUpdater;
      private Curve _cloneCurve;

      public SingleCurveSettingsPresenter(ISingleCurveSettingsView view, IChartUpdater chartUpdater) : base(view)
      {
         _chartUpdater = chartUpdater;
      }

      public void Edit(IChart chart, Curve curve)
      {
         _cloneCurve = curve.Clone();
         _view.BindTo(_cloneCurve);

         _view.Display();
         if(_view.Canceled)
            return;

         curve.UpdateFrom(_cloneCurve);
         _chartUpdater.Update(chart);
      }
   }
}