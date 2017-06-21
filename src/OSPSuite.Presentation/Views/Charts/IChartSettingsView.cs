using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Presenters.Charts;

namespace OSPSuite.Presentation.Views.Charts
{
   public interface IChartSettingsView : IView<IChartSettingsPresenter>
   {
      void BindTo(IChart chart);
      void Refresh();
      void DeleteBinding();
      bool NameVisible { get; set; }
      void BindTo(CurveChartTemplate chartTemplate);
   }
}