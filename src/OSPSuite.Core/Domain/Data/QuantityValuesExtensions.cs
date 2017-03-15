using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Data
{
   public static class QuantityValuesExtensions
   {
      public static bool IsNull(this QuantityValues quantityValues)
      {
         return quantityValues == null || quantityValues.IsAnImplementationOf<NullQuantityValues>();
      }
   }
}