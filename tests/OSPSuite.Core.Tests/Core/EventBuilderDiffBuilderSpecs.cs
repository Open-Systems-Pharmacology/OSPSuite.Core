using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core
{
   public class When_comparing_EventBuilders_with_different_One_time_event_settings : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var e1 = new EventBuilder().WithName("Event");
         e1.OneTime = true;

         var e2 = new EventBuilder().WithName("Event");
         e2.OneTime = false;

         _object1 = e1;
         _object2 = e2;
      }

      [Observation]
      public void should_report_the_differences_accordingly()
      {
         _report.Count().ShouldBeEqualTo(1);
      }
   }

   public class When_comparing_EventBuilders_with_different_conditions : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var e1 = new EventBuilder().WithName("Event");
         e1.OneTime = true;
         e1.Formula = new ExplicitFormula("Time>StartTime");

         var e2 = new EventBuilder().WithName("Event");
         e2.OneTime = true;
         e2.Formula = new ExplicitFormula("Time>StopTime");

         _object1 = e1;
         _object2 = e2;
      }

      [Observation]
      public void should_report_the_differences_accordingly()
      {
         _report.Count().ShouldBeEqualTo(1);
      }
   }

   public class When_comparing_EventBuilders_with_different_assingments : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var e1 = new EventBuilder().WithName("Event");
         e1.OneTime = true;
         var eventAssignmentBuilder1 = new EventAssignmentBuilder();
         eventAssignmentBuilder1.ObjectPath = new ObjectPath("Sim|A|B|C");
         eventAssignmentBuilder1.Name = "EAB";
         e1.AddAssignment(eventAssignmentBuilder1);

         var e2 = new EventBuilder().WithName("Event");
         e2.OneTime = true;
         var eventAssignmentBuilder2 = new EventAssignmentBuilder();
         eventAssignmentBuilder2.ObjectPath = new ObjectPath("Sim|A|B|D");
         eventAssignmentBuilder2.Name = "EAB";
         e2.AddAssignment(eventAssignmentBuilder2);


         _object1 = e1;
         _object2 = e2;
      }

      [Observation]
      public void should_report_the_differences_accordingly()
      {
         _report.Count().ShouldBeEqualTo(2);
      }
   }
}