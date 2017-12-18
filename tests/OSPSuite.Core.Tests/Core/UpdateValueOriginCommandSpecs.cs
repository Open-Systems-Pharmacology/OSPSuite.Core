using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Commands;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using Command = OSPSuite.Assets.Command;

namespace OSPSuite.Core
{
   public abstract class concern_for_UpdateValueOriginCommand : ContextSpecification<UpdateValueOriginCommand>
   {
      protected string _newValueDescription = "New value description";
      protected readonly ValueOriginType _newValueOriginType = ValueOriginTypes.Assumption;
      protected ValueOrigin _valueOrigin;
      protected ValueOrigin _oldValueOrigin;
      protected IOSPSuiteExecutionContext _context;
      protected string _objectType = "Paramter";

      protected override void Context()
      {
         _context = A.Fake<IOSPSuiteExecutionContext>();

         _valueOrigin = new ValueOrigin
         {
            Description = "Old Description",
            Type = ValueOriginTypes.ManualFit
         };

         _oldValueOrigin = _valueOrigin.Clone();
         sut = new UpdateValueOriginCommand(_newValueOriginType, _newValueDescription, _valueOrigin, _objectType) {Visible = false};
      }
   }

   public class When_creating_an_update_value_origin_command : concern_for_UpdateValueOriginCommand
   {
      [Observation]
      public void should_have_set_the_comamnd_type_and_object_type_as_expected()
      {
         sut.CommandType.ShouldBeEqualTo(Command.CommandTypeEdit);
         sut.ObjectType.ShouldBeEqualTo(_objectType);
      }
   }

   public class When_retrieving_the_inverse_of_the_update_value_origin_command : concern_for_UpdateValueOriginCommand
   {
      private ICommand _inverseCommand;

      protected override void Because()
      {
         _inverseCommand = sut.InverseCommand(_context);
      }

      [Observation]
      public void should_return_a_command_that_has_the_same_visibility_as_the_command()
      {
         _inverseCommand.Visible.ShouldBeFalse();
      }

      [Observation]
      public void should_have_set_the_comamnd_type_and_object_type_as_expected()
      {
         _inverseCommand.CommandType.ShouldBeEqualTo(Command.CommandTypeEdit);
         _inverseCommand.ObjectType.ShouldBeEqualTo(_objectType);
      }
   }

   public class When_executing_the_update_value_origin_command : concern_for_UpdateValueOriginCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_update_the_value_origin_with_the_new_type_and_description()
      {
         _valueOrigin.Type.ShouldBeEqualTo(_newValueOriginType);
         _valueOrigin.Description.ShouldBeEqualTo(_newValueDescription);
      }
   }

   public class When_executing_the_inverse_of_the_set_value_origin_command : concern_for_UpdateValueOriginCommand
   {
      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void should_reset_the_value_of_type_and_description_to_their_previous_values()
      {
         _valueOrigin.Type.ShouldBeEqualTo(_oldValueOrigin.Type);
         _valueOrigin.Description.ShouldBeEqualTo(_oldValueOrigin.Description);
      }
   }
}