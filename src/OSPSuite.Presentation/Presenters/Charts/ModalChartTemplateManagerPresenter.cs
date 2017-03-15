using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Views.Charts;

namespace OSPSuite.Presentation.Presenters.Charts
{
   public interface IModalChartTemplateManagerPresenter : IPresenter<IModalChartTemplateManagerView>, IDisposablePresenter
   {
      void EditTemplates(IEnumerable<CurveChartTemplate> chartTemplates);
      bool HasChanged { get; }
      IEnumerable<CurveChartTemplate> EditedTemplates { get; }
      bool Canceled();
      void Display();
   }

   public class ModalChartTemplateManagerPresenter : AbstractDisposableContainerPresenter<IModalChartTemplateManagerView, IModalChartTemplateManagerPresenter>, IModalChartTemplateManagerPresenter
   {
      private readonly IChartTemplateManagerPresenter _chartTemplateManagerPresenter;
      private readonly ICloneManager _cloneManager;

      public ModalChartTemplateManagerPresenter(IModalChartTemplateManagerView view, IChartTemplateManagerPresenter chartTemplateManagerPresenter, ICloneManager cloneManager)
         : base(view)
      {
         _chartTemplateManagerPresenter = chartTemplateManagerPresenter;
         _cloneManager = cloneManager;
         AddSubPresenters(_chartTemplateManagerPresenter);
         view.SetBaseView(_chartTemplateManagerPresenter.BaseView);
      }

      public void EditTemplates(IEnumerable<CurveChartTemplate> chartTemplates)
      {
         var chartTemplatesToEdit = chartTemplates.Select(x =>
         {
            var curveChartTemplate = _cloneManager.Clone(x);
            // setting IsDefault is not part of general cloning and is done when cloning the set only
            curveChartTemplate.IsDefault = x.IsDefault;
            return curveChartTemplate;
         });
         _chartTemplateManagerPresenter.EditTemplates(chartTemplatesToEdit);
      }

      public bool HasChanged
      {
         get { return _chartTemplateManagerPresenter.HasChanged; }
      }

      public IEnumerable<CurveChartTemplate> EditedTemplates
      {
         get { return _chartTemplateManagerPresenter.EditedTemplates; }
      }

      public bool Canceled()
      {
         return _view.Canceled;
      }

      public void Display()
      {
         _view.Display();
      }
   }
}
