using System.Collections.Generic;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Services.Charts;
using OSPSuite.Presentation.Views.Charts;

namespace OSPSuite.Presentation.Presenters.Charts
{
   public interface IChartExportSettingsPresenter : IPresenter<IChartExportSettingsView>
   {
      void BindTo(IChartManagement chartSettings);
      IEnumerable<string> GetFontFamilyNames();
      IEnumerable<int> GetFontSizes();
      void ResetValuesToDefault();
      void DeleteBinding();
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

      public void BindTo(IChartManagement chartSettings)
      {
         _chartSettings = chartSettings;
         _view.BindToSource(chartSettings);
      }

      public IEnumerable<string> GetFontFamilyNames()
      {
         return _fontsTask.ChartFontFamilyNames;
      }

      public IEnumerable<int> GetFontSizes()
      {
         return Constants.ChartFontOptions.GetFontSizes();
      }

      public void ResetValuesToDefault()
      {
         _chartSettings.FontAndSize.Reset();
      }

      public void DeleteBinding()
      {
         _view.DeleteBinding();
      }
   }
}