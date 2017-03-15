using OSPSuite.Utility.Collections;
using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Presenters.Charts;

namespace OSPSuite.Presentation.Views.Charts
{
   public interface IAxisSettingsView : IView<IAxisSettingsPresenter>, IViewWithColumnSettings
   {
      void BindToSource(ICache<AxisTypes, IAxis> axes);
      void DeleteBinding();
   }
}