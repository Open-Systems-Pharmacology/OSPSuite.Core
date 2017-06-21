using OSPSuite.Core.Domain;

namespace OSPSuite.Presentation.Presenters
{
   public interface ISimulationAnalysisPresenterFactory
   {
      ISimulationAnalysisPresenter PresenterFor(ISimulationAnalysis simulationAnalysis);
   }
}  