using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.DTO;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_MessageStatusFilterDTO : ContextSpecification<MessageStatusFilterDTO>
   {
      protected override void Context()
      {
         sut = new MessageStatusFilterDTO();
      }
   }

   public class When_retrieving_the_status_of_a_filter_for_which_warning_and_info_should_be_displayed : concern_for_MessageStatusFilterDTO
   {
      protected override void Context()
      {
         base.Context();
         sut.Warning = true;
         sut.Info = true;
         sut.Error = false;
         sut.Debug = false;
      }

      [Observation]
      public void should_return_warning_and_info_status()
      {
         ((sut.Status & NotificationType.Warning) != 0).ShouldBeTrue();
         ((sut.Status & NotificationType.Info) != 0).ShouldBeTrue();
      }

      [Observation]
      public void should_not_return_error_status()
      {
         ((sut.Status & NotificationType.Error) != 0).ShouldBeFalse();
      }
   }

   public class When_retrieving_the_status_of_a_filter_for_which_error_should_be_displayed : concern_for_MessageStatusFilterDTO
   {
      protected override void Context()
      {
         base.Context();
         sut.Warning = false;
         sut.Info = false;
         sut.Error = true;
         sut.Debug = false;
      }

      [Observation]
      public void should_return_error_status_only()
      {
         sut.Status.ShouldBeEqualTo(NotificationType.Error);
      }
   }
}