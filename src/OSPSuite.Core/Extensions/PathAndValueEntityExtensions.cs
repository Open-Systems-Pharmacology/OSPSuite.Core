using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Extensions
{
   public static class PathAndValueEntityExtensions
   {
      public static bool IsDistributed(this PathAndValueEntity entity) => entity.DistributionType != null;
   }
}
