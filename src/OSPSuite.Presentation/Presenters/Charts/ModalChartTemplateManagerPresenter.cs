using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Views.Charts;

namespace OSPSuite.Presentation.Presenters.Charts
{
   public interface IModalChartTemplateManagerPresenter : IPresenter<IModalChartTemplateManagerView>, IDisposablePresenter
   {
      void EditTemplates(IEnumerable<CurveChartTemplate> chartTemplates);

      /// <summary>
      ///    Edits the templates created for charts of type <paramref name="chartTypeToManage" /> only. Templates created for
      ///    other chart types are not shown but are preserved and returned as part of <see cref="EditedTemplates" />
      /// </summary>
      void EditTemplates(IEnumerable<CurveChartTemplate> chartTemplates, CurveChartTypes chartTypeToManage);

      bool HasChanged { get; }
      IEnumerable<CurveChartTemplate> EditedTemplates { get; }
      bool Canceled();
      void Display();
   }

   public class ModalChartTemplateManagerPresenter : AbstractDisposableContainerPresenter<IModalChartTemplateManagerView, IModalChartTemplateManagerPresenter>, IModalChartTemplateManagerPresenter
   {
      private readonly IChartTemplateManagerPresenter _chartTemplateManagerPresenter;
      private readonly ICloneManager _cloneManager;
      private readonly List<CurveChartTemplate> _hiddenTemplates = [];

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
         _hiddenTemplates.Clear();
         _chartTemplateManagerPresenter.EditTemplates(cloneOf(chartTemplates));
      }

      public void EditTemplates(IEnumerable<CurveChartTemplate> chartTemplates, CurveChartTypes chartTypeToManage)
      {
         var clonedTemplates = cloneOf(chartTemplates);
         _hiddenTemplates.Clear();
         _hiddenTemplates.AddRange(clonedTemplates.Where(x => x.ChartType != chartTypeToManage));
         _chartTemplateManagerPresenter.EditTemplates(clonedTemplates.Where(x => x.ChartType == chartTypeToManage), _hiddenTemplates.AllNames(), chartTypeToManage);
      }

      private List<CurveChartTemplate> cloneOf(IEnumerable<CurveChartTemplate> chartTemplates)
      {
         return chartTemplates.Select(x =>
         {
            var curveChartTemplate = _cloneManager.Clone(x);
            // setting IsDefault is not part of general cloning and is done when cloning the set only
            curveChartTemplate.IsDefault = x.IsDefault;
            return curveChartTemplate;
         }).ToList();
      }

      public bool HasChanged => _chartTemplateManagerPresenter.HasChanged;

      public IEnumerable<CurveChartTemplate> EditedTemplates => _chartTemplateManagerPresenter.EditedTemplates.Concat(_hiddenTemplates);

      public bool Canceled() => _view.Canceled;

      public void Display() => _view.Display();
   }
}