using System;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;

namespace OSPSuite.Core.Commands
{
   public abstract class concern_for_command_id : ContextSpecification<CommandId>
   {
      protected string _guidId;

      protected override void Context()
      {
         _guidId = Guid.NewGuid().ToString();
         sut = new CommandId(_guidId, null);
      }
   }

   public class When_retrieving_the_inverse_of_a_command_id : concern_for_command_id
   {
      private CommandId _result;

      protected override void Because()
      {
         _result = sut.CreateInverseId();
      }

      [Observation]
      public void should_return_a_new_command_id_that_is_indeed_the_inverse()
      {
         _result.IsInverseFor(sut).ShouldBeTrue();
         sut.IsInverseFor(_result).ShouldBeTrue();
      }
   }

   public class When_retrieving_the_inverse_of_a_command_id_that_is_already_an_inverse : concern_for_command_id
   {
      private CommandId _result;

      protected override void Because()
      {
         _result = sut.CreateInverseId().CreateInverseId();
      }

      [Observation]
      public void should_return_a_new_command_id_that_is_the_original_id()
      {
         _result.ShouldBeEqualTo(sut);
      }
   }

   public class When_asking_if_a_random_command_id_is_the_inverse : concern_for_command_id
   {
      private bool _result;
      private CommandId _oneRandomCommandId;

      [Observation]
      public void should_return_false()
      {
         _result.ShouldBeFalse();
      }

      protected override void Context()
      {
         base.Context();
         _oneRandomCommandId = new CommandId();
      }

      protected override void Because()
      {
         _result = sut.IsInverseFor(_oneRandomCommandId);
      }
   }

   public class When_asking_if_one_command_id_is_its_inverse : concern_for_command_id
   {
      private bool _result;

      [Observation]
      public void should_return_false()
      {
         _result.ShouldBeFalse();
      }

      protected override void Because()
      {
         _result = sut.IsInverseFor(sut);
      }
   }

   public class When_retrieving_the_id_of_a_command_id : concern_for_command_id
   {
      private string _result;

      [Observation]
      public void should_return_the_guid()
      {
         _result.ShouldBeEqualTo(_guidId.ToString());
      }

      protected override void Because()
      {
         _result = sut.Id;
      }
   }
}