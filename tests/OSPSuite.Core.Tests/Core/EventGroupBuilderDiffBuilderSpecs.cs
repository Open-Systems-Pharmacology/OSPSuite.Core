using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core
{
   public class When_comparing_EventGroups_having_different_criteria : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var eg1 = new EventGroupBuilder().WithName("Events");
         var e1 = new EventBuilder().WithName("Start").WithParentContainer(eg1);
         eg1.SourceCriteria = Create.Criteria(x => x.With("A").And.Not("B"));


         var eg2 = new EventGroupBuilder().WithName("Events");
         var e2 = new EventBuilder().WithName("Start").WithParentContainer(eg2);
         eg2.SourceCriteria = Create.Criteria(x => x.With("B").And.Not("A"));
         _object1 = eg1;
         _object2 = eg2;
      }

      [Observation]
      public void should_report_the_differences_accordingly()
      {
         _report.Count().ShouldBeEqualTo(1);
      }
   }

   public class When_comparing_EventGroups_having_different_events : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var eg1 = new EventGroupBuilder().WithName("Events");
         var e1 = new EventBuilder().WithName("Start").WithParentContainer(eg1);


         var eg2 = new EventGroupBuilder().WithName("Events");
         var e2 = new EventBuilder().WithName("Stop").WithParentContainer(eg2);

         _object1 = eg1;
         _object2 = eg2;
      }

      [Observation]
      public void should_report_the_differences_accordingly()
      {
         _report.Count().ShouldBeEqualTo(2);
      }
   }

   public class When_comparing_ApplicationBuilder_with_different_applicated_molecules : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var eg1 = new ApplicationBuilder().WithName("Events");
         eg1.MoleculeName = "Drug";
         //var e1 = new EventBuilder().WithName("Start").WithParentContainer(eg1);


         var eg2 = new ApplicationBuilder().WithName("Events");
         eg2.MoleculeName = "Metab";
         //var e2 = new EventBuilder().WithName("Stop").WithParentContainer(eg2);

         _object1 = eg1;
         _object2 = eg2;
      }

      [Observation]
      public void should_report_the_differences_accordingly()
      {
         _report.Count().ShouldBeEqualTo(1);
      }
   }

   public class When_comparing_ApplicationBuilder_with_different_application_molecules : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var eg1 = new ApplicationBuilder().WithName("Events");
         eg1.MoleculeName = "Drug";
         var am1 = new ApplicationMoleculeBuilder().WithName("Start").WithParentContainer(eg1);
         am1.RelativeContainerPath = new ObjectPath("..", "C1");


         var eg2 = new ApplicationBuilder().WithName("Events");
         eg2.MoleculeName = "Drug";
         var am2 = new ApplicationMoleculeBuilder().WithName("Start").WithParentContainer(eg2);
         am2.RelativeContainerPath = new ObjectPath("..", "C2");

         _object1 = eg1;
         _object2 = eg2;
      }

      [Observation]
      public void should_report_the_differences_accordingly()
      {
         _report.Count().ShouldBeEqualTo(2);
      }
   }

   public class When_comparing_ApplicationMoleculeBuilder_with_different_paths_and_formulas : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var amb1 = new ApplicationMoleculeBuilder().WithName("drug");
         amb1.RelativeContainerPath = new ObjectPath("..", "C1");
         amb1.Formula = new ExplicitFormula("0");


         var amb2 = new ApplicationMoleculeBuilder().WithName("drug");
         amb2.RelativeContainerPath = new ObjectPath("..", "C2");
         amb2.Formula = new ExplicitFormula("1");


         _object1 = amb1;
         _object2 = amb2;
      }

      [Observation]
      public void should_report_the_differences_accordingly()
      {
         _report.Count().ShouldBeEqualTo(2);
      }
   }

   public class When_comparing_EventGroup_with_different_properties : concern_for_ObjectComparer
   {
      private IEventAssignmentBuilder _eventAssignmentBuilder1;
      private IParameter _p1;
      private ExplicitFormula _explicitFormula1;
      private EventBuilder _e1;

      protected override void Context()
      {
         base.Context();
         var eg1 = new EventGroupBuilder().WithName("Events");
         var app1 = new ApplicationBuilder().WithName("App").WithParentContainer(eg1);
         app1.MoleculeName = "Drug";
         var c1 = new Container().WithName("ProtocolSchemaItem").WithParentContainer(app1);
         _e1 = new EventBuilder().WithName("Event").WithParentContainer(app1);
         _eventAssignmentBuilder1 = new EventAssignmentBuilder();
         _eventAssignmentBuilder1.ObjectPath = new ObjectPath("Sim|A|B|C");
         _eventAssignmentBuilder1.Name = "EAB";
         _eventAssignmentBuilder1.UseAsValue = true;
         _eventAssignmentBuilder1.Formula = new ExplicitFormula("a+b");
         _e1.AddAssignment(_eventAssignmentBuilder1);
         _explicitFormula1 = new ExplicitFormula("1+2");
         _p1 = new Parameter().WithName("P").WithFormula(_explicitFormula1).WithParentContainer(c1);

         var eg2 = new EventGroupBuilder().WithName("Events");
         var app2 = new ApplicationBuilder().WithName("App").WithParentContainer(eg2);
         app2.MoleculeName = "Drug";
         var c2 = new Container().WithName("ProtocolSchemaItem").WithParentContainer(app2);
         var e2 = new EventBuilder().WithName("Event").WithParentContainer(app2);
         var eventAssignmentBuilder2 = new EventAssignmentBuilder();
         eventAssignmentBuilder2.ObjectPath = new ObjectPath("Sim|A|B|C");
         eventAssignmentBuilder2.Name = "EAB";
         eventAssignmentBuilder2.UseAsValue = false;
         eventAssignmentBuilder2.Formula = new ExplicitFormula("a+b");
         e2.AddAssignment(eventAssignmentBuilder2);
         var p2 = new Parameter().WithName("P").WithFormula(new ExplicitFormula("2+1")).WithParentContainer(c2);

         _object1 = eg1;
         _object2 = eg2;
      }

      [Observation]
      public void should_report_the_differences_accordingly_with_the_right_parent()
      {
         _report.Count().ShouldBeEqualTo(2);
         var diffItem = _report.FirstOrDefault(item => item.Object1.Equals(_eventAssignmentBuilder1));
         diffItem.ShouldNotBeNull();
         diffItem.CommonAncestor.ShouldBeEqualTo(_e1);
         diffItem = _report.FirstOrDefault(item => item.Object1.Equals(_explicitFormula1));
         diffItem.ShouldNotBeNull();
         diffItem.CommonAncestor.ShouldBeEqualTo(_p1);
      }
   }
}