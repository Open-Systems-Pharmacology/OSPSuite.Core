using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core
{
   public class When_comparing_Event_Groups_with_differernt_EventGroupType:concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var eg1 = new  EventGroup().WithName("Application");
         var eg2 = new EventGroup().WithName("Application");

         eg1.EventGroupType = "IV";
         eg2.EventGroupType = "Oral";

         _object1 = eg1;
         _object2 = eg2;
      }

      [Observation]
      public void should_report_the_difference()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }

}