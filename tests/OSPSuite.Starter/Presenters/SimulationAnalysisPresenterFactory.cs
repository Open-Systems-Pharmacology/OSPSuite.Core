using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.Starter.Presenters
{
   public class SimulationAnalysisPresenterFactory:OSPSuite.Presentation.Presenters.SimulationAnalysisPresenterFactory
   {
      public SimulationAnalysisPresenterFactory(IContainer container) : base(container)
      {
      }

      protected override ISimulationAnalysisPresenter PresenterFor(ISimulationAnalysis simulationAnalysis, IContainer container)
      {
         return null;
      }
   }
}