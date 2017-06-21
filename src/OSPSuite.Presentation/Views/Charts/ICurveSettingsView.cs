using System.Collections.Generic;
using OSPSuite.Presentation.DTO.Charts;
using OSPSuite.Presentation.Presenters.Charts;

namespace OSPSuite.Presentation.Views.Charts
{
   public interface ICurveSettingsView : IView<ICurveSettingsPresenter>, IViewWithColumnSettings
   {
      void BindTo(IEnumerable<CurveDTO> curves);
   }
}