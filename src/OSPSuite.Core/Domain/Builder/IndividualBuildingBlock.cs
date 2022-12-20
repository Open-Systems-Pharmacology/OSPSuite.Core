namespace OSPSuite.Core.Domain.Builder
{
   public class IndividualBuildingBlock : PathAndValueEntityBuildingBlockFromPKSim<IndividualParameter>
   {
      public OriginDataItems OriginData { get; } = new OriginDataItems();
   }
}