using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Comparison;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Helpers;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.DiffBuilders
{
   public class When_comparing_two_start_value_building_blocks : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var initialConditionsBuildingBlock1 = new InitialConditionsBuildingBlock().WithName("Tada");
         var initialConditionA = new InitialCondition().WithName("MSVa");
         initialConditionA.ContainerPath = new ObjectPath("Root", "Liver", "Plasma");
         var initialConditionB = new InitialCondition().WithName("MSVb");
         initialConditionB.ContainerPath = new ObjectPath("Root", "Liver", "Plasma");
         initialConditionsBuildingBlock1.Add(initialConditionA);
         initialConditionsBuildingBlock1.Add(initialConditionB);


         var initialConditionsBuildingBlock2 = new InitialConditionsBuildingBlock().WithName("Tada");
         initialConditionA = new InitialCondition().WithName("MSVa");
         initialConditionA.ContainerPath = new ObjectPath("Root", "Liver", "Plasma");
         initialConditionB = new InitialCondition().WithName("MSVb");
         initialConditionB.ContainerPath = new ObjectPath("Root", "Liver", "Plasma");

         initialConditionsBuildingBlock2.Add(initialConditionA);
         initialConditionsBuildingBlock2.Add(initialConditionB);

         _object1 = initialConditionsBuildingBlock1;
         _object2 = initialConditionsBuildingBlock2;
      }

      [Observation]
      public void should_not_report_any_differences()
      {
         _report.ShouldBeEmpty();
      }
   }

   public class When_comparing_two_start_value_building_blocks_with_different_names : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var initialConditionsBuildingBlock1 = new InitialConditionsBuildingBlock().WithName("Tada");
         var initialConditionA = new InitialCondition().WithName("MSVa");
         initialConditionA.ContainerPath = new ObjectPath("Root", "Liver", "Plasma");
         var initialConditionB = new InitialCondition().WithName("MSVb");
         initialConditionB.ContainerPath = new ObjectPath("Root", "Liver", "Plasma");
         initialConditionsBuildingBlock1.Add(initialConditionA);
         initialConditionsBuildingBlock1.Add(initialConditionB);


         var initialConditionsBuildingBlock2 = new InitialConditionsBuildingBlock().WithName("Toto");
         initialConditionA = new InitialCondition().WithName("MSVa");
         initialConditionA.ContainerPath = new ObjectPath("Root", "Liver", "Plasma");
         initialConditionB = new InitialCondition().WithName("MSVb");
         initialConditionB.ContainerPath = new ObjectPath("Root", "Liver", "Plasma");

         initialConditionsBuildingBlock2.Add(initialConditionA);
         initialConditionsBuildingBlock2.Add(initialConditionB);

         _object1 = initialConditionsBuildingBlock1;
         _object2 = initialConditionsBuildingBlock2;
         _comparerSettings.OnlyComputingRelevant = false;
      }

      [Observation]
      public void should_report_differences()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }

   public class When_comparing_two_start_value_building_blocks_with_a_missing_start_value : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var initialConditionsBuildingBlock1 = new InitialConditionsBuildingBlock().WithName("Tada");
         var initialConditionA = new InitialCondition().WithName("MSVa");
         initialConditionA.ContainerPath = new ObjectPath("Root", "Liver", "Plasma");
         var initialConditionB = new InitialCondition().WithName("MSVb");
         initialConditionB.ContainerPath = new ObjectPath("Root", "Liver", "Plasma");
         initialConditionsBuildingBlock1.Add(initialConditionA);
         initialConditionsBuildingBlock1.Add(initialConditionB);

         var initialConditionsBuildingBlock2 = new InitialConditionsBuildingBlock().WithName("Tada");
         initialConditionA = new InitialCondition().WithName("MSVa");
         initialConditionA.ContainerPath = new ObjectPath("Root", "Liver", "Plasma");
         initialConditionB = new InitialCondition().WithName("MSVb");
         initialConditionB.ContainerPath = new ObjectPath("Root", "Liver", "Cell");

         initialConditionsBuildingBlock2.Add(initialConditionA);
         initialConditionsBuildingBlock2.Add(initialConditionB);

         _object1 = initialConditionsBuildingBlock1;
         _object2 = initialConditionsBuildingBlock2;
      }

      [Observation]
      public void should_report_differences()
      {
         _report.Count.ShouldBeEqualTo(2);
      }
   }

   public class When_comparing_two_parameter_start_value_building_blocks_with_a_missing_start_value :
      concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var parameterStartValuesBuildingBlock1 = new ParameterStartValuesBuildingBlock().WithName("Tada");

         var parameterStartValueA = new ParameterStartValue().WithName("MSVa");
         parameterStartValueA.ContainerPath = new ObjectPath("Root", "Liver", "Plasma");

         var parameterStartValueB = new ParameterStartValue().WithName("MSVb");
         parameterStartValueB.ContainerPath = new ObjectPath("Root", "Liver", "Plasma");
         parameterStartValueB.Formula = new ExplicitFormula("1+2").WithName("HELLO");

         parameterStartValuesBuildingBlock1.Add(parameterStartValueA);
         parameterStartValuesBuildingBlock1.Add(parameterStartValueB);

         var parameterStartValuesBuildingBlock2 = new ParameterStartValuesBuildingBlock().WithName("Tada");
         parameterStartValueA = new ParameterStartValue().WithName("MSVa");
         parameterStartValueA.ContainerPath = new ObjectPath("Root", "Liver", "Plasma");

         parameterStartValueB = new ParameterStartValue().WithName("MSVb");
         parameterStartValueB.ContainerPath = new ObjectPath("Root", "Liver", "Cell");
         parameterStartValueB.Value = 10;
         parameterStartValueB.Dimension = DomainHelperForSpecs.TimeDimensionForSpecs();

         parameterStartValuesBuildingBlock2.Add(parameterStartValueA);
         parameterStartValuesBuildingBlock2.Add(parameterStartValueB);

         _object1 = parameterStartValuesBuildingBlock1;
         _object2 = parameterStartValuesBuildingBlock2;
      }

      [Observation]
      public void should_report_differences()
      {
         _report.Count.ShouldBeEqualTo(2);
      }

      [Observation]
      public void should_use_the_start_value_in_display_unit_if_available()
      {
         _report[0].DowncastTo<MissingDiffItem>().PresentObjectDetails.Contains("HELLO").ShouldBeTrue();
         _report[1].DowncastTo<MissingDiffItem>().PresentObjectDetails.Contains("10").ShouldBeTrue();
      }
   }
}