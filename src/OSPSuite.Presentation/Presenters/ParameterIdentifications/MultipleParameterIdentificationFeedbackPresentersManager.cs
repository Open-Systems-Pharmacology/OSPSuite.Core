using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Events;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Events;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public interface IMultipleParameterIdentificationFeedbackPresentersManager : 
      IListener<ParameterIdentificationStartedEvent>,
      IListener<ParameterIdentificationTerminatedEvent>,
      IListener<ProjectClosedEvent>
   {
      IParameterIdentificationFeedbackPresenter FeedbackPresenterFor(ParameterIdentification parameterIdentification);
   }

   public class MultipleParameterIdentificationFeedbackPresentersManager : IMultipleParameterIdentificationFeedbackPresentersManager
   {
      static private IContainer _container;
      private Cache<ParameterIdentification, IParameterIdentificationFeedbackPresenter> _currentParameterIdentifications = new Cache<ParameterIdentification, IParameterIdentificationFeedbackPresenter>();

      public MultipleParameterIdentificationFeedbackPresentersManager(IContainer container)
      {
         _container = container;
      }

      public IParameterIdentificationFeedbackPresenter FeedbackPresenterFor(ParameterIdentification parameterIdentification)
      {
         if (_currentParameterIdentifications.Contains(parameterIdentification))
            return _currentParameterIdentifications[parameterIdentification];

         var presenter = _container.Resolve<IParameterIdentificationFeedbackPresenter>();
         _currentParameterIdentifications.Add(parameterIdentification, presenter);
         return presenter;
      }

      public void Handle(ParameterIdentificationStartedEvent eventToHandle)
      {
         FeedbackPresenterFor(eventToHandle.ParameterIdentification).OnParameterIdentificationStarted(eventToHandle.ParameterIdentification);
      }

      public void Handle(ParameterIdentificationTerminatedEvent eventToHandle)
      {
         FeedbackPresenterFor(eventToHandle.ParameterIdentification).OnParameterIdentificationTerminated(eventToHandle.ParameterIdentification);
      }

      public void Handle(ProjectClosedEvent eventToHandle)
      {
         _currentParameterIdentifications.Clear();
      }
   }
}
