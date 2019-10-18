using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Helpers;

namespace OSPSuite.Core
{
   public abstract class concern_for_SpatialStructure : ContextForIntegration<ISpatialStructure>
   {
      protected IBuildConfiguration _buildConfiguration;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _buildConfiguration = IoC.Resolve<ModelHelperForSpecs>().CreateBuildConfiguration();
      }

      protected override void Context()
      {
         sut = new SpatialStructure();
      }
   }

   public class When_told_to_update_properties_from_a_source_spatial_structure_with_clone_manager_for_buildingblocks : concern_for_SpatialStructure
   {
      private ISpatialStructure _sourceSpatialStructure;
      private ICloneManagerForBuildingBlock _cloneManager;
      private const uint _buildingBlockVersion = 5;

      protected override void Context()
      {
         base.Context();
         _sourceSpatialStructure = _buildConfiguration.SpatialStructure;
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
         sut.Neighborhoods.Each(neighbor => neighbor.FirstNeighbor.ShouldNotBeNull());
         sut.Neighborhoods.Each(neighbor => neighbor.SecondNeighbor.ShouldNotBeNull());
      }

      [Observation]
      public void should_have_set_BuildingVersion()
      {
         sut.Version.ShouldBeEqualTo(_buildingBlockVersion);
      }
   }

   public class When_enumerating_over_all_the_container_defined_in_a_spatial_structure : concern_for_SpatialStructure
   {
      [Observation]
      public void should_not_return_undefined_container()
      {
         sut.Each(x=>x.ShouldNotBeNull());
      }
   }
}