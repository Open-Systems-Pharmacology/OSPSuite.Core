using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Events
{
   public abstract class AnalysableEvent
   {
      public IAnalysable Analysable { get; }

      protected AnalysableEvent(IAnalysable analysable)
      {
         Analysable = analysable;
      }
   }

   public class ObservedDataAddedToAnalysableEvent : AnalysableEvent
   {
      public IReadOnlyList<DataRepository> ObservedData { get; }
      public bool ShowData { get; }

      public ObservedDataAddedToAnalysableEvent(IAnalysable analysable, DataRepository observedData, bool showData) : this(analysable, new[] {observedData}, showData)
      {
      }

      public ObservedDataAddedToAnalysableEvent(IAnalysable analysable, IReadOnlyList<DataRepository> observedData, bool showData) : base(analysable)
      {
         ObservedData = observedData;
         ShowData = showData;
      }
   }

   public class ObservedDataRemovedFromAnalysableEvent : AnalysableEvent
   {
      public IReadOnlyList<DataRepository> ObservedData { get; }

      public ObservedDataRemovedFromAnalysableEvent(IAnalysable analysable, DataRepository observedData) : this(analysable, new[] {observedData})
      {
      }

      public ObservedDataRemovedFromAnalysableEvent(IAnalysable analysable, IReadOnlyList<DataRepository> observedData) : base(analysable)
      {
         ObservedData = observedData;
      }
   }

   public class SimulationAnalysisCreatedEvent : AnalysableEvent
   {
      public ISimulationAnalysis SimulationAnalysis { get; }

      public SimulationAnalysisCreatedEvent(IAnalysable analysable, ISimulationAnalysis simulationAnalysis) : base(analysable)
      {
         SimulationAnalysis = simulationAnalysis;
      }
   }

   public abstract class SimulationEvent : AnalysableEvent
   {
      public ISimulation Simulation => Analysable.DowncastTo<ISimulation>();

      protected SimulationEvent(ISimulation simulation) : base(simulation)
      {
      }
   }

   public class SimulationStatusChangedEvent : SimulationEvent
   {
      public SimulationStatusChangedEvent(ISimulation simulation) : base(simulation)
      {
      }
   }

   public class SimulationResultsUpdatedEvent : SimulationEvent
   {
      public SimulationResultsUpdatedEvent(ISimulation simulation) : base(simulation)
      {
      }
   }
}