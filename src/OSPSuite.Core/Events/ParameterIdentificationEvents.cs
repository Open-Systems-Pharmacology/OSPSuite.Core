using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Core.Events
{
   public abstract class ParameterIdentificationEvent : AnalysableEvent
   {
      public ParameterIdentification ParameterIdentification { get; }

      protected ParameterIdentificationEvent(ParameterIdentification parameterIdentification):base(parameterIdentification)
      {
         ParameterIdentification = parameterIdentification;
      }
   }

   public class ParameterIdentificationCreatedEvent : ParameterIdentificationEvent
   {
      public ParameterIdentificationCreatedEvent(ParameterIdentification parameterIdentification) : base(parameterIdentification)
      {
      }
   }

   public class ParameterIdentificationDeletedEvent : ParameterIdentificationEvent
   {
      public ParameterIdentificationDeletedEvent(ParameterIdentification parameterIdentification) : base(parameterIdentification)
      {
      }
   }

   public class ParameterIdentificationStartedEvent : ParameterIdentificationEvent
   {
      public ParameterIdentificationStartedEvent(ParameterIdentification parameterIdentification) : base(parameterIdentification)
      {
      }
   }

   public class ParameterIdentificationTerminatedEvent : ParameterIdentificationEvent
   {
      public ParameterIdentificationTerminatedEvent(ParameterIdentification parameterIdentification) : base(parameterIdentification)
      {
      }
   }

   public class ParameterIdentificationResultsUpdatedEvent : ParameterIdentificationEvent
   {
      public ParameterIdentificationResultsUpdatedEvent(ParameterIdentification parameterIdentification) : base(parameterIdentification)
      {
      }
   }

   public class ParameterIdentificationIntermediateResultsUpdatedEvent : ParameterIdentificationEvent
   {
      public ParameterIdentificationRunState State { get;  }

      public ParameterIdentificationIntermediateResultsUpdatedEvent(ParameterIdentification parameterIdentification, ParameterIdentificationRunState state) : base(parameterIdentification)
      {
         State = state;
      }
   }
}