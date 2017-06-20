using System;
using System.Collections.Generic;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Services.Charts;
using OSPSuite.Presentation.Views.Charts;

namespace OSPSuite.Presentation.Presenters.Charts
{
   public interface IChartExportSettingsPresenter : IPresenter<IChartExportSettingsView>
   {
      void Edit(IChartManagement chartManagement);
      IEnumerable<string> AllFontFamilyNames { get; }
      IEnumerable<int> AllFontSizes { get; }
      void ResetValuesToDefault();
      void Clear();
      event Action ChartExportSettingsChanged;
      void NotifyChartExportSettingsChanged();
   }

   public class ChartExportSettingsPresenter : AbstractPresenter<IChartExportSettingsView, IChartExportSettingsPresenter>, IChartExportSettingsPresenter
   {
      private readonly IFontsTask _fontsTask;
      private IChartManagement _chartManagement;
      public event Action ChartExportSettingsChanged = delegate { };
      public void NotifyChartExportSettingsChanged() => ChartExportSettingsChanged();

      public ChartExportSettingsPresenter(IChartExportSettingsView view, IFontsTask fontsTask)
         : base(view)
      {
         _fontsTask = fontsTask;
      }

      public void Edit(IChartManagement chartManagement)
      {
         _chartManagement = chartManagement;
         _view.BindToSource(chartManagement);
      }

      public IEnumerable<string> AllFontFamilyNames => _fontsTask.ChartFontFamilyNames;

      public IEnumerable<int> AllFontSizes => Constants.ChartFontOptions.AllFontSizes;

      public void ResetValuesToDefault()
      {
         _chartManagement.FontAndSize.Reset();
         NotifyChartExportSettingsChanged();
      }

      public void Clear()
      {
         _view.DeleteBinding();
      }
   }
}