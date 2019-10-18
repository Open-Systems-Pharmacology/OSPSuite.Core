using OSPSuite.Core.Domain.Formulas;
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

      public static double ConvertToUnit(this IQuantity quantity, Unit unit) => quantity.ConvertToUnit(quantity.Value, unit);

      public static double ConvertToUnit(this IQuantity quantity, string unit) => quantity.ConvertToUnit(quantity.Value, unit);

      /// <summary>
      /// Returns the formula defined in the quantity. Required for R Binding
      /// </summary>
      public static IFormula GetFormula(this IQuantity quantity) => quantity?.Formula;
   }
}