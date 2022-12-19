using OSPSuite.Utility.Collections;

namespace OSPSuite.Core.Domain.Builder
{
   public class OriginDataCache : Cache<string, string>
   {
      public OriginDataCache() : base(onMissingKey: x => string.Empty)
      {
      }

      public ValueOrigin ValueOrigin { set; get; }
      // public CalculationMethodCache CalculationMethodCache { set; get; }
   }

   public class IndividualBuildingBlock : PathAndValueEntityBuildingBlockFromPKSim<IndividualParameter>
   {
      public OriginDataCache OriginData { get; } = new OriginDataCache();
   }
}