using System.Collections.Generic;
using OSPSuite.Core.Domain.Data;

namespace OSPSuite.Core.Domain
{
   public interface IUsesObservedData : IWithName
   {
      bool UsesObservedData(DataRepository observedData);
   }

   public interface IAnalysable : IWithName
   {
      void RemoveAnalysis(ISimulationAnalysis simulationAnalysis);
      IEnumerable<ISimulationAnalysis> Analyses { get; }
      void AddAnalysis(ISimulationAnalysis simulationAnalysis);
      bool HasUpToDateResults { get; }
      bool ComesFromPKSim { get; }
   }

   public interface IParameterAnalysable : IAnalysable, ILazyLoadable, IObjectBase
   {
      IReadOnlyList<ISimulation> AllSimulations { get; }
      bool UsesSimulation(ISimulation oldSimulation);
      void SwapSimulations(ISimulation oldSimulation, ISimulation newSimulation);
   }
}