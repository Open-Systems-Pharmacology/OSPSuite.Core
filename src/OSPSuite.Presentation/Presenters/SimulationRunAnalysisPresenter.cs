using System;
using System.Collections.Generic;
using System.Text;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Services.Charts;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation.Presenters
{
   public interface ISimulationRunAnalysisPresenter : ISimulationAnalysisPresenter
   {
   }

   public abstract class SimulationRunAnalysisPresenter<TChart> : SimulationAnalysisChartPresenter<TChart, ISimulationRunAnalysisView, ISimulationRunAnalysisPresenter>, ISimulationRunAnalysisPresenter where TChart : ChartWithObservedData, ISimulationAnalysis
   {
      protected SimulationRunAnalysisPresenter(ISimulationRunAnalysisView view, ChartPresenterContext chartPresenterContext) : base(view, chartPresenterContext)
      {
      }
   }
}
