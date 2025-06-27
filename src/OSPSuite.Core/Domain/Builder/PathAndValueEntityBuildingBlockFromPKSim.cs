using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain.Builder
{
   public abstract class PathAndValueEntityBuildingBlockFromPKSim<T> : PathAndValueEntityBuildingBlock<T> where T : PathAndValueEntity
   {
      public string PKSimVersion { set; get; }

      /// <summary>
      /// When a building block is created from PKSim module snapshot, this will give the origin module where the snapshot is kept.
      /// If this value is null, the building block was not imported with snapshot, or it has been changed since it was created from snapshot.
      /// </summary>
      public string SnapshotOriginModuleId { set; get; }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);

         var pathAndValueEntityBuildingBlock = source as PathAndValueEntityBuildingBlockFromPKSim<T>;
         if (pathAndValueEntityBuildingBlock == null)
            return;

         PKSimVersion = pathAndValueEntityBuildingBlock.PKSimVersion;
      }
   }
}