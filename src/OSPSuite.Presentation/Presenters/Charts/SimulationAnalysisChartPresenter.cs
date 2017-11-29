using System.Linq;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Services.Charts;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presenters.Charts
{
   public abstract class SimulationAnalysisChartPresenter<TChart, TView, TPresenter> : ChartPresenter<TChart, TView, TPresenter>, ISimulationAnalysisPresenter
      where TPresenter : ISimulationAnalysisPresenter
      where TChart : ChartWithObservedData, ISimulationAnalysis
      where TView : class, IView<TPresenter>
   {
      private bool _isInitializing;
      private bool _chartIsNew;

      protected SimulationAnalysisChartPresenter(TView view, ChartPresenterContext chartPresenterContext)
         : base(view, chartPresenterContext)
      {
      }

      public ISimulationAnalysis Analysis => Chart;

      public virtual void InitializeAnalysis(ISimulationAnalysis simulationAnalysis, IAnalysable analysable)
      {
         try
         {
            _isInitializing = true;
            base.InitializeAnalysis(simulationAnalysis.DowncastTo<TChart>());
            _chartIsNew = !Chart.Curves.Any();
            UpdateAnalysisBasedOn(analysable);
         }
         finally
         {
            _isInitializing = false;
         }
      }

      protected virtual bool ChartIsBeingCreated => _isInitializing && _chartIsNew;
      protected virtual bool ChartIsBeingLoaded => _isInitializing && !_chartIsNew;
      protected virtual bool ChartIsBeingUpdated => !_isInitializing;

      public abstract void UpdateAnalysisBasedOn(IAnalysable analysable);
   }
}