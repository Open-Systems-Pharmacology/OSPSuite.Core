using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Extensions
{
   public static class WithDimensionExtensions
   {
      public static bool IsAmount(this IWithDimension objectWithDimension)
      {
         return objectWithDimension.Dimension != null &&
       (string.Equals(objectWithDimension.Dimension.Name, Constants.Dimension.AMOUNT) ||
        string.Equals(objectWithDimension.Dimension.Name, Constants.Dimension.MASS_AMOUNT));
      }

      public static bool IsConcentration(this IWithDimension objectWithDimension)
      {
         return objectWithDimension.Dimension != null &&
                (string.Equals(objectWithDimension.Dimension.Name, Constants.Dimension.MOLAR_CONCENTRATION) ||
                 string.Equals(objectWithDimension.Dimension.Name, Constants.Dimension.MASS_CONCENTRATION));
      }

      public static bool IsFraction(this IWithDimension objectWithDimension)
      {
         return objectWithDimension.Dimension != null && objectWithDimension.Dimension.Name.IsOneOf(Constants.Dimension.FRACTION);
      }
   }
}