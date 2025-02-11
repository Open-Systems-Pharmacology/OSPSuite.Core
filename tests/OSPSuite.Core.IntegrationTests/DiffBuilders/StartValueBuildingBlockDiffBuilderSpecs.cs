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
   public class When_comparing_two_initial_conditions_building_blocks : concern_for_ObjectComparer
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

   public class When_comparing_two_initial_conditions_building_blocks_with_different_names : concern_for_ObjectComparer
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

   public class When_comparing_two_initial_conditions_building_blocks_with_a_missing_initial_condition : concern_for_ObjectComparer
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

   public class When_comparing_two_parameter_value_building_blocks_with_a_missing_parameter_value :
      concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var parameterValuesBuildingBlock1 = new ParameterValuesBuildingBlock().WithName("Tada");

         var parameterValueA = new ParameterValue().WithName("MSVa");
         parameterValueA.ContainerPath = new ObjectPath("Root", "Liver", "Plasma");

         var parameterValueB = new ParameterValue().WithName("MSVb");
         parameterValueB.ContainerPath = new ObjectPath("Root", "Liver", "Plasma");
         parameterValueB.Formula = new ExplicitFormula("1+2").WithName("HELLO");

         parameterValuesBuildingBlock1.Add(parameterValueA);
         parameterValuesBuildingBlock1.Add(parameterValueB);

         var parameterValuesBuildingBlock2 = new ParameterValuesBuildingBlock().WithName("Tada");
         parameterValueA = new ParameterValue().WithName("MSVa");
         parameterValueA.ContainerPath = new ObjectPath("Root", "Liver", "Plasma");

         parameterValueB = new ParameterValue().WithName("MSVb");
         parameterValueB.ContainerPath = new ObjectPath("Root", "Liver", "Cell");
         parameterValueB.Value = 10;
         parameterValueB.Dimension = DomainHelperForSpecs.TimeDimensionForSpecs();

         parameterValuesBuildingBlock2.Add(parameterValueA);
         parameterValuesBuildingBlock2.Add(parameterValueB);

         _object1 = parameterValuesBuildingBlock1;
         _object2 = parameterValuesBuildingBlock2;
      }

      [Observation]
      public void should_report_differences()
      {
         _report.Count.ShouldBeEqualTo(2);
      }

      [Observation]
      public void should_use_the_parameter_value_in_display_unit_if_available()
      {
         _report[0].DowncastTo<MissingDiffItem>().PresentObjectDetails.Contains("HELLO").ShouldBeTrue();
         _report[1].DowncastTo<MissingDiffItem>().PresentObjectDetails.Contains("10").ShouldBeTrue();
      }
   }

   public class When_comparing_parameter_values_building_blocks_with_distributed_parameters_that_have_sub_parameter_differences : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var initialConditionsBuildingBlock1 = new InitialConditionsBuildingBlock().WithName("Tada");
         var initialConditionA = new InitialCondition().WithName("MSVa");
         initialConditionA.ContainerPath = new ObjectPath("Root", "Liver", "Plasma");
         initialConditionA.DistributionType = DistributionType.Normal;
         var initialConditionB = new InitialCondition().WithName("MSVb");
         initialConditionB.ContainerPath = new ObjectPath("Root", "Liver", "Plasma");
         initialConditionsBuildingBlock1.Add(initialConditionA);
         initialConditionsBuildingBlock1.Add(initialConditionB);
         initialConditionsBuildingBlock1.Add(new InitialCondition
         {
            Path = new ObjectPath("Root", "Liver", "Plasma", "MSVa", "Median"),
         });


         var initialConditionsBuildingBlock2 = new InitialConditionsBuildingBlock().WithName("Tada");
         initialConditionA = new InitialCondition().WithName("MSVa");
         initialConditionA.DistributionType = DistributionType.Normal;
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

   public class When_comparing_parameter_values_building_blocks_with_distributed_parameters_that_have_different_distribution_types : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var initialConditionsBuildingBlock1 = new InitialConditionsBuildingBlock().WithName("Tada");
         var initialConditionA = new InitialCondition().WithName("MSVa");
         initialConditionA.ContainerPath = new ObjectPath("Root", "Liver", "Plasma");
         initialConditionA.DistributionType = DistributionType.Normal;
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
      public void should_report_the_different_distribution_type()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }
}