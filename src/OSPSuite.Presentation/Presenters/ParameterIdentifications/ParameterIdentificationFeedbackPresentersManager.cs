using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Events;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Events;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public interface IParameterIdentificationFeedbackPresentersManager : 
      IListener<ParameterIdentificationStartedEvent>,
      IListener<ParameterIdentificationTerminatedEvent>,
      IListener<ProjectClosedEvent>
   {
      IParameterIdentificationFeedbackPresenter FeedbackPresenterFor(ParameterIdentification parameterIdentification);
   }

   public class ParameterIdentificationFeedbackPresentersManager : IParameterIdentificationFeedbackPresentersManager
   {
      private readonly IContainer _container;
      private readonly Cache<ParameterIdentification, IParameterIdentificationFeedbackPresenter> _currentParameterIdentifications = new Cache<ParameterIdentification, IParameterIdentificationFeedbackPresenter>();
      private readonly object _locker = new object();

      public ParameterIdentificationFeedbackPresentersManager(IContainer container)
      {
         _container = container;
      }

      public IParameterIdentificationFeedbackPresenter FeedbackPresenterFor(ParameterIdentification parameterIdentification)
      {
         lock (_locker)
         {
            if (_currentParameterIdentifications.Contains(parameterIdentification))
               return _currentParameterIdentifications[parameterIdentification];

            var presenter = _container.Resolve<IParameterIdentificationFeedbackPresenter>();
            _currentParameterIdentifications.Add(parameterIdentification, presenter);
            return presenter;
         }
      }

      public void Handle(ParameterIdentificationStartedEvent eventToHandle)
      {
         FeedbackPresenterFor(eventToHandle.ParameterIdentification).ParameterIdentificationStarted(eventToHandle.ParameterIdentification);
      }

      public void Handle(ParameterIdentificationTerminatedEvent eventToHandle)
      {
         FeedbackPresenterFor(eventToHandle.ParameterIdentification).ParameterIdentificationTerminated(eventToHandle.ParameterIdentification);
      }

      public void Handle(ProjectClosedEvent eventToHandle)
      {
         lock (_locker)
         {
            _currentParameterIdentifications.Clear();
         }
      }
   }
}
