using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Events;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Events;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public class ParameterIdentificationFeedback
   {
      public ParameterIdentificationFeedback(ParameterIdentification parameterIdentification)
      {
         ParameterIdentification = parameterIdentification;
      }

      public ParameterIdentification ParameterIdentification { get; private set; }

      public bool Running { get; set; }
   }

   public interface IParameterIdentificationFeedbackManager : IListener<ProjectClosedEvent>, IListener<ParameterIdentificationStartedEvent>
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
         var parameterIdentification = eventToHandle.ParameterIdentification;
         lock (_locker)
         {
            var feedback = _cache[parameterIdentification];
            if (feedback == null)
            {
               feedback = new ParameterIdentificationFeedback(parameterIdentification);
               _cache.Add(parameterIdentification, feedback);
            }
            feedback.Running = true;
         }
      }
   }
}
