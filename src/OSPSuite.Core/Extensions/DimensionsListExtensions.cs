using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Extensions
{
   public static class DimensionsListExtensions
   {
      public static IDimension DimensionForUnit(this IEnumerable<IDimension> dimensionList, string unitName)
      {
         var matches = dimensionList.Where(x => x.SupportsUnit(unitName, ignoreCase: true)).ToList();
         if (!matches.Any())
            return null;

         //Try to find the first one that matches EXACTLY 
         return matches.FirstOrDefault(x => x.SupportsUnit(unitName, ignoreCase: false)) ?? matches.First();
      }
   }
}