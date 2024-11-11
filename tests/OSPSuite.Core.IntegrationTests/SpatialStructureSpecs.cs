using System;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Helpers;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core
{
   public abstract class concern_for_SpatialStructure : ContextForIntegration<SpatialStructure>
   {
      protected SimulationConfiguration _simulationConfiguration;
      protected SimulationBuilder _simulationBuilder;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _simulationConfiguration = IoC.Resolve<ModelHelperForSpecs>().CreateSimulationConfiguration();
         _simulationBuilder = new SimulationBuilder(_simulationConfiguration);
      }

      protected override void Context()
      {
         sut = new SpatialStructure();
      }
   }

   public class When_getting_the_caption_of_a_building_block_without_a_module : concern_for_SpatialStructure
   {
      protected override void Context()
      {
         base.Context();
         sut.Name = "spatialStructure";
      }

      [Observation]
      public void the_display_name_is_only_the_building_block_name()
      {
         sut.DisplayName.ShouldBeEqualTo("spatialStructure");
      }
   }

   public class When_getting_the_caption_of_a_building_block_within_a_module : concern_for_SpatialStructure
   {
      private Module _module;

      protected override void Context()
      {
         base.Context();
         sut.Name = "spatialStructure";
         _module = new Module().WithName("moduleName");
         _module.Add(sut);
      }

      [Observation]
      public void the_display_name_contains_the_module_name_and_the_building_block_name()
      {
         sut.DisplayName.Contains("moduleName").ShouldBeTrue();
         sut.DisplayName.Contains("spatialStructure").ShouldBeTrue();
      }
   }

   internal class When_told_to_update_properties_from_a_source_spatial_structure_with_clone_manager_for_building_blocks : concern_for_SpatialStructure
   {
      private SpatialStructure _sourceSpatialStructure;
      private ICloneManagerForBuildingBlock _cloneManager;
      private const uint _buildingBlockVersion = 5;

      protected override void Context()
      {
         base.Context();
         _sourceSpatialStructure = _simulationBuilder.SpatialStructureAndMergeBehaviors[0].spatialStructure;
         _sourceSpatialStructure.Version = _buildingBlockVersion;
         _cloneManager = IoC.Resolve<ICloneManagerForBuildingBlock>();
      }

      protected override void Because()
      {
         sut = _cloneManager.Clone(_sourceSpatialStructure, new FormulaCache());
      }

      [Observation]
      public void all_neighbors_should_be_cloned()
      {
         sut.Neighborhoods.Each(neighbor => neighbor.FirstNeighborPath.ShouldNotBeNull());
         sut.Neighborhoods.Each(neighbor => neighbor.SecondNeighborPath.ShouldNotBeNull());
      }

      [Observation]
      public void neighbor_paths_should_be_cloned_not_shared()
      {
         var sourceNeighborhood = _sourceSpatialStructure.Neighborhoods.First();
         var clonedNeighborhood = sut.Neighborhoods.First();

         // Check that paths are different instances
         clonedNeighborhood.FirstNeighborPath.ShouldNotBeEqualTo(sourceNeighborhood.FirstNeighborPath);
         clonedNeighborhood.SecondNeighborPath.ShouldNotBeEqualTo(sourceNeighborhood.SecondNeighborPath);

         // Check that path values remain the same
         clonedNeighborhood.FirstNeighborPath.PathAsString.ShouldBeEqualTo(sourceNeighborhood.FirstNeighborPath.PathAsString);
         clonedNeighborhood.SecondNeighborPath.PathAsString.ShouldBeEqualTo(sourceNeighborhood.SecondNeighborPath.PathAsString);
      }

      [Observation]
      public void should_have_set_BuildingVersion()
      {
         sut.Version.ShouldBeEqualTo(_buildingBlockVersion);
      }
   }
}