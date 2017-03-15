using OSPSuite.Core.Domain.SensitivityAnalyses;

namespace OSPSuite.Core.Events
{
   public abstract class SensitivityAnalysisEvent
   {
      public SensitivityAnalysis SensitivityAnalysis { get; }

      protected SensitivityAnalysisEvent(SensitivityAnalysis sensitivityAnalysis)
      {
         SensitivityAnalysis = sensitivityAnalysis;
      }
   }

   public class SensitivityAnalysisCreatedEvent : SensitivityAnalysisEvent
   {
      public SensitivityAnalysisCreatedEvent(SensitivityAnalysis sensitivityAnalysis) : base(sensitivityAnalysis)
      {
      }
   }

   public class SensitivityAnalysisDeletedEvent : SensitivityAnalysisEvent
   {
      public SensitivityAnalysisDeletedEvent(SensitivityAnalysis sensitivityAnalysis) : base(sensitivityAnalysis)
      {
      }
   }

   public class SensitivityAnalysisStartedEvent : SensitivityAnalysisEvent
   {
      public SensitivityAnalysisStartedEvent(SensitivityAnalysis sensitivityAnalysis) : base(sensitivityAnalysis)
      {
      }
   }

   public class SensitivityAnalysisResultsUpdatedEvent : SensitivityAnalysisEvent
   {
      public SensitivityAnalysisResultsUpdatedEvent(SensitivityAnalysis sensitivityAnalysis) : base(sensitivityAnalysis)
      {
      }
   }

   public class SensitivityAnalysisProgressEvent : SensitivityAnalysisEvent
   {
      public int NumberOfCalculatedSimulation { get; }
      public int NumberOfSimulations { get; }

      public SensitivityAnalysisProgressEvent(SensitivityAnalysis sensitivityAnalysis, int numberOfCalculatedSimulation, int numberOfSimulations) : base(sensitivityAnalysis)
      {
         NumberOfCalculatedSimulation = numberOfCalculatedSimulation;
         NumberOfSimulations = numberOfSimulations;
      }
   }

   public class SensitivityAnalysisTerminatedEvent : SensitivityAnalysisEvent
   {
      public SensitivityAnalysisTerminatedEvent(SensitivityAnalysis sensitivityAnalysis) : base(sensitivityAnalysis)
      {
      }
   }
}