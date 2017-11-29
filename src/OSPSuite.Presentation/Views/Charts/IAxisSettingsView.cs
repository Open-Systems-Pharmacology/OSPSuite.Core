using System.Collections.Generic;
using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Presenters.Charts;

namespace OSPSuite.Presentation.Views.Charts
{
   public interface IAxisSettingsView : IView<IAxisSettingsPresenter>, IViewWithColumnSettings
   {
      void BindTo(IEnumerable<Axis> axes);
      void DeleteBinding();
   }
}