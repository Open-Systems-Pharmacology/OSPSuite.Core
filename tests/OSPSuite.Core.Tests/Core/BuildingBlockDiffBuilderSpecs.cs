using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core
{
   public class When_comparing_two_building_blocks : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var moleculeBuildingBlock1 = new MoleculeBuildingBlock().WithName("Tada");
         var moleculeBuilderA = new MoleculeBuilder().WithName("MSVa");
         var moleculeBuilderB = new MoleculeBuilder().WithName("MSVb");
         
         moleculeBuildingBlock1.Add(moleculeBuilderA);
         moleculeBuildingBlock1.Add(moleculeBuilderB);


         var moleculeBuildingBlock2 = new MoleculeBuildingBlock().WithName("Tada");
         moleculeBuilderA = new MoleculeBuilder().WithName("MSVa");
         moleculeBuilderB = new MoleculeBuilder().WithName("MSVb");
         

         moleculeBuildingBlock2.Add(moleculeBuilderA);
         moleculeBuildingBlock2.Add(moleculeBuilderB);

         _object1 = moleculeBuildingBlock1;
         _object2 = moleculeBuildingBlock2;
      }

      [Observation]
      public void should_not_report_any_differences()
      {
         _report.ShouldBeEmpty();
      }
   }

   public class When_comparing_two_building_blocks_with_different_names : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var moleculeBuildingBlock1 = new MoleculeBuildingBlock().WithName("Tada");
         var moleculeBuilderA = new MoleculeBuilder().WithName("MSVa");
         var moleculeBuilderB = new MoleculeBuilder().WithName("MSVb");

         moleculeBuildingBlock1.Add(moleculeBuilderA);
         moleculeBuildingBlock1.Add(moleculeBuilderB);


         var moleculeBuildingBlock2 = new MoleculeBuildingBlock().WithName("Toto");
         moleculeBuilderA = new MoleculeBuilder().WithName("MSVa");
         moleculeBuilderB = new MoleculeBuilder().WithName("MSVb");


         moleculeBuildingBlock2.Add(moleculeBuilderA);
         moleculeBuildingBlock2.Add(moleculeBuilderB);

         _object1 = moleculeBuildingBlock1;
         _object2 = moleculeBuildingBlock2;

         _comparerSettings.OnlyComputingRelevant = false;
      }

      [Observation]
      public void should_report_differences()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }

   public class When_comparing_two_molecule_building_blocks_with_different_versions_and_at_least_one_local_parameter_ : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var moleculeBuildingBlock1 = new MoleculeBuildingBlock().WithName("X");
         moleculeBuildingBlock1.Version = 1;
         var moleculeBuilderA = new MoleculeBuilder().WithName("MSVa");
         moleculeBuilderA.AddParameter(new Parameter().WithMode(ParameterBuildMode.Local));

         moleculeBuildingBlock1.Add(moleculeBuilderA);


         var moleculeBuildingBlock2 = new MoleculeBuildingBlock().WithName("X");
         moleculeBuildingBlock2.Version = 1;
         moleculeBuilderA = new MoleculeBuilder().WithName("MSVa");


         moleculeBuildingBlock2.Add(moleculeBuilderA);

         _object1 = moleculeBuildingBlock1;
         _object2 = moleculeBuildingBlock2;

      }

      [Observation]
      public void should_report_differences()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }
  
   public class When_comparing_two_building_blocks_with_missing_child_builder : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var moleculeBuildingBlock1 = new MoleculeBuildingBlock().WithName("Tada");
         var moleculeBuilderA = new MoleculeBuilder().WithName("DrugA");
         var moleculeBuilderB = new MoleculeBuilder().WithName("DrugB");

         moleculeBuildingBlock1.Add(moleculeBuilderA);
         moleculeBuildingBlock1.Add(moleculeBuilderB);


         var moleculeBuildingBlock2 = new MoleculeBuildingBlock().WithName("Tada");
         moleculeBuilderA = new MoleculeBuilder().WithName("DrugA");
         moleculeBuilderB = new MoleculeBuilder().WithName("DrugC");

         moleculeBuildingBlock2.Add(moleculeBuilderA);
         moleculeBuildingBlock2.Add(moleculeBuilderB);

         _object1 = moleculeBuildingBlock1;
         _object2 = moleculeBuildingBlock2;
      }

      [Observation]
      public void should_report_differences()
      {
         _report.Count.ShouldBeEqualTo(2);
      }
   }

   public class When_comparing_two_spatial_structures_with_missing_child_and_Top_container : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var spatialStructure1 = new SpatialStructure().WithName("Tada");
         var containerA = new Container().WithName("ContainerA");
         var childA1 = new Container().WithName("1").WithParentContainer(containerA);
         var containerB = new Container().WithName("ContainerB");

         spatialStructure1.Add(containerA);
         spatialStructure1.Add(containerB);

         var spatialStructure2 = new SpatialStructure().WithName("Tada");
         containerA = new Container().WithName("ContainerA");
         containerB = new Container().WithName("ContainerC");

         spatialStructure2.Add(containerA);
         spatialStructure2.Add(containerB);

         _object1 = spatialStructure1;
         _object2 = spatialStructure2;
      }

      [Observation]
      public void should_report_differences()
      {
         _report.Count.ShouldBeEqualTo(3);
      }
   }

   public class When_comparing_two_observer_building_block_with_different_observer_types_with_same_name_builder : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         
         base.Context();
         var observerBuildingBlock1 = new ObserverBuildingBlock().WithName("Tada");
          IObserverBuilder observerBuilderA = new ContainerObserverBuilder().WithName("OBS");
         var observerBuilderB = new ContainerObserverBuilder().WithName("ObsB");

         observerBuildingBlock1.Add(observerBuilderA);
         observerBuildingBlock1.Add(observerBuilderB);

         var observerBuildingBlock2 = new ObserverBuildingBlock().WithName("Tada");
         observerBuilderA = new AmountObserverBuilder().WithName("OBS");
         observerBuilderB = new ContainerObserverBuilder().WithName("ObsB");

         observerBuildingBlock2.Add(observerBuilderA);
         observerBuildingBlock2.Add(observerBuilderB);

         _object1 = observerBuildingBlock1;
         _object2 = observerBuildingBlock2;
      }

      [Observation]
      public void should_report_differences()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }

   public class When_comparing_two_event_group_building_block_with_different_objects_on_top_level : concern_for_ObjectComparer
   {
      protected override void Context()
      {

         base.Context();
         var observerBuildingBlock1 = new EventGroupBuildingBlock().WithName("Tada");
         IEventGroupBuilder observerBuilderA = new ApplicationBuilder().WithName("EGA");
         var observerBuilderB = new ApplicationBuilder().WithName("EGB");

         observerBuildingBlock1.Add(observerBuilderA);
         observerBuildingBlock1.Add(observerBuilderB);

         var observerBuildingBlock2 = new EventGroupBuildingBlock().WithName("Tada");
         observerBuilderA = new EventGroupBuilder().WithName("EGA");
         observerBuilderB = new ApplicationBuilder().WithName("EGB");

         observerBuildingBlock2.Add(observerBuilderA);
         observerBuildingBlock2.Add(observerBuilderB);

         _object1 = observerBuildingBlock1;
         _object2 = observerBuildingBlock2;
      }

      [Observation]
      public void should_report_differences()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }
}