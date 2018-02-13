using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core
{
   public class When_comparing_two_start_value_building_blocks : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var moleculeStartValuesBuildingBlock1 = new MoleculeStartValuesBuildingBlock().WithName("Tada");
         var moleculeStartValueA = new MoleculeStartValue().WithName("MSVa");
         moleculeStartValueA.ContainerPath = new ObjectPath("Root", "Liver", "Plasma");
         var moleculeStartValueB = new MoleculeStartValue().WithName("MSVb");
         moleculeStartValueB.ContainerPath = new ObjectPath("Root", "Liver", "Plasma");
         moleculeStartValuesBuildingBlock1.Add(moleculeStartValueA);
         moleculeStartValuesBuildingBlock1.Add(moleculeStartValueB);


         var moleculeStartValuesBuildingBlock2 = new MoleculeStartValuesBuildingBlock().WithName("Tada");
         moleculeStartValueA = new MoleculeStartValue().WithName("MSVa");
         moleculeStartValueA.ContainerPath = new ObjectPath("Root", "Liver", "Plasma");
         moleculeStartValueB = new MoleculeStartValue().WithName("MSVb");
         moleculeStartValueB.ContainerPath = new ObjectPath("Root", "Liver", "Plasma");

         moleculeStartValuesBuildingBlock2.Add(moleculeStartValueA);
         moleculeStartValuesBuildingBlock2.Add(moleculeStartValueB);

         _object1 = moleculeStartValuesBuildingBlock1;
         _object2 = moleculeStartValuesBuildingBlock2;
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
         var moleculeStartValuesBuildingBlock1 = new MoleculeStartValuesBuildingBlock().WithName("Tada");
         var moleculeStartValueA = new MoleculeStartValue().WithName("MSVa");
         moleculeStartValueA.ContainerPath = new ObjectPath("Root", "Liver", "Plasma");
         var moleculeStartValueB = new MoleculeStartValue().WithName("MSVb");
         moleculeStartValueB.ContainerPath = new ObjectPath("Root", "Liver", "Plasma");
         moleculeStartValuesBuildingBlock1.Add(moleculeStartValueA);
         moleculeStartValuesBuildingBlock1.Add(moleculeStartValueB);


         var moleculeStartValuesBuildingBlock2 = new MoleculeStartValuesBuildingBlock().WithName("Toto");
         moleculeStartValueA = new MoleculeStartValue().WithName("MSVa");
         moleculeStartValueA.ContainerPath = new ObjectPath("Root", "Liver", "Plasma");
         moleculeStartValueB = new MoleculeStartValue().WithName("MSVb");
         moleculeStartValueB.ContainerPath = new ObjectPath("Root", "Liver", "Plasma");

         moleculeStartValuesBuildingBlock2.Add(moleculeStartValueA);
         moleculeStartValuesBuildingBlock2.Add(moleculeStartValueB);

         _object1 = moleculeStartValuesBuildingBlock1;
         _object2 = moleculeStartValuesBuildingBlock2;
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
         var moleculeStartValuesBuildingBlock1 = new MoleculeStartValuesBuildingBlock().WithName("Tada");
         var moleculeStartValueA = new MoleculeStartValue().WithName("MSVa");
         moleculeStartValueA.ContainerPath = new ObjectPath("Root", "Liver", "Plasma");
         var moleculeStartValueB = new MoleculeStartValue().WithName("MSVb");
         moleculeStartValueB.ContainerPath = new ObjectPath("Root", "Liver", "Plasma");
         moleculeStartValuesBuildingBlock1.Add(moleculeStartValueA);
         moleculeStartValuesBuildingBlock1.Add(moleculeStartValueB);

         var moleculeStartValuesBuildingBlock2 = new MoleculeStartValuesBuildingBlock().WithName("Tada");
         moleculeStartValueA = new MoleculeStartValue().WithName("MSVa");
         moleculeStartValueA.ContainerPath = new ObjectPath("Root", "Liver", "Plasma");
         moleculeStartValueB = new MoleculeStartValue().WithName("MSVb");
         moleculeStartValueB.ContainerPath = new ObjectPath("Root", "Liver", "Cell");

         moleculeStartValuesBuildingBlock2.Add(moleculeStartValueA);
         moleculeStartValuesBuildingBlock2.Add(moleculeStartValueB);

         _object1 = moleculeStartValuesBuildingBlock1;
         _object2 = moleculeStartValuesBuildingBlock2;
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
         parameterStartValuesBuildingBlock1.Add(parameterStartValueA);
         parameterStartValuesBuildingBlock1.Add(parameterStartValueB);

         var parameterStartValuesBuildingBlock2 = new ParameterStartValuesBuildingBlock().WithName("Tada");
         parameterStartValueA = new ParameterStartValue().WithName("MSVa");
         parameterStartValueA.ContainerPath = new ObjectPath("Root", "Liver", "Plasma");
         parameterStartValueB = new ParameterStartValue().WithName("MSVb");
         parameterStartValueB.ContainerPath = new ObjectPath("Root", "Liver", "Cell");

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
   }
}