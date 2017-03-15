using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Events
{
   public class SimulationReplacedInParameterAnalyzableEvent
   {
      public SimulationReplacedInParameterAnalyzableEvent(IParameterAnalysable parameterAnalysable, ISimulation oldSimulation, ISimulation newSimulation)
      {
         OldSimulation = oldSimulation;
         NewSimulation = newSimulation;
         ParameterAnalysable = parameterAnalysable;
      }

      public IParameterAnalysable ParameterAnalysable { get; private set; }

      public ISimulation NewSimulation { get; private set; }

      public ISimulation OldSimulation { get; private set; }
   }
}