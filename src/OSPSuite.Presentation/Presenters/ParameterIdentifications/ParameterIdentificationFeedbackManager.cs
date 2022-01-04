using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public class ParameterIdentificationFeedback
   {
      public ParameterIdentificationFeedback(ParameterIdentification parameterIdentification)
      {
         ParameterIdentification = parameterIdentification;
      }
      public ParameterIdentification ParameterIdentification { get; private set; }
   }

   public interface IParameterIdentificationFeedbackManager
   {
      ParameterIdentificationFeedback GetFeedbackFor(ParameterIdentification parameterIdentification);
   }

   public class ParameterIdentificationFeedbackManager : IParameterIdentificationFeedbackManager
   {
      protected readonly Cache<ParameterIdentification, ParameterIdentificationFeedback> _cache = new Cache<ParameterIdentification, ParameterIdentificationFeedback>();
      protected readonly object _locker = new object();

      public ParameterIdentificationFeedback GetFeedbackFor(ParameterIdentification parameterIdentification)
      {
         lock (_locker)
         {
            if (_cache.Contains(parameterIdentification))
               return _cache[parameterIdentification];

            var feedback = new ParameterIdentificationFeedback(parameterIdentification);
            _cache.Add(parameterIdentification, feedback);
            return feedback;
         }
      }
   }
}
