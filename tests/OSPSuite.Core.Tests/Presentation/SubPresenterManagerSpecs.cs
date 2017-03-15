using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Events;
using FakeItEasy;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_SubPresenterManager : ContextSpecification<ISubPresenterItemManager<ISubPresenter>>
   {
      protected IEventPublisher _eventPublisher;
      protected ISubPresenter _subPresenter2;
      protected ISubPresenter _subPresenter1;
      protected ISubPresenter _subPresenter3;
      protected IContainerPresenter _containerPresenter;
      protected IContainer _container;

      protected override void Context()
      {
         _eventPublisher = A.Fake<IEventPublisher>();
         _subPresenter1 = A.Fake<ISubPresenter1>();
         _subPresenter2 = A.Fake<ISubPresenter2>();
         _subPresenter3 = A.Fake<ISubPresenter3>();
         _container= A.Fake<IContainer>();
         A.CallTo(() => _container.Resolve<ISubPresenter>(typeof(ISubPresenter1))).Returns(_subPresenter1);
         A.CallTo(() => _container.Resolve<ISubPresenter>(typeof(ISubPresenter2))).Returns(_subPresenter2);
         A.CallTo(() => _container.Resolve<ISubPresenter>(typeof(ISubPresenter3))).Returns(_subPresenter3);

         _containerPresenter = A.Fake<IContainerPresenter>();
         sut = new SubPresenterItemManager<ISubPresenter>(_container, _eventPublisher);
         sut.InitializeWith(_containerPresenter, SubPresenterItems.All);
      }
   }

   public class When_initializing_the_sub_presenter_manager_with_a_command_register : concern_for_SubPresenterManager
   {
      [Observation]
      public void should_initialize_all_available_sub_presenter()
      {
         A.CallTo(() => _subPresenter1.InitializeWith(_containerPresenter)).MustHaveHappened();
         A.CallTo(() => _subPresenter2.InitializeWith(_containerPresenter)).MustHaveHappened();
      }
   }

   public class When_the_sub_presenter_manager_is_asked_if_it_can_close_when_all_presenter_can_be_closed : concern_for_SubPresenterManager
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _subPresenter1.CanClose).Returns(true);
         A.CallTo(() => _subPresenter2.CanClose).Returns(true);
         A.CallTo(() => _subPresenter3.CanClose).Returns(true);
      }

      [Observation]
      public void should_return_true()
      {
         sut.CanClose.ShouldBeTrue();
      }
   }

   public class When_the_sub_presenter_manager_is_asked_if_it_can_close_when_at_least_one_presenter_cannot_be_closed : concern_for_SubPresenterManager
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _subPresenter1.CanClose).Returns(true);
         A.CallTo(() => _subPresenter2.CanClose).Returns(false);
      }

      [Observation]
      public void should_return_true()
      {
         sut.CanClose.ShouldBeFalse();
      }
   }

   public class When_the_sub_presenter_manager_is_releasing_the_sub_presenter : concern_for_SubPresenterManager
   {
      protected override void Because()
      {
         sut.ReleaseFrom(_eventPublisher);
      }

      [Observation]
      public void should_remove_all_presenter_that_are_also_listener_from_the_event_publisher()
      {
         A.CallTo(() => _subPresenter1.ReleaseFrom(_eventPublisher)).MustHaveHappened();
      }
   }

   public class When_the_sub_presenter_manager_is_releasing_the_sub_presenter_implicitely : concern_for_SubPresenterManager
   {
      protected override void Because()
      {
         sut.Release();
      }

      [Observation]
      public void should_remove_all_presenter_that_are_also_listener_from_the_event_publisher()
      {
         A.CallTo(() => _subPresenter1.ReleaseFrom(_eventPublisher)).MustHaveHappened();
      }
   }

   public class When_returning_all_availalbe_sub_presenters : concern_for_SubPresenterManager
   {
      [Observation]
      public void should_return_all_sub_presenter_that_are_being_managed()
      {
         sut.AllSubPresenters.ShouldOnlyContain(_subPresenter1, _subPresenter2,_subPresenter3);
      }
   }

   public class When_returning_the_presenter_item_by_their_index : concern_for_SubPresenterManager
   {
      [Observation]
      public void should_return_the_presenter_as_registered_in_the_initialization()
      {
         sut.ItemAtPosition(0).ShouldBeEqualTo(SubPresenterItems.Item1);
         sut.ItemAtPosition(1).ShouldBeEqualTo(SubPresenterItems.Item2);
         sut.ItemAtPosition(2).ShouldBeEqualTo(SubPresenterItems.Item3);
      }
   }

   public interface ISubPresenter1 : ISubPresenter
   {
   }

   public interface ISubPresenter2 : ISubPresenter
   {
   }

   public interface ISubPresenter3 : ISubPresenter
   {
   }

   public static class SubPresenterItems
   {
      public static readonly SubPresenterItem<ISubPresenter1> Item1 = new SubPresenterItem<ISubPresenter1>();
      public static readonly SubPresenterItem<ISubPresenter2> Item2 = new SubPresenterItem<ISubPresenter2>();
      public static readonly SubPresenterItem<ISubPresenter3> Item3 = new SubPresenterItem<ISubPresenter3>();

      public static readonly IReadOnlyList<ISubPresenterItem> All = new List<ISubPresenterItem> { Item1, Item2, Item3 };
   }
}