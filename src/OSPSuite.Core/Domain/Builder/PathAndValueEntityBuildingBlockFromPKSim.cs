using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain.Builder
{
   public abstract class PathAndValueEntityBuildingBlockFromPKSim<T> : PathAndValueEntityBuildingBlock<T> where T : PathAndValueEntity
   {
      public string PKSimVersion { set; get; }

      /// <summary>
      /// Holds a snapshot of everything required to recreate the building block in PK-sim.
      /// Will be empty or null if the building block was not part of a SimulationTransfer
      /// </summary>
      public string Snapshot { set; get; } = string.Empty;

      public bool HasSnapshot => !string.IsNullOrEmpty(Snapshot);
      
      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);

         var pathAndValueEntityBuildingBlock = source as PathAndValueEntityBuildingBlockFromPKSim<T>;
         if (pathAndValueEntityBuildingBlock == null)
            return;

         Snapshot = pathAndValueEntityBuildingBlock.Snapshot;
         PKSimVersion = pathAndValueEntityBuildingBlock.PKSimVersion;
      }
   }
}