using OSPSuite.Utility.Events;
using OSPSuite.Core.Domain;

namespace OSPSuite.Presentation.Presenters
{
   public interface ISimulationAnalysisPresenter : IPresenter, IListener, IPresenterWithSettings
   {
      /// <summary>
      ///    Intitialize the presenter for the <paramref name="simulationAnalysis" />.
      /// </summary>
      void InitializeAnalysis(ISimulationAnalysis simulationAnalysis, IAnalysable analysable);

      /// <summary>
      /// Updates the underlying <see cref="ISimulationAnalysis"/> with the data from <paramref name="analysable"/> (e.g in case of simulation chart, the simulaton results)
      /// </summary>
      void UpdateAnalysisBasedOn(IAnalysable analysable);

      ISimulationAnalysis Analysis { get; }

      /// <summary>
      ///    Clear the analysis presenter (References to all underlying objects are removed)
      /// </summary>
      void Clear();
   }

   public interface ISimulationAnalysisPresenter<TAnalyzable> : ISimulationAnalysisPresenter where TAnalyzable : IAnalysable
   {
      void UpdateAnalysisBasedOn(TAnalyzable analyzable);
   }
}