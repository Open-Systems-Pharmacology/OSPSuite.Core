using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Views.Charts;

namespace OSPSuite.Presentation.Presenters.Charts
{
   public interface ISingleCurveSettingsPresenter : IPresenter<ISingleCurveSettingsView>, IDisposablePresenter
   {
      void Edit(ICurve subject);
   }

   public class SingleCurveSettingsPresenter : AbstractDisposablePresenter<ISingleCurveSettingsView, ISingleCurveSettingsPresenter>, ISingleCurveSettingsPresenter
   {
      public SingleCurveSettingsPresenter(ISingleCurveSettingsView view) : base(view)
      {
      }

      public void Edit(ICurve subject)
      {
         _view.BindTo(subject);
      }
   }
}