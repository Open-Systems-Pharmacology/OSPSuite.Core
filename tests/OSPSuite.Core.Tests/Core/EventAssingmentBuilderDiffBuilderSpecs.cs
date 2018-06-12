using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core
{
   public class When_comparing_EventAssingmentBuilders_with_different_names : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var e1 = new EventBuilder().WithName("Event");
         e1.OneTime = true;
         var eventAssignmentBuilder1 = new EventAssignmentBuilder();
         eventAssignmentBuilder1.ObjectPath = new ObjectPath("Sim|A|B|C");
         eventAssignmentBuilder1.Name = "EAB1";
         e1.AddAssignment(eventAssignmentBuilder1);

         var e2 = new EventBuilder().WithName("Event");
         e2.OneTime = true;
         var eventAssignmentBuilder2 = new EventAssignmentBuilder();
         eventAssignmentBuilder2.ObjectPath = new ObjectPath("Sim|A|B|C");
         eventAssignmentBuilder2.Name = "EAB2";
         e2.AddAssignment(eventAssignmentBuilder2);


         _object1 = e1;
         _object2 = e2;
         _comparerSettings.OnlyComputingRelevant = false;
      }


      [Observation]
      public void should_report_the_differences_accordingly()
      {
         _report.Count().ShouldBeEqualTo(1);
      }
   }

   public class When_comparing_EventAssingmentBuilderss_with_different_properties : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var e1 = new EventBuilder().WithName("Event");
         e1.OneTime = true;
         var eventAssignmentBuilder1 = new EventAssignmentBuilder();
         eventAssignmentBuilder1.ObjectPath = new ObjectPath("Sim|A|B|C");
         eventAssignmentBuilder1.Name = "EAB";
         eventAssignmentBuilder1.UseAsValue = true;
         eventAssignmentBuilder1.Formula = new ExplicitFormula("a+b");
         e1.AddAssignment(eventAssignmentBuilder1);

         var e2 = new EventBuilder().WithName("Event");
         e2.OneTime = true;
         var eventAssignmentBuilder2 = new EventAssignmentBuilder();
         eventAssignmentBuilder2.ObjectPath = new ObjectPath("Sim|A|B|C");
         eventAssignmentBuilder2.Name = "EAB";
         eventAssignmentBuilder2.UseAsValue = false;
         eventAssignmentBuilder2.Formula = new ExplicitFormula("a-b");
         e2.AddAssignment(eventAssignmentBuilder2);

         _object1 = e1;
         _object2 = e2;
      }

      [Observation]
      public void should_report_the_differences_as_property_difference_and_not_as_missing()
      {
         _report.Count().ShouldBeEqualTo(2);
      }
   }

   public class When_comparing_EventAssingmentBuilderss_with_one_missing_a_parameter : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var e1 = new EventBuilder().WithName("Event");
         e1.OneTime = true;
         var eventAssignmentBuilder1 = new EventAssignmentBuilder();
         eventAssignmentBuilder1.ObjectPath = new ObjectPath("Sim|A|B|C");
         eventAssignmentBuilder1.Name = "EAB";
         
         eventAssignmentBuilder1.Formula = new ExplicitFormula("a+b");
         e1.AddAssignment(eventAssignmentBuilder1);
         e1.AddParameter(new Parameter().WithName("P1"));

         var e2 = new EventBuilder().WithName("Event");
         e2.OneTime = true;
         var eventAssignmentBuilder2 = new EventAssignmentBuilder();
         eventAssignmentBuilder2.ObjectPath = new ObjectPath("Sim|A|B|C");
         eventAssignmentBuilder2.Name = "EAB";
         
         eventAssignmentBuilder2.Formula = new ExplicitFormula("a+b");
         e2.AddAssignment(eventAssignmentBuilder2);

         _object1 = e1;
         _object2 = e2;
      }

      [Observation]
      public void should_report_the_differences_as_property_difference_and_not_as_missing()
      {
         _report.Count().ShouldBeEqualTo(1);
      }
   }
}