using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain.Builder
{
   public class IndividualBuildingBlock : PathAndValueEntityBuildingBlockFromPKSim<IndividualParameter>
   {
      public ExtendedProperties OriginData { get; } = new ExtendedProperties();

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);
         var sourceIndividualBuildingBlock = source as IndividualBuildingBlock;
         if (sourceIndividualBuildingBlock == null)
            return;

         OriginData.UpdateFrom(sourceIndividualBuildingBlock.OriginData);
      }
   }
}