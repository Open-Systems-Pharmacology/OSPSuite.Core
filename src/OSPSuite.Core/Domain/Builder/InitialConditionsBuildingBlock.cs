using OSPSuite.Assets;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain.Builder
{
   public class InitialConditionsBuildingBlock : StartValueBuildingBlock<InitialCondition>
   {
      public InitialConditionsBuildingBlock()
      {
         Icon = IconNames.INITIAL_CONDITIONS;
      }

      /// <summary>
      ///    Id of the spatial structure used to create the parameter values.
      /// </summary>
      public string SpatialStructureId { get; set; }

      /// <summary>
      ///    Id of the molecule building block used to create the parameter values
      /// </summary>
      public string MoleculeBuildingBlockId { get; set; }

      public bool Uses(IBuildingBlock buildingBlock)
      {
         return refersTo(buildingBlock, SpatialStructureId)
                || refersTo(buildingBlock, MoleculeBuildingBlockId);
      }

      private bool refersTo(IBuildingBlock buildingBlock, string idToCheck)
      {
         return string.Equals(idToCheck, buildingBlock.Id);
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);
         var sourceInitialConditionsBuildingBlock = source as InitialConditionsBuildingBlock;
         if (sourceInitialConditionsBuildingBlock == null) return;

         MoleculeBuildingBlockId = sourceInitialConditionsBuildingBlock.MoleculeBuildingBlockId;
         SpatialStructureId = sourceInitialConditionsBuildingBlock.SpatialStructureId;
      }
   }
}