using System.Collections.Generic;
using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Presenters.Charts;

namespace OSPSuite.Presentation.Views.Charts
{
   public interface ICurveSettingsView : IView<ICurveSettingsPresenter>, IViewWithColumnSettings
   {
      void BindToSource(IEnumerable<ICurve> curves);
      void RefreshData();
   }
}