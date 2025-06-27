using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain.Builder
{
   public abstract class PathAndValueEntityBuildingBlockFromPKSim<T> : PathAndValueEntityBuildingBlock<T> where T : PathAndValueEntity
   {
      public string PKSimVersion { set; get; }
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