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
      event EventHandler ChartExportSettingsChanged;
      void NotifyChartExportSettingsChanged();
   }

   public class ChartExportSettingsPresenter : AbstractPresenter<IChartExportSettingsView, IChartExportSettingsPresenter>, IChartExportSettingsPresenter
   {
      private readonly IFontsTask _fontsTask;
      private IChartManagement _chartManagement;
      public event EventHandler ChartExportSettingsChanged = delegate { };
      public void NotifyChartExportSettingsChanged() => ChartExportSettingsChanged(this, EventArgs.Empty);

      public ChartExportSettingsPresenter(IChartExportSettingsView view, IFontsTask fontsTask)
         : base(view)
      {
         _fontsTask = fontsTask;
      }

      public void Edit(IChartManagement chartManagement)
      {
         _chartManagement = chartManagement;
         _view.BindTo(chartManagement);
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