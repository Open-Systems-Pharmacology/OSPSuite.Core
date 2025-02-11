using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;

namespace OSPSuite.Core.Services
{
   public abstract class concern_for_ConfirmationManager : ContextSpecification<IConfirmationManager>
   {
      protected override void Context()
      {
         sut = new ConfirmationManager();
      }
   }

   internal class When_adding_a_suppression_flag : concern_for_ConfirmationManager
   {
      protected override void Because()
      {
         sut.SuppressConfirmation(ConfirmationFlags.ObservedDataEntryRemoved);
      }

      [Observation]
      public void should_confirm_this_suppression()
      {
         sut.IsConfirmationSuppressed(ConfirmationFlags.ObservedDataEntryRemoved).ShouldBeTrue();
      }
   }
   internal class When_there_is_no_suppression_flag : concern_for_ConfirmationManager
   {
      [Observation]
      public void should_deny_suppressions()
      {
         sut.IsConfirmationSuppressed(ConfirmationFlags.ObservedDataEntryRemoved).ShouldBeFalse();
      }
   }

    internal class When_adding_a_flag_that_already_exists : concern_for_ConfirmationManager
   {
      protected override void Because()
      {
         sut.SuppressConfirmation(ConfirmationFlags.ObservedDataEntryRemoved);
         sut.SuppressConfirmation(ConfirmationFlags.ObservedDataEntryRemoved);
        }

      [Observation]
      public void should_still_confirm_this_suppression()
      {
         sut.IsConfirmationSuppressed(ConfirmationFlags.ObservedDataEntryRemoved);
      }
   }
}
