using System.Collections.Generic;
using OSPSuite.Presentation.DTO.Charts;
using OSPSuite.Presentation.Presenters.Charts;

namespace OSPSuite.Presentation.Views.Charts
{
   public interface ICurveTemplateView : IView<ICurveTemplatePresenter>
   {
      void BindTo(IEnumerable<CurveTemplateDTO> curveTemplates);
   }
}
