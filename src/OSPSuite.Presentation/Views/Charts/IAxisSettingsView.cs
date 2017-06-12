using System.Collections.Generic;
using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Presenters.Charts;

namespace OSPSuite.Presentation.Views.Charts
{
   public interface IAxisSettingsView : IView<IAxisSettingsPresenter>, IViewWithColumnSettings
   {
      void BindToSource(IEnumerable<Axis> axes);
      void DeleteBinding();
   }
}