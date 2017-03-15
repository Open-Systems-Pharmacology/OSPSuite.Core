using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Extensions
{
   public static class UnitExtensions
   {
      public static bool IsWeeks(this Unit inputUnit)
      {
         return string.Equals(inputUnit.Name, Constants.Dimension.Units.Weeks);
      }

      public static bool IsMonths(this Unit inputUnit)
      {
         return string.Equals(inputUnit.Name, Constants.Dimension.Units.Months);
      }

      public static bool IsYears(this Unit inputUnit)
      {
         return string.Equals(inputUnit.Name, Constants.Dimension.Units.Years);
      }

      public static bool IsDays(this Unit inputUnit)
      {
         return string.Equals(inputUnit.Name, Constants.Dimension.Units.Days);
      }

      public static bool IsHours(this Unit inputUnit)
      {
         return string.Equals(inputUnit.Name, Constants.Dimension.Units.Hours);
      }

      public static bool IsMinutes(this Unit inputUnit)
      {
         return string.Equals(inputUnit.Name, Constants.Dimension.Units.Minutes);
      }
   }
}