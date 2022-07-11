using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Services.Charts;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Events;

namespace OSPSuite.Presentation.Presenters
{
   public interface ISimulationPredictedVsObservedChartPresenter : IChartPresenter<SimulationPredictedVsObservedChart>,
      ISimulationAnalysisPresenter
      /*,
      IListener<RenamedEvent>,
      IListener<ObservedDataAddedToAnalysableEvent>,
      IListener<ObservedDataRemovedFromAnalysableEvent>,
      IListener<SimulationResultsUpdatedEvent>*/
   {
   }

   public class SimulationPredictedVsObservedChartPresenter : ChartPresenter<SimulationPredictedVsObservedChart, ISimulationAnalysisChartView, ISimulationPredictedVsObservedChartPresenter>,
      ISimulationPredictedVsObservedChartPresenter
   {
      public SimulationPredictedVsObservedChartPresenter(ISimulationAnalysisChartView view, ChartPresenterContext chartPresenterContext) : base(view, chartPresenterContext)
      {
      }

      protected override ISimulation SimulationFor(DataColumn dataColumn)
      {
         throw new System.NotImplementedException();
      }

      public void InitializeAnalysis(ISimulationAnalysis simulationAnalysis, IAnalysable analysable)
      {
         throw new System.NotImplementedException();
      }

      public void UpdateAnalysisBasedOn(IAnalysable analysable)
      {
         throw new System.NotImplementedException();
      }

      public ISimulationAnalysis Analysis { get; }
   }
}