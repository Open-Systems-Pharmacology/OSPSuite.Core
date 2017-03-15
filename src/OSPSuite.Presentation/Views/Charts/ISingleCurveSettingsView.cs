using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Presenters.Charts;

namespace OSPSuite.Presentation.Views.Charts
{
   public interface ISingleCurveSettingsView : IView<ISingleCurveSettingsPresenter>
   {
      void BindTo(ICurve curve);
   }
}