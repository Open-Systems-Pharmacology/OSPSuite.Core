using OSPSuite.Core.Domain.UnitSystem;
using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Core.Services
{
   public interface IDimensionHelperService
   {
      /// <summary>
      ///    Retrieves the dimension containing a unit <paramref name="unitName" />
      ///    from the supported dimensions of the list <paramref name="supportedDimensions" /> or null if not found.
      ///    This methods looks for synonyms and also supports different case (e.g. mL and ml)
      /// </summary>
      /// <returns>The dimension associated with the unit if found or null otherwise</returns>
      IDimension DimensionForUnitFromDimensionsList(string unitName, List<IDimension> supportedDimensions);
   }

   public class DimensionHelperService : IDimensionHelperService
   {
      private readonly IDimensionFactory _dimensionFactory;

      public DimensionHelperService(IDimensionFactory dimensionFactory)
      {
         _dimensionFactory = dimensionFactory;
      }

      public IDimension DimensionForUnitFromDimensionsList(string unitName, List<IDimension> supportedDimensions)
      {
         var matches = supportedDimensions.Where(x => x.SupportsUnit(unitName, ignoreCase: true)).ToList();
         if (!matches.Any())
            return null;

         //Try to find the first one that matches EXACTLY 
         return matches.FirstOrDefault(x => x.SupportsUnit(unitName, ignoreCase: false)) ?? matches.First();
      }
   }
}