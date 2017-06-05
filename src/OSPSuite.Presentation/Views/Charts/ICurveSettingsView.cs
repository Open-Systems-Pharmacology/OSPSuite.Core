using System.Collections.Generic;
using OSPSuite.Presentation.DTO.Charts;
using OSPSuite.Presentation.Presenters.Charts;

namespace OSPSuite.Presentation.Views.Charts
{
   public interface ICurveSettingsView : IView<ICurveSettingsPresenter>, IViewWithColumnSettings
   {
      void BindToSource(IEnumerable<CurveDTO> curves);
      void RefreshData();
   }
}