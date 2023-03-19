using OSPSuite.Assets;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain.Builder
{
   public class MoleculeStartValuesBuildingBlock : StartValueBuildingBlock<MoleculeStartValue>
   {
      public MoleculeStartValuesBuildingBlock()
      {
         Icon = IconNames.MOLECULE_START_VALUES;
      }

      /// <summary>
      ///    Id of the spatial structure used to create the parameter start values.
      /// </summary>
      public string SpatialStructureId { get; set; }

      /// <summary>
      ///    Id of the molecule building block used to create the parameter start values
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
         var sourceMoleculeStartValueBuildingBlock = source as MoleculeStartValuesBuildingBlock;
         if (sourceMoleculeStartValueBuildingBlock == null) return;

         MoleculeBuildingBlockId = sourceMoleculeStartValueBuildingBlock.MoleculeBuildingBlockId;
         SpatialStructureId = sourceMoleculeStartValueBuildingBlock.SpatialStructureId;
      }

   }
}