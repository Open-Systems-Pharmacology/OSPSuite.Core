using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain
{
   public static class QuantityExtensions
   {
      public static T WithQuantityType<T>(this T quantity, QuantityType quantityType) where T : IQuantity
      {
         quantity.QuantityType = quantityType;
         return quantity;
      }

      public static double ConvertToUnit<T>(this T quantity, Unit unit) where T : IQuantity
      {
         return quantity.ConvertToUnit(quantity.Value, unit);
      }

      public static double ConvertToUnit<T>(this T quantity, string unit) where T : IQuantity
      {
         return quantity.ConvertToUnit(quantity.Value, unit);
      }
   }
}