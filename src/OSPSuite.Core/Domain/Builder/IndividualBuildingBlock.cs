using OSPSuite.Utility.Collections;

namespace OSPSuite.Core.Domain.Builder
{
   public class IndividualBuildingBlock : PathAndValueEntityBuildingBlockFromPKSim<IndividualParameter>
   {
      public Cache<string, string> OriginData { set; get; } = new Cache<string, string>();
   }
}