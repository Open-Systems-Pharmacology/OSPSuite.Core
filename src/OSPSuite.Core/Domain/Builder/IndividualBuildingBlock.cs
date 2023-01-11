using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain.Builder
{
   public class IndividualBuildingBlock : PathAndValueEntityBuildingBlockFromPKSim<IndividualParameter>
   {
      public OriginDataItems OriginData { get; } = new OriginDataItems();

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);
         var sourceIndividualBuildingBlock = source as IndividualBuildingBlock;
         if (sourceIndividualBuildingBlock == null)
            return;
         
         OriginData.ValueOrigin.UpdateAllFrom(sourceIndividualBuildingBlock.OriginData.ValueOrigin);
         OriginData.UpdateFrom(sourceIndividualBuildingBlock.OriginData);
      }
   }
}