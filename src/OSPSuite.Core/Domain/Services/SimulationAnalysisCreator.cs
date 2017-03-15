using OSPSuite.Core.Commands;
using OSPSuite.Core.Events;

namespace OSPSuite.Core.Domain.Services
{
   public interface ISimulationAnalysisCreator
   {
      void AddSimulationAnalysisTo(IAnalysable analysable, ISimulationAnalysis simulationAnalysis);
      ISimulationAnalysis CreateAnalysisBasedOn(ISimulationAnalysis simulationAnalysis);
   }

   public abstract class SimulationAnalysisCreator : ISimulationAnalysisCreator
   {
      private readonly IContainerTask _containerTask;
      private readonly IOSPSuiteExecutionContext _executionContext;

      protected SimulationAnalysisCreator(IContainerTask containerTask, IOSPSuiteExecutionContext executionContext)
      {
         _containerTask = containerTask;
         _executionContext = executionContext;
      }

      public void AddSimulationAnalysisTo(IAnalysable analysable, ISimulationAnalysis simulationAnalysis)
      {
         if (simulationAnalysis == null) return;

         var defaultAnalysisName = string.IsNullOrEmpty(simulationAnalysis.Name) ? DefaultAnalysisNameFor(simulationAnalysis) : simulationAnalysis.Name;
         simulationAnalysis.Name = _containerTask.CreateUniqueName(analysable.Analyses, defaultAnalysisName, canUseBaseName: true);
         analysable.AddAnalysis(simulationAnalysis);
         _executionContext.PublishEvent(new SimulationAnalysisCreatedEvent(analysable, simulationAnalysis));
      }

      protected virtual string DefaultAnalysisNameFor(ISimulationAnalysis simulationAnalysis)
      {
         return _executionContext.TypeFor(simulationAnalysis);
      }

      public abstract ISimulationAnalysis CreateAnalysisBasedOn(ISimulationAnalysis simulationAnalysis);
   }
}