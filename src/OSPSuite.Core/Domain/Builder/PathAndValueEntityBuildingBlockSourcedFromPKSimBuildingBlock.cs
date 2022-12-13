using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain.Builder
{
   public abstract class PathAndValueEntityBuildingBlockSourcedFromPKSimBuildingBlock<T> : PathAndValueEntityBuildingBlock<T> where T : class, IWithPath, IObjectBase
   {
      public string PKSimVersion { set; get; }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);

         var pathAndValueEntityBuildingBlock = source as PathAndValueEntityBuildingBlockSourcedFromPKSimBuildingBlock<T>;
         if (pathAndValueEntityBuildingBlock == null)
            return;

         PKSimVersion = pathAndValueEntityBuildingBlock.PKSimVersion;
      }
   }
}