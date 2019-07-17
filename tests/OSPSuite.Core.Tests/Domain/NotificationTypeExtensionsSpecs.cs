using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;

namespace OSPSuite.Core.Domain
{
   public class When_testing_if_a_message_type_is_of_a_given_type : StaticContextSpecification
   {
      [Observation]
      public void should_return_true_for_all_basic_type()
      {
         NotificationType.Error.Is(NotificationType.Error).ShouldBeTrue();
         NotificationType.Warning.Is(NotificationType.Warning).ShouldBeTrue();
         NotificationType.Info.Is(NotificationType.Info).ShouldBeTrue();
      }

      [Observation]
      public void should_return_the_expected_result_for_composite_name()
      {
         (NotificationType.Error | NotificationType.Info).Is(NotificationType.Error).ShouldBeTrue();
         (NotificationType.Error | NotificationType.Info).Is(NotificationType.Info).ShouldBeTrue();
         (NotificationType.Error | NotificationType.Warning).Is(NotificationType.Info).ShouldBeFalse();
         (NotificationType.Error).Is(NotificationType.Error | NotificationType.Info).ShouldBeTrue();
         (NotificationType.Info).Is(NotificationType.Info | NotificationType.Error).ShouldBeTrue();
         (NotificationType.Error).Is(NotificationType.Info | NotificationType.Warning).ShouldBeFalse();
         (NotificationType.All).Is(NotificationType.Info).ShouldBeTrue();
         (NotificationType.All).Is(NotificationType.Error).ShouldBeTrue();
         (NotificationType.All).Is(NotificationType.Warning).ShouldBeTrue();
         (NotificationType.All).Is(NotificationType.Debug).ShouldBeTrue();
      }
   }
}