using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core
{
   public abstract class concern_for_EventAssignmentDiffBuilder : concern_for_ObjectComparer
   {
      protected Event _e1;
      protected Event _e2;
      protected EventAssignment _eventAssignment1;
      protected EventAssignment _eventAssignment2;

      protected override void Context()
      {
         base.Context();
         var sim = new Container().WithContainerType(ContainerType.Simulation).WithName("Sim");
         var cont = new Container().WithName("A").WithParentContainer(sim);
         var p = new Parameter().WithName("B").WithParentContainer(cont);
         _e1 = new Event().WithName("Event");
         _e1.OneTime = true;
         _eventAssignment1 = new EventAssignment
         {
            ObjectPath = new ObjectPath("Sim|A|B"),
            ChangedEntity = p,
            Name = "EAB"
         };
         _e1.AddAssignment(_eventAssignment1);

         _e2 = new Event().WithName("Event");
         _e2.OneTime = true;
         _eventAssignment2 = new EventAssignment
         {
            ObjectPath = new ObjectPath("Sim|A|B"),
            ChangedEntity = p,
            Name = "EAB"
         };
         _e2.AddAssignment(_eventAssignment2);


         _object1 = _e1;
         _object2 = _e2;
      }
   }

   public class When_comparing_EventAssignments_with_different_names : concern_for_EventAssignmentDiffBuilder
   {
      protected override void Context()
      {
         base.Context();
         _comparerSettings.OnlyComputingRelevant = false;
         _eventAssignment1.Name = "EAB1";
         _eventAssignment2.Name = "EAB2";
      }

      [Observation]
      public void should_report_the_differences_accordingly()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }

   public class When_comparing_EventAssignments_with_different_properties : concern_for_EventAssignmentDiffBuilder
   {
      protected override void Context()
      {
         base.Context();

         _eventAssignment1.UseAsValue = true;
         _eventAssignment1.Formula = new ExplicitFormula("a+b");

         _eventAssignment2.UseAsValue = false;
         _eventAssignment2.Formula = new ExplicitFormula("a-b");
      }

      [Observation]
      public void should_report_the_differences_as_property_difference_and_not_as_missing()
      {
         _report.Count.ShouldBeEqualTo(2);
      }
   }

   public class When_comparing_EventAssignments_with_different_object_path_but_referencing_a_property_with_the_same_normalized_path : concern_for_EventAssignmentDiffBuilder
   {
      protected override void Context()
      {
         base.Context();
         var sim = new Container().WithContainerType(ContainerType.Simulation).WithName("Sim2");
         var cont = new Container().WithName("A").WithParentContainer(sim);
         var p = new Parameter().WithName("B").WithParentContainer(cont);

         _eventAssignment2.ObjectPath = new ObjectPath("Sim2|A|B");
         _eventAssignment2.ChangedEntity = p;
      }

      [Observation]
      public void should_not_report_any_difference()
      {
         _report.Count.ShouldBeEqualTo(0);
      }
   }

   public class When_comparing_EventAssignments_with_one_missing_a_parameter : concern_for_EventAssignmentDiffBuilder
   {
      protected override void Context()
      {
         base.Context();
         _e1.Add(new Parameter().WithName("Missing"));
      }

      [Observation]
      public void should_report_the_missing_parameter()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }
}