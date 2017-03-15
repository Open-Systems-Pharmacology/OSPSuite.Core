using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Views.Charts;

namespace OSPSuite.Presentation.Presenters.Charts
{
   public interface IChartSettingsPresenter : IPresenter<IChartSettingsView>
   {
      void BindTo(IChart chart);

      /// <summary>
      ///    specifies whether the <c>Name</c> property of the settings can be edited or not. If not, the name field will be
      ///    hidden.
      ///    Default is <c>true</c>
      /// </summary>
      bool NameVisible { get; set; }

      void BindTo(CurveChartTemplate chartTemplate);
      void DeleteBinding();
   }

   internal class ChartSettingsPresenter : AbstractPresenter<IChartSettingsView, IChartSettingsPresenter>, IChartSettingsPresenter
   {
      public bool NameVisible
      {
         get { return _view.NameVisible; }
         set { _view.NameVisible = value; }
      }

      public ChartSettingsPresenter(IChartSettingsView view)
         : base(view)
      {
      }

      public void BindTo(CurveChartTemplate chartTemplate)
      {
         _view.BindToSource(chartTemplate);
      }

      public void DeleteBinding()
      {
         _view.DeleteBinding();
      }

      public void BindTo(IChart chart)
      {
         _view.BindToSource(chart);
      }
   }
}