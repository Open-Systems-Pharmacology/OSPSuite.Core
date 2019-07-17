using System.Collections.Generic;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Comparison;
using OSPSuite.Core.Domain;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Presentation.UICommands
{
   public abstract class concern_for_CompareObjectsUICommand : ContextSpecification<CompareObjectsUICommand>
   {
      protected IEventPublisher _eventPublisher;

      protected override void Context()
      {
         _eventPublisher = A.Fake<IEventPublisher>();
         sut = new CompareObjectsUICommand(_eventPublisher);
      }
   }

   public class When_executing_the_compare_objects_command_for_more_than_two_objects : concern_for_CompareObjectsUICommand
   {
      protected override void Context()
      {
         base.Context();
         sut.Subject = new List<IObjectBase> {A.Fake<IObjectBase>(), A.Fake<IObjectBase>(), A.Fake<IObjectBase>()};
      }

      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.Execute()).ShouldThrowAn<OSPSuiteException>();
      }
   }

   public class When_executing_the_compare_objects_command_for_two_objects : concern_for_CompareObjectsUICommand
   {
      private IObjectBase _leftObject;
      private IObjectBase _rightObject;
      private StartComparisonEvent _event;

      protected override void Context()
      {
         base.Context();
         _leftObject = A.Fake<IObjectBase>().WithName("LEFT");
         _rightObject = A.Fake<IObjectBase>().WithName("RIGHT");
         sut.Subject = new List<IObjectBase> {_leftObject, _rightObject};
         A.CallTo(() => _eventPublisher.PublishEvent(A<StartComparisonEvent>._))
            .Invokes(x => _event = x.GetArgument<StartComparisonEvent>(0));
      }

      protected override void Because()
      {
         sut.Execute();
      }

      [Observation]
      public void should_start_the_comparison_for_the_given_objects_by_throwing_the_start_comparison_event()
      {
         _event.LeftObject.ShouldBeEqualTo(_leftObject);
         _event.RightObject.ShouldBeEqualTo(_rightObject);
      }

      [Observation]
      public void should_set_labels_to_object_Names()
      {
         _event.LeftCaption.ShouldBeEqualTo(_leftObject.Name);
         _event.RightCaption.ShouldBeEqualTo(_rightObject.Name);
      }
   }

   public class When_executing_the_compare_objects_command_for_two_objects_and_the_names_are_specificed : concern_for_CompareObjectsUICommand
   {
      private IObjectBase _leftObject;
      private IObjectBase _rightObject;
      private StartComparisonEvent _event;

      protected override void Context()
      {
         base.Context();
         _leftObject = A.Fake<IObjectBase>().WithName("LEFT");
         _rightObject = A.Fake<IObjectBase>().WithName("RIGHT");
         sut.Subject = new List<IObjectBase> { _leftObject, _rightObject };
         A.CallTo(() => _eventPublisher.PublishEvent(A<StartComparisonEvent>._))
            .Invokes(x => _event = x.GetArgument<StartComparisonEvent>(0));

         sut.ObjectNames = new List<string> { "A", "B" };
      }

      protected override void Because()
      {
         sut.Execute();
      }

      [Observation]
      public void should_start_the_comparison_for_the_given_objects_by_throwing_the_start_comparison_event()
      {
         _event.LeftObject.ShouldBeEqualTo(_leftObject);
         _event.RightObject.ShouldBeEqualTo(_rightObject);
      }

      [Observation]
      public void should_set_labels_to_specified_names()
      {
         _event.LeftCaption.ShouldBeEqualTo("A");
         _event.RightCaption.ShouldBeEqualTo("B");
      }
   }
}