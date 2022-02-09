using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Events;
using OSPSuite.Utility;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Events;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public class ParameterIdentificationFeedback : IWithId
   {
      public string Id { get; set; }

      public ParameterIdentificationFeedback(ParameterIdentification parameterIdentification)
      {
         ParameterIdentification = parameterIdentification;
         Id = ShortGuid.NewGuid();
      }

      public ParameterIdentification ParameterIdentification { get; private set; }

      public RunStatusId RunStatus { get; set; }
   }

   public interface IParameterIdentificationFeedbackManager : 
      IListener<ProjectClosedEvent>, 
      IListener<ParameterIdentificationStartedEvent>,
      IListener<ParameterIdentificationTerminatedEvent>
   {
      ParameterIdentificationFeedback GetFeedbackFor(ParameterIdentification parameterIdentification);
   }

   public class ParameterIdentificationFeedbackManager : IParameterIdentificationFeedbackManager
   {
      protected readonly Cache<ParameterIdentification, ParameterIdentificationFeedback> _cache = new Cache<ParameterIdentification, ParameterIdentificationFeedback>(onMissingKey: _ => null);
      protected readonly object _locker = new object();

      public ParameterIdentificationFeedback GetFeedbackFor(ParameterIdentification parameterIdentification)
      {
         lock (_locker)
         {
            var feedback = _cache[parameterIdentification];
            if (feedback == null)
            {
               feedback = new ParameterIdentificationFeedback(parameterIdentification);
               _cache.Add(parameterIdentification, feedback);
            }
            return feedback;
         }
      }

      public void Handle(ProjectClosedEvent eventToHandle)
      {
         lock (_locker)
         {
            _cache.Clear();
         }
      }

      public void Handle(ParameterIdentificationStartedEvent eventToHandle)
      {
         GetFeedbackFor(eventToHandle.ParameterIdentification).RunStatus = RunStatusId.Running;
      }

      public void Handle(ParameterIdentificationTerminatedEvent eventToHandle)
      {
         GetFeedbackFor(eventToHandle.ParameterIdentification).RunStatus = RunStatusId.Canceled;
      }
   }
}
