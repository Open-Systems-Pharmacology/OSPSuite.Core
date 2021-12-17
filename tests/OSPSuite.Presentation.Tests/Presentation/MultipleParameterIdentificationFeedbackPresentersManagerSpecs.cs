using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Utility.Container;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_MultipleParameterIdentificationFeedbackPresentersManager : ContextSpecification<MultipleParameterIdentificationFeedbackPresentersManager>
   {
      protected IContainer _container;
      protected ParameterIdentification _parameterIdentification;

      protected override void Context()
      {
         _container = A.Fake<IContainer>();
         _parameterIdentification = new ParameterIdentification();
         A.CallTo(() => _container.Resolve<IParameterIdentificationFeedbackPresenter>()).ReturnsLazily(() => A.Fake<IParameterIdentificationFeedbackPresenter>());
         sut = new MultipleParameterIdentificationFeedbackPresentersManager(_container);
      }
   }

   public class When_starting_a_new_parameter_identification : concern_for_MultipleParameterIdentificationFeedbackPresentersManager
   {
      protected override void Because()
      {
         sut.Handle(new ParameterIdentificationStartedEvent(_parameterIdentification));
      }

      [Observation]
      public void should_send_started_event_to_proper_presenter()
      {
         A.CallTo(() => sut.FeedbackPresenterFor(_parameterIdentification).OnParameterIdentificationStarted(_parameterIdentification)).MustHaveHappened();
      }
   }

   public class When_terminating_a_new_parameter_identification : concern_for_MultipleParameterIdentificationFeedbackPresentersManager
   {
      protected override void Because()
      {
         sut.Handle(new ParameterIdentificationTerminatedEvent(_parameterIdentification));
      }

      [Observation]
      public void should_send_terminated_event_to_proper_presenter()
      {
         A.CallTo(() => sut.FeedbackPresenterFor(_parameterIdentification).OnParameterIdentificationTerminated(_parameterIdentification)).MustHaveHappened();
      }
   }

   public class When_accessing_a_presenter_through_parameter_identification : concern_for_MultipleParameterIdentificationFeedbackPresentersManager
   {
      [Observation]
      public void should_return_the_same_for_a_single_parameter_identification()
      {
         sut.FeedbackPresenterFor(_parameterIdentification).ShouldBeEqualTo(sut.FeedbackPresenterFor(_parameterIdentification));
      }

      [Observation]
      public void should_return_a_new_presenter_for_each_new_parameter_identification()
      {
         sut.FeedbackPresenterFor(_parameterIdentification).ShouldNotBeEqualTo(sut.FeedbackPresenterFor(new ParameterIdentification()));
      }
   }
}
