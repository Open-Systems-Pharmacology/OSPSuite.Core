using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Events;
using FakeItEasy;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_ApplicationController : ContextSpecification<IApplicationController>
   {
      protected IList<ISingleStartPresenter> _allOpenedPresenters;
      protected IEventPublisher _eventPublisher;
      protected IContainer _container;
      protected ICommandCollector _commandCollector;

      protected override void Context()
      {
         _allOpenedPresenters = new List<ISingleStartPresenter>();
         _eventPublisher = A.Fake<IEventPublisher>();
         _container = A.Fake<IContainer>();
         _commandCollector = A.Fake<ICommandCollector>();
         sut = new ApplicationController(_container, _eventPublisher, _allOpenedPresenters);
      }
   }

   public class When_the_application_controlled_is_asked_to_retrieve_a_presenter : concern_for_ApplicationController
   {
      private IPresenter _presenter;

      protected override void Context()
      {
         base.Context();
         _presenter = A.Fake<IPresenter>();
         A.CallTo(() => _container.Resolve<IPresenter>()).Returns(_presenter);
      }

      [Observation]
      public void should_retrieve_an_implementation_and_intialize_it()
      {
         sut.Start<IPresenter>().ShouldBeEqualTo(_presenter);
      }
   }

   public class When_the_application_controlled_is_asked_to_initalize_a_presenter : concern_for_ApplicationController
   {
      private IPresenter _presenter;
      private IPresenter _result;

      protected override void Context()
      {
         base.Context();
         _presenter = A.Fake<IPresenter>();
         A.CallTo(() => _container.Resolve<IPresenter>()).Returns(_presenter);
      }

      protected override void Because()
      {
         _result = sut.Start<IPresenter>();
      }

      [Observation]
      public void should_retrieve_an_implementation_and_intialize_it()
      {
         _result.ShouldBeEqualTo(_presenter);
      }

      [Observation]
      public void should_initialize_the_presenter()
      {
         A.CallTo(() => _result.Initialize()).MustHaveHappened();
      }
   }

   public class When_the_application_controller_is_asked_to_open_a_single_start_presenter_that_was_already_started : concern_for_ApplicationController
   {
      private ISingleStartPresenter<IBuildingBlock> _presenter;
      private ISingleStartPresenter _result;
      private IBuildingBlock _individualToEdit;

      protected override void Context()
      {
         base.Context();
         _presenter = A.Fake<ISingleStartPresenter<IBuildingBlock>>();
         _individualToEdit = A.Fake<IBuildingBlock>();
         A.CallTo(() => _presenter.Subject).Returns(_individualToEdit);
         _allOpenedPresenters.Add(_presenter);
      }

      protected override void Because()
      {
         _result = sut.Open<ISingleStartPresenter<IBuildingBlock>, IBuildingBlock>(_individualToEdit, _commandCollector);
      }

      [Observation]
      public void should_retrieve_an_implementation_and_intialize_it()
      {
         _result.ShouldBeEqualTo(_presenter);
      }
   }

   public class When_the_application_controller_is_asked_to_open_a_single_start_presenter_that_was_not_already_started : concern_for_ApplicationController
   {
      private ISingleStartPresenter<IBuildingBlock> _presenter;
      private IBuildingBlock _individualToEdit;
      private ISingleStartPresenter _result;

      protected override void Context()
      {
         base.Context();
         _presenter = A.Fake<ISingleStartPresenter<IBuildingBlock>>();
         _individualToEdit = A.Fake<IBuildingBlock>();
         A.CallTo(() => _presenter.Subject).Returns(_individualToEdit);
         A.CallTo(() => _container.Resolve<ISingleStartPresenter<IBuildingBlock>>()).Returns(_presenter);
      }

      protected override void Because()
      {
         _result = sut.Open<ISingleStartPresenter<IBuildingBlock>, IBuildingBlock>(_individualToEdit, _commandCollector);
      }

      [Observation]
      public void should_retrieve_an_implementation_and_intialize_it()
      {
         _result.ShouldBeEqualTo(_presenter);
      }

      [Observation]
      public void should_initialize_the_presenter_with_the_workspace_as_command_collector()
      {
         A.CallTo(() => _presenter.InitializeWith(_commandCollector)).MustHaveHappened();
      }

      [Observation]
      public void should_add_the_newly_created_presenter_in_the_list_of_managed_presenter()
      {
         _allOpenedPresenters.Contains(_presenter).ShouldBeTrue();
      }
   }

   public class When_the_application_controller_is_being_notified_that_a_view_is_closing : concern_for_ApplicationController
   {
      private ISingleStartPresenter<IBuildingBlock> _presenter;
      private IBuildingBlock _individualToEdit;

      protected override void Context()
      {
         base.Context();
         _presenter = A.Fake<ISingleStartPresenter<IBuildingBlock>>();
         _individualToEdit = A.Fake<IBuildingBlock>();
         A.CallTo(() => _container.Resolve<ISingleStartPresenter<IBuildingBlock>>()).Returns(_presenter);
         sut.Open<ISingleStartPresenter<IBuildingBlock>, IBuildingBlock>(_individualToEdit, _commandCollector);
      }

      protected override void Because()
      {
         _presenter.Closing += Raise.WithEmpty();
      }

      [Observation]
      public void should_remove_the_presenter_from_the_list_of_opened_presenters()
      {
         _allOpenedPresenters.Contains(_presenter).ShouldBeFalse();
      }

      [Observation]
      public void should_unregister_the_presenter_from_the_event_publisher()
      {
         A.CallTo(() => _eventPublisher.RemoveListener(_presenter)).MustHaveHappened();
      }
   }

   public class When_the_application_controller_is_asked_to_close_all_the_opened_presenter : concern_for_ApplicationController
   {
      private ISingleStartPresenter<IBuildingBlock> _presenter;

      protected override void Context()
      {
         base.Context();
         _presenter = A.Fake<ISingleStartPresenter<IBuildingBlock>>();
         _allOpenedPresenters.Add(_presenter);
      }

      protected override void Because()
      {
         sut.CloseAll();
      }

      [Observation]
      public void should_close_the_opened_presenter()
      {
         A.CallTo(() => _presenter.Close()).MustHaveHappened();
      }
   }

   public class When_the_application_controller_is_asked_to_close_a_presenter_for_a_given_subject : concern_for_ApplicationController
   {
      private ISingleStartPresenter<IBuildingBlock> _presenter;
      private IBuildingBlock _individualToEdit;

      protected override void Context()
      {
         base.Context();
         _presenter = A.Fake<ISingleStartPresenter<IBuildingBlock>>();
         _individualToEdit = A.Fake<IBuildingBlock>();
         A.CallTo(() => _presenter.Subject).Returns(_individualToEdit);
         _allOpenedPresenters.Add(_presenter);
      }

      protected override void Because()
      {
         sut.Close(_individualToEdit);
      }

      [Observation]
      public void should_close_any_presenter_that_matches_the_subject()
      {
         A.CallTo(() => _presenter.Close()).MustHaveHappened();
      }
   }

   public class When_the_application_controller_is_asked_to_close_a_presenter_for_a_given_subject_that_is_not_being_edited : concern_for_ApplicationController
   {
      private ISingleStartPresenter<IBuildingBlock> _presenter;
      private IBuildingBlock _individual;

      protected override void Context()
      {
         base.Context();
         _presenter = A.Fake<ISingleStartPresenter<IBuildingBlock>>();
         _individual = A.Fake<IBuildingBlock>();
         A.CallTo(() => _presenter.Subject).Returns(A.Fake<IBuildingBlock>().WithId("TOTO"));
         _allOpenedPresenters.Add(_presenter);
      }

      protected override void Because()
      {
         sut.Close(_individual);
      }

      [Observation]
      public void should_not_close_any_other_edit_presenter()
      {
         A.CallTo(() => _presenter.Close()).MustNotHaveHappened();
      }
   }

   public class When_the_application_controller_is_asked_if_a_presenter_is_open_for_an_edited_subject : concern_for_ApplicationController
   {
      private ISingleStartPresenter<IBuildingBlock> _presenter;
      private IBuildingBlock _individual;

      protected override void Context()
      {
         base.Context();
         _presenter = A.Fake<ISingleStartPresenter<IBuildingBlock>>();
         _individual = A.Fake<IBuildingBlock>();
         A.CallTo(() => _presenter.Subject).Returns(_individual);
         _allOpenedPresenters.Add(_presenter);
      }

      [Observation]
      public void should_return_that_a_presenter_is_open_for_the_subject()
      {
         sut.HasPresenterOpenedFor(_individual).ShouldBeTrue();
      }
   }

   public class When_the_application_controller_is_asked_if_a_presenter_is_open_for_a_subject_not_being_edited : concern_for_ApplicationController
   {
      private IBuildingBlock _individual;

      protected override void Context()
      {
         base.Context();
         _individual = A.Fake<IBuildingBlock>();
      }

      [Observation]
      public void should_return_that_no_presenter_is_open_for_the_subject()
      {
         sut.HasPresenterOpenedFor(_individual).ShouldBeFalse();
      }
   }
}