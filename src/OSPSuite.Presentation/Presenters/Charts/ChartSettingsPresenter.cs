using System;
using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Views.Charts;

namespace OSPSuite.Presentation.Presenters.Charts
{
   public interface IChartSettingsPresenter : IPresenter<IChartSettingsView>
   {
      void Edit(IChart chart);

      /// <summary>
      ///    specifies whether the <c>Name</c> property of the settings can be edited or not. If not, the name field will be
      ///    hidden.
      ///    Default is <c>true</c>
      /// </summary>
      bool NameVisible { get; set; }

      void Edit(CurveChartTemplate chartTemplate);

      void Clear();

      event EventHandler ChartSettingsChanged;

      void NotifyChartSettingsChanged();
   }

   internal class ChartSettingsPresenter : AbstractPresenter<IChartSettingsView, IChartSettingsPresenter>, IChartSettingsPresenter
   {
      public event EventHandler ChartSettingsChanged = delegate { };

      public void NotifyChartSettingsChanged() => ChartSettingsChanged(this, EventArgs.Empty);

      public ChartSettingsPresenter(IChartSettingsView view) : base(view)
      {
      }

      public bool NameVisible
      {
         get => _view.NameVisible;
         set => _view.NameVisible = value;
      }

      public void Edit(CurveChartTemplate chartTemplate)
      {
         _view.BindTo(chartTemplate);
      }

      public void Clear()
      {
         _view.DeleteBinding();
      }

      public void Edit(IChart chart)
      {
         _view.BindTo(chart);
      }
   }
}