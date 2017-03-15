using System.Collections.Generic;
using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Views
{
   public interface IChartTemplateManagerView : IView<IChartTemplateManagerPresenter>, IResizableView
   {
      void BindTo(IEnumerable<CurveChartTemplate> chartTemplates);
      void SetChartTemplateView(IView view);
   }
}