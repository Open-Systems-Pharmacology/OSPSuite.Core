using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Core.Events;
using OSPSuite.Utility.Events;

namespace OSPSuite.Core
{
   public abstract class concern_for_ParameterIdentificationEngine : ContextSpecification<IParameterIdentificationEngine>
   {
      protected IEventPublisher _eventPublisher;
      protected ParameterIdentification _parameterIdentification;
      protected IParameterIdentificationRunFactory _parameterIdentificationRunFactory;
      protected ICoreUserSettings _coreUserSettings;

      protected override void Context()
      {
         _eventPublisher = A.Fake<IEventPublisher>();
         _parameterIdentificationRunFactory = A.Fake<IParameterIdentificationRunFactory>();
         _coreUserSettings = A.Fake<ICoreUserSettings>();
         A.CallTo(() => _coreUserSettings.MaximumNumberOfCoresToUse).Returns(2);
         _parameterIdentification = new ParameterIdentification().WithId("PI");

         sut = new ParameterIdentificationEngine(_eventPublisher, _parameterIdentificationRunFactory, _coreUserSettings);
      }
   }

   public class When_starting_a_parameter_identification_run_for_a_parameter_identification : concern_for_ParameterIdentificationEngine
   {
      private IParameterIdentificationRun _parameterIdentificationRun;
      private ParameterIdentificationStartedEvent _startedEvent;
      private ParameterIdentificationTerminatedEvent _terminatedEvent;
      private List<IParameterIdentificationRun> _parameterIdentificationsRuns;

      protected override void Context()
      {
         base.Context();
         _parameterIdentificationRun = A.Fake<IParameterIdentificationRun>();
         _parameterIdentificationsRuns = new List<IParameterIdentificationRun> {_parameterIdentificationRun};
         A.CallTo(() => _parameterIdentificationRunFactory.CreateFor(_parameterIdentification, A<CancellationToken>._)).Returns(_parameterIdentificationsRuns);

         A.CallTo(() => _eventPublisher.PublishEvent(A<ParameterIdentificationStartedEvent>._))
            .Invokes(x => _startedEvent = x.GetArgument<ParameterIdentificationStartedEvent>(0));

         A.CallTo(() => _eventPublisher.PublishEvent(A<ParameterIdentificationTerminatedEvent>._))
            .Invokes(x => _terminatedEvent = x.GetArgument<ParameterIdentificationTerminatedEvent>(0));
      }

      protected override void Because()
      {
         sut.StartAsync(_parameterIdentification).Wait();
         //this is required because of some issues with NCrunch and // processing
         Thread.Sleep(1000);
      }

      [Observation]
      public void should_create_one_parameter_identification_run_and_start_it_with_a_clone_of_the_parameter_identification()
      {
         A.CallTo(() => _parameterIdentificationRun.Run(A<CancellationToken>._)).MustHaveHappened();
      }

      [Observation]
      public void should_notify_a_parameter_identification_started_event()
      {
         _startedEvent.ParameterIdentification.ShouldBeEqualTo(_parameterIdentification);
      }

      [Observation]
      public void should_notify_a_parameter_identification_terminated_event()
      {
         _terminatedEvent.ParameterIdentification.ShouldBeEqualTo(_parameterIdentification);
      }
   }

   public class When_the_parameter_identification_engine_presenter_is_notified_that_the_results_of_a_parameter_identification_run_were_updated : concern_for_ParameterIdentificationEngine
   {
      private IParameterIdentificationRun _parameterIdentificationRun;
      private ParameterIdentificationRunState _runState;
      private ParameterIdentificationIntermediateResultsUpdatedEvent _event;
      private List<IParameterIdentificationRun> _parameterIdentificationsRuns;

      protected override void Context()
      {
         base.Context();
         _parameterIdentificationRun = A.Fake<IParameterIdentificationRun>();
         _parameterIdentificationsRuns = new List<IParameterIdentificationRun> {_parameterIdentificationRun};
         A.CallTo(() => _parameterIdentificationRunFactory.CreateFor(_parameterIdentification, A<CancellationToken>._)).Returns(_parameterIdentificationsRuns);
         _runState = A.Fake<ParameterIdentificationRunState>();

         A.CallTo(() => _eventPublisher.PublishEvent(A<ParameterIdentificationIntermediateResultsUpdatedEvent>._))
            .Invokes(x => _event = x.GetArgument<ParameterIdentificationIntermediateResultsUpdatedEvent>(0));

         sut.StartAsync(_parameterIdentification).Wait();
      }

      protected override void Because()
      {
         _parameterIdentificationRun.RunStatusChanged += Raise.With(new ParameterIdentificationRunStatusEventArgs(_runState));
      }

      [Observation]
      public void should_publish_the_intermediated_results_updated_event()
      {
         _event.ParameterIdentification.ShouldBeEqualTo(_parameterIdentification);
         _event.State.ShouldBeEqualTo(_runState);
      }
   }

   public class When_the_parameter_identification_engine_is_notified_that_the_parameter_identification_run_was_canceled : concern_for_ParameterIdentificationEngine
   {
      protected override void Context()
      {
         base.Context();
         var runStatus = new[] {RunStatus.RanToCompletion, RunStatus.Canceled, RunStatus.Faulted, RunStatus.Created};

         var parameterIdentificationRuns = new List<IParameterIdentificationRun>();

         //set to 1 to ensure that we execute the run in the expected order
         A.CallTo(() => _coreUserSettings.MaximumNumberOfCoresToUse).Returns(1);

         A.CallTo(() => _parameterIdentificationRunFactory.CreateFor(_parameterIdentification, A<CancellationToken>._))
            .Returns(parameterIdentificationRuns);

         for (int i = 0; i < runStatus.Length; i++)
         {
            var run = A.Fake<ParameterIdentificationRun>();
            var runResult = A.Fake<ParameterIdentificationRunResult>();
            A.CallTo(() => runResult.Status).Returns(runStatus[i]);
            A.CallTo(() => run.Run(A<CancellationToken>._)).Returns(runResult);

            if (runResult.Status == RunStatus.Faulted)
            {
               A.CallTo(() => run.Run(A<CancellationToken>._)).Invokes(r =>
                  {
                     sut.Stop();
                  })
                  .Returns(runResult);
            }
            parameterIdentificationRuns.Add(run);
         }
      }

      protected override void Because()
      {
         try
         {
            sut.StartAsync(_parameterIdentification).Wait();
         }
         catch (AggregateException)
         {
         }
      }

      [Observation]
      public void should_only_return_parameter_identification_run_results_that_were_canceled_or_ran_to_completion()
      {
         _parameterIdentification.Results.Count.ShouldBeEqualTo(2);
         _parameterIdentification.Results.Select(x => x.Status).ShouldOnlyContain(RunStatus.RanToCompletion, RunStatus.Canceled);
      }
   }
}