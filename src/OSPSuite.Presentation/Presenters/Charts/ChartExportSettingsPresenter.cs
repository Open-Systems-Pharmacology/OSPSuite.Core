using System.Collections.Generic;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Services.Charts;
using OSPSuite.Presentation.Views.Charts;

namespace OSPSuite.Presentation.Presenters.Charts
{
   public interface IChartExportSettingsPresenter : IPresenter<IChartExportSettingsView>
   {
      void Edit(IChartManagement chartSettings);
      IEnumerable<string> AllFontFamilyNames { get; }
      IEnumerable<int> AllFontSizes { get; }
      void ResetValuesToDefault();
      void Clear();
   }

   public class ChartExportSettingsPresenter : AbstractPresenter<IChartExportSettingsView, IChartExportSettingsPresenter>, IChartExportSettingsPresenter
   {
      private readonly IFontsTask _fontsTask;
      private IChartManagement _chartSettings;

      public ChartExportSettingsPresenter(IChartExportSettingsView view, IFontsTask fontsTask)
         : base(view)
      {
         _fontsTask = fontsTask;
      }

      public void Edit(IChartManagement chartSettings)
      {
         _chartSettings = chartSettings;
         _view.BindToSource(chartSettings);
      }

      public IEnumerable<string> AllFontFamilyNames => _fontsTask.ChartFontFamilyNames;

      public IEnumerable<int> AllFontSizes => Constants.ChartFontOptions.AllFontSizes;

      public void ResetValuesToDefault()
      {
         _chartSettings.FontAndSize.Reset();
      }

      public void Clear()
      {
         _view.DeleteBinding();
      }
   }
}