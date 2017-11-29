using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Presenters.Charts;

namespace OSPSuite.Presentation.Views.Charts
{
   public interface ISingleCurveSettingsView : IModalView<ISingleCurveSettingsPresenter>
   {
      void BindTo(Curve curve);
   }
}