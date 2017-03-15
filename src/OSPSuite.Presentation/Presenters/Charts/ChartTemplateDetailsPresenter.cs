using System.Collections.Generic;
using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Views.Charts;

namespace OSPSuite.Presentation.Presenters.Charts
{
   public interface IChartTemplateDetailsPresenter : IPresenter<IChartTemplateDetailsView>
   {
      void Edit(CurveChartTemplate chartTemplate);
   }

   public class ChartTemplateDetailsPresenter : AbstractSubPresenter<IChartTemplateDetailsView, IChartTemplateDetailsPresenter>, IChartTemplateDetailsPresenter
   {
      private readonly IChartSettingsPresenter _chartSettingsPresenter;
      private readonly ICurveTemplatePresenter _curveTemplatePresenter;
      private readonly IAxisSettingsPresenter _axisSettingsPresenter;
      private readonly IChartExportSettingsPresenter _chartExportSettingsPresenter;

      public ChartTemplateDetailsPresenter(IChartTemplateDetailsView view, IChartSettingsPresenter chartSettingsPresenter, ICurveTemplatePresenter curveTemplatePresenter, IAxisSettingsPresenter axisSettingsPresenter, IChartExportSettingsPresenter chartExportSettingsPresenter)
         : base(view)
      {
         _chartSettingsPresenter = chartSettingsPresenter;
         _curveTemplatePresenter = curveTemplatePresenter;
         _axisSettingsPresenter = axisSettingsPresenter;
         _chartExportSettingsPresenter = chartExportSettingsPresenter;
         _view.SetChartSettingsView(_chartSettingsPresenter.View);
         _view.SetCurveTemplateView(_curveTemplatePresenter.View);
         _view.SetAxisSettingsView(_axisSettingsPresenter.View);
         _view.SetChartExportSettingsView(_chartExportSettingsPresenter.View);
         AddSubPresenters(_chartSettingsPresenter, _curveTemplatePresenter, _axisSettingsPresenter, _chartExportSettingsPresenter);
      }

      public void Edit(CurveChartTemplate chartTemplate)
      {
         if (chartTemplate == null)
            deleteBinding();
         else
            bindTo(chartTemplate);
      }

      private void bindTo(CurveChartTemplate chartTemplate)
      {
         _curveTemplatePresenter.Edit(chartTemplate.Curves);
         _chartSettingsPresenter.BindTo(chartTemplate);
         _axisSettingsPresenter.SetDataSource(chartTemplate.Axes);
         _chartExportSettingsPresenter.BindTo(chartTemplate);
      }

      private void deleteBinding()
      {
         _curveTemplatePresenter.Edit(new List<CurveTemplate>());
         _chartSettingsPresenter.DeleteBinding();
         _axisSettingsPresenter.SetDataSource(null);
         _chartExportSettingsPresenter.DeleteBinding();
      }
   }
}
