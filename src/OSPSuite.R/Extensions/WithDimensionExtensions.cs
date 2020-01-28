using OSPSuite.Core.Domain;

namespace OSPSuite.R.Extensions
{
   /// <summary>
   /// Required to support another logic for R since we have issue with unicode characters
   /// See https://github.com/Open-Systems-Pharmacology/OSPSuite-R/issues/233
   /// </summary>
   public static class WithDimensionExtensions
   {
      /// <summary>
      /// Converts the value given in the unit at index <paramref name="unitIndex"/> into base unit
      /// </summary>
      /// <param name="withDimension">Object with dimension used for conversion</param>
      /// <param name="value">Value in unit defined at <paramref name="unitIndex"/></param>
      /// <param name="unitIndex">1-based index of the unit in the units array</param>
      /// <returns>The value converted in base unit</returns>
      public static double ConvertToBaseUnit(this IWithDimension withDimension, double value, int unitIndex)
      {
         return withDimension.ConvertToBaseUnit(value, withDimension.Dimension.UnitAt(unitIndex - 1));
      }


      /// <summary>
      /// Converts the value given in base unit to the unit at index  <paramref name="unitIndex"/> 
      /// </summary>
      /// <param name="withDimension">Object with dimension used for conversion</param>
      /// <param name="valueInBaseUnit">Value in base unit to convert in unit defined at <paramref name="unitIndex"/></param>
      /// <param name="unitIndex">1-based index of the unit in the units array</param>
      /// <returns>The value converted in unit</returns>
      public static double ConvertToUnit(this IWithDimension withDimension, double valueInBaseUnit, int unitIndex)
      {
         return withDimension.ConvertToUnit(valueInBaseUnit, withDimension.Dimension.UnitAt(unitIndex-1));
      }
   }
}